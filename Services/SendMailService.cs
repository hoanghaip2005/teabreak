using System;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

public class MailSettings
{
    public string Mail { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }

}

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
    Task SendSmsAsync(string number, string message);
}

public class SendMailService : IEmailSender {


    private readonly MailSettings mailSettings;

    private readonly ILogger<SendMailService> logger;


    // mailSetting được Inject qua dịch vụ hệ thống
    // Có inject Logger để xuất log
    public SendMailService (IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger) {
        mailSettings = _mailSettings.Value;
        logger = _logger;
        logger.LogInformation("Create SendMailService");
    }

   
    public async Task SendEmailAsync(string email, string subject, string htmlMessage) {
        try {
            // Validate email address
            if (string.IsNullOrEmpty(email) || !App.Utilities.AppUtilities.IsValidEmail(email))
            {
                logger.LogError($"Invalid email address: {email}");
                throw new ArgumentException($"Invalid email address: {email}");
            }

            var message = new MimeMessage();
            message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            
            // Log email details
            logger.LogInformation($"Preparing to send email to: {email}");
            logger.LogInformation($"From: {mailSettings.Mail} ({mailSettings.DisplayName})");
            logger.LogInformation($"Subject: {subject}");

            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            message.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try {
                logger.LogInformation($"Connecting to SMTP server: {mailSettings.Host}:{mailSettings.Port}");
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                logger.LogInformation("SMTP connection successful");

                logger.LogInformation($"Authenticating with email: {mailSettings.Mail}");
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                logger.LogInformation("SMTP authentication successful");

                logger.LogInformation($"Sending email to {email}");
                await smtp.SendAsync(message);
                logger.LogInformation($"Email sent successfully to {email}");
            }
            catch (Exception ex) {
                logger.LogError($"SMTP Error: {ex.Message}");
                logger.LogError($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    logger.LogError($"Inner Exception: {ex.InnerException.Message}");
                    logger.LogError($"Inner Stack Trace: {ex.InnerException.StackTrace}");
                }
                
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    System.IO.Directory.CreateDirectory("mailssave");
                    var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                    await message.WriteToAsync(emailsavefile);
                    logger.LogInformation("Development: Email saved to " + emailsavefile);
                }
                
                throw new InvalidOperationException($"Failed to send email to {email}", ex);
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in SendEmailAsync: {ex.Message}");
            throw;
        }
    }

        public Task SendSmsAsync(string number, string message)
        {
            // Cài đặt dịch vụ gửi SMS tại đây
            System.IO.Directory.CreateDirectory("smssave");
            var emailsavefile = string.Format(@"smssave/{0}-{1}.txt",number, Guid.NewGuid());
            System.IO.File.WriteAllTextAsync(emailsavefile, message);
            return Task.FromResult(0);
        }
}