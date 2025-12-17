# Email Service Implementation

The Email Service has been implemented with the following features:

1. **Database-Driven Configuration**
   - Email credentials and SMTP settings are stored in the database
   - No hardcoded values or configuration file dependencies
   - Settings can be updated without code redeployment
   - Supports dynamic provider changes (Gmail, Office 365, etc.)

2. **Provider-Agnostic Design**
   - Generic email service implementation
   - Configurable SMTP settings to support different email providers
   - Easy to switch between email providers by updating database settings

3. **Required Database Fields**
   In the `settings` table:
   - `emailAddress`: SMTP username/email address
   - `passwords`: SMTP password or app-specific password
   - `smtpServer`: SMTP server address (e.g., "smtp.gmail.com")
   - `smtpPort`: SMTP port number (e.g., 587)
   - `enableSsl`: SSL/TLS flag (true/false)

## Features
- Supports both single and multiple recipient emails
- Proper error handling and exception propagation
- Lazy loading of SMTP settings
- Default values for common SMTP configurations

## Usage Example
```csharp
private readonly IEmailService _emailService;

public YourClass(IEmailService emailService)
{
    _emailService = emailService;
}

// Send single email
await _emailService.SendEmailAsync(
    "recipient@example.com",
    "Subject",
    "HTML Content",
    "Sender Name"
);

// Send to multiple recipients
await _emailService.SendEmailAsyncMultiple(
    new[] { "recipient1@example.com", "recipient2@example.com" },
    "Subject",
    "HTML Content",
    "Sender Name"
);
```

## Service Registration
Register the service in your `Program.cs`:
```csharp
builder.Services.AddScoped<IEmailService, EmailService>();
```

## Notes
1. The service automatically loads SMTP settings from the database on first use
2. For Gmail, use an App Password instead of regular account password
3. Settings can be updated in the database without service restart
4. The design supports future migration to different email providers without code changes