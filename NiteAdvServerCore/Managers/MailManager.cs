using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;


namespace NiteAdvServerCore.Managers
{
   

        public static class MailManager
        {

            //public static async Task<bool> SpedisciMail( String To, String Subject, String Body)
            //{

            //    try
            //    {
            //        string From = "support@mobyalps.com";
            //        //var body = "<p>Email Da: {0} ({1})</p><p>Messaggio:</p><p>{2}</p>";
            //        var message = new MailMessage();
            //        message.To.Add(new MailAddress(To));  // replace with valid value 
            //        message.From = new MailAddress(From);  // replace with valid value
            //        message.Subject = Subject;
            //        message.Body = Body; //string.Format(body, model.FromName, model.FromEmail, model.Message);
            //        message.IsBodyHtml = true;

            //        using (var smtp = new SmtpClient()) //"localhost"
            //        {
            //            var credential = new NetworkCredential
            //            {
            //                UserName = "support@mobyalps.com",  // replace with valid value
            //                Password = "MotorolDic75"  // replace with valid value
            //            };
            //            smtp.Credentials = credential;
            //            smtp.Host = "authsmtp.securemail.pro";
            //            smtp.Port = 465;
            //            //smtp.Port = 25;
            //            smtp.EnableSsl = true;
            //            await smtp.SendMailAsync(message);
            //        }
            //        ////RELEASE
            //        //using (var smtp = new SmtpClient("localhost"))
            //        //{

            //        //    await smtp.SendMailAsync(message);
            //        //}
            //        return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        return false;
            //    }

            //}
            public static bool SpedisciMailCliente(String To, String Subject, String Body)
            {

                try
                {
                    PerformSendMail("support@mobyalps.com", To, Subject, Body);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
            public static bool SpedisciMailAmministrazione(String From, String Subject, String Body)
            {

                try
                {
                    PerformSendMail(From, "support@mobyalps.com", Subject, Body);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
            private static void PerformSendMail(String From, String To, String Subject, String Body)
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(From));
                message.To.Add(MailboxAddress.Parse(To));
                message.Subject = Subject;

                message.Body = new TextPart("plain")
                {
                    Text = Body
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("authsmtp.securemail.pro", 465, true);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("smtp@mobyalps.com", "Motorol@Dic75");

                    client.Send(message);
                    client.Disconnect(true);
                }
            }

        }


    
}
