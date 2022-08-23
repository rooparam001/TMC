using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TMC.AppRepository
{
    public class EmailManager
    {
       



        public void SendEmail(string Subject, string Body, string To)
        {
            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            string UserId = configuration.GetSection("EmailSettings:FromEmail").Value;
            string Password = configuration.GetSection("EmailSettings:Password").Value;
            mail.To.Add(To);
            mail.From = new MailAddress(UserId);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = configuration.GetSection("EmailSettings:Smtp").Value;
            smtp.Port = Convert.ToInt16(configuration.GetSection("EmailSettings:Port").Value);
            smtp.Credentials = new NetworkCredential(UserId, Password);
          //  smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        
    }
}
