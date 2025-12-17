using System.Net;
using System.Net.Mail;
using System.Data;
using Microsoft.Data.SqlClient;
using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly string _connectionString;
        private SmtpSettings _smtpSettings;

        private class SmtpSettings
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public bool EnableSsl { get; set; }
        }

        public EmailService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private async Task LoadSmtpSettings()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("GetTableData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TableName", "settings");
                    cmd.Parameters.AddWithValue("@Columns", "emailAddress,passwords,smtpServer,smtpPort,enableSsl");
                    cmd.Parameters.AddWithValue("@OrderColumn", "settingId");
                    cmd.Parameters.AddWithValue("@OrderDirection", "asc");
                    cmd.Parameters.AddWithValue("@ActiveStatus", 1);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            _smtpSettings = new SmtpSettings
                            {
                                Username = reader["emailAddress"] as string ?? string.Empty,
                                Password = reader["passwords"] as string ?? string.Empty,
                                Server = reader["smtpServer"] as string ?? "smtp.gmail.com", // Default value
                                Port = Convert.ToInt32(reader["smtpPort"] ?? 587), // Default value
                                EnableSsl = Convert.ToBoolean(reader["enableSsl"] ?? true) // Default value
                            };
                        }
                        else
                        {
                            //throw new Exception("SMTP settings not found in database");
                        }
                    }
                }
            }
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent, string fromName)
        {
            try
            {
                if (_smtpSettings == null)
                {
                    await LoadSmtpSettings();
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.Username, fromName),
                    Subject = subject,
                    Body = htmlContent,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                using var smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.EnableSsl
                };

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                // throw new Exception($"SMTP error occurred while sending email: {smtpEx.Message}", smtpEx);
            }
            catch (Exception ex)
            {
                //throw new Exception($"Error occurred while sending email: {ex.Message}", ex);
            }
        }

        public async Task SendEmailAsyncMultiple(IEnumerable<string> toEmails, string subject, string htmlContent, string fromName)
        {
            foreach (var email in toEmails)
            {
                await SendEmailAsync(email, subject, htmlContent, fromName);
            }
        }
    }
}