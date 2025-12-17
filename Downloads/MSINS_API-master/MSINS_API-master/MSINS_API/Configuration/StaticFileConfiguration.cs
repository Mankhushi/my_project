using Microsoft.Extensions.FileProviders;

namespace MSINS_API.Configuration
{
    public static class StaticFileConfiguration
    {
        public static void ConfigureStaticFiles(this WebApplication app)
        {
            var staticPaths = new Dictionary<string, string>
        {
            { "consultation", "consultation" },
            { "banners", "uploads/banners" },
            { "ecosystem", "uploads/ecosystem" },
            { "partners", "uploads/partners" },
            { "about", "uploads/about" },
            { "leaders", "uploads/leaders" },
            { "initiatives", "uploads/initiatives" },
            { "executives", "uploads/executives" },
            { "featuredResource", "uploads/featuredResource" },
            { "GovernmentResolution", "uploads/GovernmentResolution" },
            { "events", "uploads/events" },
            { "media", "uploads/media" }
        };

            foreach (var path in staticPaths)
            {
                var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), path.Value);

                // Ensure directory exists
                if (!Directory.Exists(physicalPath))
                {
                    try
                    {
                        Directory.CreateDirectory(physicalPath);
                    }
                    catch (Exception ex)
                    {
                        // Log the error if you have a logging system
                        Console.WriteLine($"Error creating directory {physicalPath}: {ex.Message}");
                    }
                }

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(physicalPath),
                    RequestPath = $"/{path.Value}"
                });
            }
        }
    }
}
