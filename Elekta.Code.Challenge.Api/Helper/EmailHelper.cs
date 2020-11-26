using Elekta.Code.Challenge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Helper
{
    public static class EmailHelper
    {
        public static bool SendEmail(string subject, string to, string htmlBody, EmailSetting emailSetting)
        {
            // Setting up email, but only send it in Production environment
            if (emailSetting.Environment == "Development") { return true; }

            bool sendEmailStatus = true;

            MailMessage oMail;
            SmtpClient smpt;
            int PortNo = emailSetting.SMTP_Port;
            string smptHost = emailSetting.SMTP_Server;

            string sSenderEmail = "";
            string sSenderPwd = "";

            try
            {

                sSenderEmail = emailSetting.NoReply_Email;
                sSenderPwd = emailSetting.NoReply_Email_Pwd;

                oMail = new MailMessage();
                oMail.From = new MailAddress(sSenderEmail);
                //oMail.Bcc.Add(new MailAddress(bcc.ToLower()));
                oMail.To.Add(new MailAddress(to.ToLower()));

                oMail.Subject = subject;
                oMail.IsBodyHtml = true;
                oMail.Body = htmlBody;

                //connect to host and port
                smpt = new SmtpClient();
                smpt.Host = smptHost;
                System.Net.NetworkCredential oCredential = new System.Net.NetworkCredential(sSenderEmail, sSenderPwd);
                smpt.Credentials = oCredential;
                smpt.Port = PortNo;
                smpt.Send(oMail);
            }
            catch (SmtpException ex)
            {
                sendEmailStatus = false;
                throw new Exception("Failed to send email.");
            }
            finally
            {
                oMail = null;
                smpt = null;
                GC.Collect();
            }
            return sendEmailStatus;
        }
    }
}
