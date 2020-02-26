using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class SMTPMailClient
    {
        public static bool SendRawEmail(string email, string msg, string title)
        {
            try
            {
                #region body
                string body = @"<div style=""width:500px;margin:auto;background-color:#fbfbfb"">
        <div style=""width:95%;margin:auto;height:60px;background-color:#66B85C"">
            <div style=""font-family:verdana, calibri;font-size:95%;color:white;font-weight:bold;"">
                <p style=""padding:20px;text-align:center"">" + title + @"</p>
            </div>
        </div>
        <br />
        <div style=""width:95%;margin:auto"">
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px;font-weight:bold "">
                Dear " + email + @",


            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;text-align:justify"">
           "+msg+ @"
</div>
               
            <br />
               
            </div>
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;text-align:justify"">
                Thanks for choosing Nano Pips.

            </div>

         
         
            <br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff"">
               Best regards,<br />
                Nano Pips Team
            </div>
            <br />
            <br />
            <br />
            
        </div>


        <div style=""width:95%;margin:auto;height:50px;background-color:#66B85C"">
            <div style=""font-family:verdana, calibri; font-size:12.0px; color:rgb(0,0,1); text-align:center; background:rgb(255,255,255); text-transform:uppercase"">
                COPYRIGHT © " + DateTime.Now.Year + @" ALL RIGHTS RESERVED BY <br /> Nano Pips
            </div>
        </div>
    </div>";
                #endregion

               SendEmail(body ,title, new string[] { email });

            }
            catch (Exception ex)
            {
                ExchangeException.Throw(ErrorCode.OTHER_ERROR, ex);
            }
            return true;
        }

        private static string Send(string address, NameValueCollection values)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    byte[] apiResponse = client.UploadValues(address, values);
                    return Encoding.UTF8.GetString(apiResponse);

                }
                catch (Exception ex)
                {
                    return "Exception caught: " + ex.Message + "\n" + ex.StackTrace;
                }
            }
        }
        public static bool SendEmail(string body, string subject, string[] to)
        {
            ///////////////SMTP CONFIGURATIONS
            //SmtpClient smtpClient = new SmtpClient("smtp.elasticemail.com", 2525);
            //NetworkCredential basicCredential =
            //    new NetworkCredential("nm@investaco.in", "ac60f054-5ba8-44ef-9e7c-af052fbbd256");
            //MailMessage message = new MailMessage();
            //MailAddress fromAddress = new MailAddress("zzz@investaco.in", "Nano Pips");
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.EnableSsl = true;
            //smtpClient.Credentials = basicCredential;
            //message.From = fromAddress;
            //message.Subject = subject;
            ////Set IsBodyHtml to true means you can send HTML email.
            //message.IsBodyHtml = true;
            //message.Body = body;

            //foreach (var item in to)
            //{
            //    message.To.Add(item);
            //}


            //try
            //{
            //    smtpClient.Send(message);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //    //Error, could not send the message
            //    // Response.Write(ex.Message);
            //}
            //////////////////////////////////////////
            /////// ELASTIC API CONFIGURATIONS
            ///
            try
            {
                NameValueCollection values = new NameValueCollection();
                values.Add("apikey", "D509C22AF02010E1E89C14EA4A983881E2690A5F43E805614511736FD8A76D81E7E09A5C41DDF3B056DF73279FDC9E20");
                values.Add("from", "clientservice@nanopips.com");
                //values.Add("apikey", "ac60f054-5ba8-44ef-9e7c-af052fbbd256");
                //values.Add("from", "zzz@investaco.in");
                values.Add("fromName", "Nano Pips");
                values.Add("to", to[0]);
                if (to.Length >= 2)
                {
                    values.Add("to", to[1]);
                }
                values.Add("subject", subject);
                values.Add("bodyHtml", body);
                values.Add("isTransactional", "true");
                string address = "https://api.elasticemail.com/v2/email/send";
                string response = Send(address, values);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
