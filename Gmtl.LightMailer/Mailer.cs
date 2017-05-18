using System;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;

namespace Gmtl.LightMailer
{
    public interface IMailer
    {
        /// <summary>
        /// Will try to send message to itself
        /// </summary>
        /// <returns></returns>
        bool TestConfiguration();

        bool SendMail(string subject, string body, string recipient);

        void DisableCertificateCheck();
    }

    public class Mailer : IMailer
    {
        private static readonly object locker = new object();
        private static Mailer _instance;

        private string from;

        public static Mailer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new Mailer();

                            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                            _instance.from = smtpSection.Network.UserName;
                        }
                    }
                }

                return _instance;
            }
        }

        public void DisableCertificateCheck()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public bool TestConfiguration()
        {
            try
            {
                var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                string username = smtpSection.Network.UserName;

                return SendMail("Mailer SelfTest", "Mailer SelfTest", username);
            }
            catch
            {
                return false;
            }
        }

        public bool SendMail(string subject, string body, string recipient)
        {
            try
            {
                using (var message = new MailMessage(from, recipient))
                {
                    message.Subject = subject;
                    message.Body = body;

                    using (var smtp = new SmtpClient())
                    {
                        //var credential = new NetworkCredential
                        //{
                        //    UserName = settings.Username,
                        //    Password = settings.Password
                        //};

                        //smtp.Credentials = credential;
                        //smtp.Host = settings.Host;
                        //smtp.Port = settings.Port;
                        //smtp.EnableSsl = settings.UseSSL;

                        smtp.Send(message);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}