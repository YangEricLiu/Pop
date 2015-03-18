/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: MailHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Helper for mail
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class MailHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tos"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="host"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool Send(string[] tos, string from, string subject, string body, string host, string username, string password, string[] files, bool isHtml = true)
        {
            try
            {
                var mimeType = new ContentType(isHtml ? "text/html" : "text/plain");
                var alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                var msg = new MailMessage { Subject = subject, Body = body, From = new MailAddress(@from), IsBodyHtml = true };

                foreach (var to in tos) msg.To.Add(to);

                foreach (var file in files)
                {
                    var data = new Attachment(file, MediaTypeNames.Application.Octet);

                    var disposition = data.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(file);
                    disposition.ModificationDate = File.GetLastWriteTime(file);
                    disposition.ReadDate = File.GetLastAccessTime(file);

                    msg.AlternateViews.Add(alternate);
                    msg.Attachments.Add(data);
                }

                var sendMail = new SmtpClient
                {
                    Host = host,
                    Credentials = new NetworkCredential(username, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                };
                sendMail.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
                return false;
            }
        }

        public static bool Send(string[] tos, string from, string subject, string body, string host, string username, string password, Dictionary<string, byte[]> files)
        {
            try
            {
               //var mimeType = new ContentType("text/html");
                var alternate = AlternateView.CreateAlternateViewFromString(body, Encoding.GetEncoding("GB2312"), "text/html"); 
                var msg = new MailMessage { Subject = subject, Body = body, From = new MailAddress(@from), IsBodyHtml = true };

                foreach (var to in tos) msg.To.Add(to);

                foreach (var file in files)
                {
                    msg.Attachments.Add(new Attachment(new MemoryStream(file.Value), file.Key, MediaTypeNames.Application.Octet));
                }
                msg.AlternateViews.Add(alternate);
                var sendMail = new SmtpClient
                {
                    Host = host,
                    Credentials = new NetworkCredential(username, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                };
                sendMail.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);

                return false;
            }
        }
    }
}
