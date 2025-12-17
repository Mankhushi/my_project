using Asp.Versioning;
using JobPortalAPI;
using JobPortalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using MSINS_API.Data;
using MSINS_API.POCO;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services;
using MSINS_API.Services.Implementation;
using MSINS_API.Services.Interface;
using Serilog;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Abstractions;
using MSINS_API.Models.Response;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.HttpOverrides;
using System.Threading.RateLimiting;
using MSINS_API.Controllers;
using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json;
using MSINS_API.Middlewares;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using MSINS_API.Authorization;
using MSINS_API.Configuration;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);



// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Set the minimum log level for debugging
    .WriteTo.Console() // Log to console
    .WriteTo.File("Logs/app-log.txt", rollingInterval: RollingInterval.Day) // Log to file
    .CreateLogger();

// Add Serilog to the logging pipeline
builder.Logging.ClearProviders(); // Remove default providers
builder.Logging.AddSerilog(); // Add Serilog

builder.Services.AddAutoMapper(typeof(MappingProfile));

// 1️ Add services for Rate Limiting
// Add IpRateLimiting services
//builder.Services.AddMemoryCache();
//builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
//builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
//builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
//builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
//builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// Bind the rate limiter settings from configuration

builder.Services.Configure<RateLimiterSettings>(builder.Configuration.GetSection("RateLimiterSettings"));

var rateLimiterSettings = builder.Configuration
    .GetSection("RateLimiterSettings")
    .Get<RateLimiterSettings>();

// Configure rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";


        return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = rateLimiterSettings.PermitLimit,
            Window = TimeSpan.FromSeconds(rateLimiterSettings.WindowSeconds),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = rateLimiterSettings.QueueLimit // Set to 0 to avoid queuing
        });
    });
    options.RejectionStatusCode = rateLimiterSettings.RejectionStatusCode; // ✅ Ensure this is set correctly

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = rateLimiterSettings.RejectionStatusCode;
        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.Headers["Retry-After"] = rateLimiterSettings.RetryAfter.ToString() + " Sec";

        var response = new
        {
            message = "Rate limit exceeded. Try again later.",
            status = rateLimiterSettings.RejectionStatusCode,
            retryAfter = rateLimiterSettings.RetryAfter + " Sec",
            allowedRequests = rateLimiterSettings.PermitLimit
        };

        await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(response), token);
    };

});


builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IConfiguration>().GetSection("BaseUrls").Get<BaseUrlSettings>());


// Register the PartitionedRateLimiter<HttpContext> as a singleton service
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<RateLimiterOptions>>().Value;
    return options.GlobalLimiter;
});


// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keeps PascalCase
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    // Use whatever reader you want
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                    new HeaderApiVersionReader("x-api-version"),
                                    new MediaTypeApiVersionReader("x-api-version"));
}).AddMvc().AddApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
}); // Nuget Package: Asp.Versioning.Mvc




// Configure Swagger with Bearer Authentication
builder.Services.AddSwaggerGen(options =>
{
    // Register API version descriptions
    var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = $"MSINS_API {description.ApiVersion}",
            Version = description.ApiVersion.ToString()
        });
    }

    //options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API", Version = "v1" });

    // Enable annotations
    options.EnableAnnotations();

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\"",
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });



    // Add XML comments
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "MSINS_API.xml"); // Replace 'YourProjectName' with your actual project name
    options.IncludeXmlComments(xmlFile);
});



// connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MSINSDbContext>(options => options.UseSqlServer(connectionString));


// Bind JWT settings from appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add authentication and authorization services
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,  // Ensures token expiration is validated
        ClockSkew = TimeSpan.Zero, // Optional: removes default 5-minute clock skew for testing
        ValidIssuers = jwtSettings.ValidIssuers,
        ValidAudiences = jwtSettings.ValidAudiences,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    options.Events = new JwtBearerEvents
    {
        // Handle expired token
        OnChallenge = async context =>
        {
            if (context.AuthenticateFailure is SecurityTokenExpiredException expiredException)
            {
                context.HandleResponse(); // Prevent default response

                var expiredAtUtc = expiredException.Expires;
                var expiredAtIST = expiredAtUtc.AddHours(5).AddMinutes(30); // Convert to IST

                var errorResponse = new AuthResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"The token expired at '{expiredAtIST:yyyy-MM-dd HH:mm:ss} IST'.",
                    Token = null,
                    Expiration = expiredAtIST,
                    RefreshToken = null
                };

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    };
});

builder.Services.AddHttpContextAccessor(); // Required for route values
builder.Services.AddSingleton<IAuthorizationHandler, StartUpAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    // OLD: Policy that requires 'Admin' role using old 'Role' claim
    // options.AddPolicy("AdminPolicy", policy =>
    //     policy.RequireAssertion(context =>
    //         context.User.HasClaim(c => c.Type == "Role" && c.Value == "admin")));
    // NEW: Policy using standard role claim for Admin
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireAssertion(context =>
        {
            var rolesClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "roles").ToList();
            if (!rolesClaims.Any()) return false;
            
            foreach (var claim in rolesClaims)
            {
                var value = claim.Value.ToUpper();
                if (value.Contains("ADMIN"))
                {
                    return true;
                }
            }
            return false;
        }));

    // OLD: Policy that requires 'Guest' role using old 'Role' claim
    // options.AddPolicy("GuestPolicy", policy =>
    //     policy.RequireAssertion(context =>
    //         context.User.HasClaim(c => c.Type == "Role" && c.Value == "Guest")));
    // NEW: Policy using standard role claim for Guest
    options.AddPolicy("GuestPolicy", policy =>
        policy.RequireAssertion(context =>
        {
            var rolesClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "roles").ToList();
            if (!rolesClaims.Any()) return false;
            
            foreach (var claim in rolesClaims)
            {
                var value = claim.Value.ToUpper();
                if (value.Contains("GUEST"))
                {
                    return true;
                }
            }
            return false;
        }));

    

    // custom startup policy check
    options.AddPolicy("StartUpPolicy", policy =>
        policy.Requirements.Add(new StartUpAuthorizationRequirement()));

    // OLD: Policy that requires either 'Admin' or 'Guest' role using old 'Role' claim
    // options.AddPolicy("AdminOrGuestPolicy", policy =>
    //     policy.RequireAssertion(context =>
    //         context.User.HasClaim(c => c.Type == "Role" && (c.Value == "Admin" || c.Value == "Guest"))));
    // NEW: Policy using standard role claim for Admin or Guest
    options.AddPolicy("AdminOrGuestPolicy", policy =>
        policy.RequireAssertion(context =>
        {
            var rolesClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "roles").ToList();
            if (!rolesClaims.Any()) return false;
            
            foreach (var claim in rolesClaims)
            {
                var value = claim.Value.ToUpper();
                if (value.Contains("ADMIN") || value.Contains("GUEST"))
                {
                    return true;
                }
            }
            return false;
        }));

    // OLD: MultiRolePolicy using old 'Role' claim with comma separation
    // options.AddPolicy("MultiRolePolicy", policy =>
    // policy.RequireAssertion(context =>
    //     context.User.Claims
    //         .Where(c => c.Type == "Role")
    //         .SelectMany(c => c.Value.Split(',')) // Split roles if stored as "Admin,StartUp"
    //         .Any(role => role == "Admin" || role == "Guest")));
    // NEW: MultiRolePolicy using standard role claim
    options.AddPolicy("MultiRolePolicy", policy =>
        policy.RequireAssertion(context =>
        {
            var rolesClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "roles").ToList();
            if (!rolesClaims.Any()) return false;
            
            foreach (var claim in rolesClaims)
            {
                var value = claim.Value.ToUpper();
                if (value.Contains("ADMIN") || value.Contains("GUEST"))
                {
                    return true;
                }
            }
            return false;
        }));

    // OLD: Policy that allows any of the roles specified using old 'Role' claim
    // options.AddPolicy("AllPolicy", policy =>
    //     policy.RequireAssertion(context =>
    //         context.User.HasClaim(c => c.Type == "Role" &&
    //             (c.Value == "admin" || c.Value == "Guest" || c.Value == "Incubator" || c.Value == "StartUp" || c.Value == "Accelerators"))));
    // NEW: Policy using standard role claim for all roles
    options.AddPolicy("AllPolicy", policy =>
    policy.RequireAssertion(context =>
    {
        var rolesClaims = context.User.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "roles")
            .ToList();

        if (!rolesClaims.Any()) return false;

        foreach (var claim in rolesClaims)
        {
            var value = claim.Value.ToUpper();

            if (value.Contains("ADMIN") || value.Contains("GUEST") ||
                value.Contains("INCUBATOR") || value.Contains("STARTUP") ||
                value.Contains("ACCELERATOR"))
            {
                return true;
            }
        }

        return false;
    }));


});


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins(
                "http://3.110.123.114:81", // React App 1
                "http://3.110.123.114:82"  // React App 2
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
            //.AllowCredentials(); // Uncomment if you use cookies or authentication headers
        });
});


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new
        {
            Message = "Validation failed",
            Errors = errors
        });
    };
});



builder.Services.AddCustomServices();

builder.Services.AddCustomeExceptionHandler();

// Add problem details support
builder.Services.AddProblemDetails(options =>
{
    // Customize ProblemDetails factory
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = context.HttpContext.Request.Path;
        // Add correlation ID if available
        if (context.HttpContext.TraceIdentifier != null)
        {
            context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        }
    };
});

var app = builder.Build();

app.UseRouting();



// 2️⃣ Detect IP correctly if behind a Load Balancer (Optional)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// 3️⃣ Middleware to log IP for debugging
app.Use(async (context, next) =>
{
    string? ipAddress = context.Connection.RemoteIpAddress?.ToString();

    // ✅ Always prioritize "X-Forwarded-For" if it's set
    if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedIp) && !string.IsNullOrEmpty(forwardedIp))
    {
        ipAddress = forwardedIp.FirstOrDefault();
    }

    Console.WriteLine($"Request from IP: {ipAddress}");
    await next();
});

// Use the custom rate limiting middleware
//app.UseMiddleware<CustomRateLimitMiddleware>();
app.UseRateLimiter();

// Configure the HTTP request pipeline.



/*app.Use(async (context, next) =>
{
    if (context.Response.StatusCode == 429) // Too Many Requests
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "You have exceeded the rate limit. Please wait before sending more requests.",
            status = 429,
            retryAfter = context.Response.Headers["Retry-After"].ToString() // Fetch retry time from headers
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
        return; // Stop further request processing
    }

    await next(); // Continue if not rate-limited
});*/


// Configure the HTTP request pipeline.
// Configure static file serving
app.ConfigureStaticFiles();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();

// Log claims after authentication
app.Use(async (context, next) =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        Console.WriteLine("=== JWT CLAIMS ===");
        foreach (var claim in context.User.Claims)
        {
            Console.WriteLine($"Type: '{claim.Type}', Value: '{claim.Value}'");
        }
        Console.WriteLine("=================");
    }
    await next();
});

app.UseAuthorization();

app.UseExceptionHandler();

// remove this temporary because we do not have SSl right now.

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {


        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            var endpointName = $"MSINS_API {description.GroupName}";
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", endpointName);

            // Highlight deprecated APIs
            if (description.IsDeprecated)
            {
                options.DocumentTitle += $" (Deprecated {description.GroupName})";
            }
        }

    });


    // Use development (default) settings
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

    //app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/error");
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // Use production settings
    builder.Configuration.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);

    app.UseExceptionHandler("/error");
}







// ✅ Load appropriate config files based on environment
if (app.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
}

// ✅ Keep exception handling based on environment
app.UseExceptionHandler("/error");


app.MapControllers();



// IMPORTANT: Bind to port 80 for Docker container
//app.Urls.Add("http://*:80");

app.Run();
