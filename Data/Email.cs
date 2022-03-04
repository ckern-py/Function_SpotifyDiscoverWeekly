using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Data
{
    public class Email : IEmail
    {
        private ILogger _logger;

        public Email(ILogger log)
        {
            _logger = log;
        }

        public void SendEmail()
        {
            ActuallySendEmail($"SpotifyDiscWeekly_Function has completed succesfully", "SUCCESS: SpotifyDiscWeekly");
        }

        public void SendEmail(Exception exception)
        {
            ActuallySendEmail($"SpotifyDiscWeekly_Function ran into a problem.\n\nMessage: {exception.Message}\n\nStackTrace: {exception.StackTrace}", "ERROR: SpotifyDiscWeekly");
        }

        private void ActuallySendEmail (string emailMessageContent, string emailSubject)
        {
            EmailLog("SendMailEx", "Start");

            string apiKey = Environment.GetEnvironmentVariable("SendgridAPIKey");
            SendGridClient client = new SendGridClient(apiKey);

            string functionEmail = Environment.GetEnvironmentVariable("FunctionEmailAddress");
            SendGridMessage emailMessage = new SendGridMessage()
            {
                From = new EmailAddress(functionEmail, "SpotifyDiscWeekly_Function"),
                Subject = emailSubject,
                PlainTextContent = emailMessageContent
            };

            string receivingEmail = Environment.GetEnvironmentVariable("MyEmailAddress");
            emailMessage.AddTo(new EmailAddress(receivingEmail));

            EmailLog("SendMailEx", "Send");
            Response _ = client.SendEmailAsync(emailMessage).Result;

            EmailLog("SendMailEx", "Return");
        }

        private void EmailLog(string method, string logString)
        {
            _logger.LogInformation($"{method} - {logString} - {DateTime.Now}");
        }
    }
}
