using System;
using SendGrid.Helpers.Mail;

namespace Domain
{
    public interface IEmail
    {
        void SendEmail();
        void SendEmail(Exception exception);
    }
}
