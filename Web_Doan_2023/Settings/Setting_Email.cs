using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

namespace Web_Doan_2023.Settings
{
    public static class setting_Email
    {
        public static  bool SendMail(string Subject, string body,  string mailto)
        {
           
            string _from = "ptranninh@gmail.com";
            string _subject = Subject;//Tiêu đề
            string _body = body;
            string _gmail = "ptranninh@gmail.com";
            string _password = "tranninhphuc@1061";
            MailMessage message = new MailMessage(_from, mailto, _subject, _body);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);
            using var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_gmail, _password);

            try
            {
               
                 smtpClient.SendMailAsync(message);
                //Console.WriteLine(smtpClient.SendMailAsync(message));
                return true;
            
            }
            catch (Exception e)
            {
                return false;
              
            }

        }
       
    }
}
   