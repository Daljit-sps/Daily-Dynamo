using DailyDynamo.Services.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using static DailyDynamo.Shared.Utilities.AppSettingsUtility;

namespace DailyDynamo.Services.Services
{
    public class SharedService : ISharedService
    {
        public async Task<(bool, string)> EmailSendingAsync(string subject, string body, List<string> to, List<string>? cc = null, List<string>? bcc = null)
        {
            if(string.IsNullOrWhiteSpace(subject))
                return (false, "Subject cannot be null or empty.");

            if(body == null)
                return (false, "Body cannot be null or empty.");

            if(to == null || to.Count == 0)
                return (false, "At least one 'To' recipient must be specified.");

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(Settings.EmailSettings.From));

            foreach(var toAddress in to)
            {
                if(!IsValidEmailAddress(toAddress))
                    return (false, $"Invalid email address: {toAddress}");
                email.To.Add(MailboxAddress.Parse(toAddress));
            }

            // Add CC and BCC recipients if provided
            if(cc != null)
            {
                foreach(var ccAddress in cc)
                {
                    if(!IsValidEmailAddress(ccAddress))
                        return (false, $"Invalid email address: {ccAddress}");
                    email.Cc.Add(MailboxAddress.Parse(ccAddress));
                }
            }

            if(bcc != null)
            {
                foreach(var bccAddress in bcc)
                {
                    if(!IsValidEmailAddress(bccAddress))
                        return (false, $"Invalid email address: {bccAddress}");
                    email.Bcc.Add(MailboxAddress.Parse(bccAddress));
                }
            }

            email.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            email.Body = bodyBuilder.ToMessageBody();

            // send email
            using(var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.CheckCertificateRevocation = false;
                    client.Connect(Settings.EmailSettings.Host, Settings.EmailSettings.Port, SecureSocketOptions.Auto);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(Settings.EmailSettings.From, Settings.EmailSettings.Password);
                    await client.SendAsync((MimeKit.MimeMessage)email);
                }
                catch(Exception ex)
                {
                    return (false, ex.Message);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
            return (true, "email sent successfully!");
        }

        private bool IsValidEmailAddress(string email)
        {
            try
            {
                string emailRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if(Regex.IsMatch(email, emailRegexPattern))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }


    }


}
