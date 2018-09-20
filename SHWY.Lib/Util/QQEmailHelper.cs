using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.Util
{
   public static class QQEmailHelper
    {
        private const string SendAddress = "865729986@qq.com";
        private const string SendPassword = "ootvqrwflvvlbdea";

        private static SmtpClient Client = null;
        static QQEmailHelper()
        {
            Client = new SmtpClient()
            {
                UseDefaultCredentials = true,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(SendAddress, SendPassword),
                Host = "smtp.qq.com"
            };
        }
        public static async Task<bool> SendMailAsync(string topic, string receiveAddress, string body)
        {
            string mailTopic = topic;//主题
            string mailBody = body;//内容
            MailMessage mmsg = new MailMessage(new MailAddress(SendAddress),new MailAddress(receiveAddress));
            mmsg.Subject = mailTopic;//邮件主题
            mmsg.SubjectEncoding = Encoding.UTF8;//主题编码
            mmsg.Body = mailBody;//邮件正文
            mmsg.BodyEncoding = Encoding.UTF8;//正文编码
            mmsg.IsBodyHtml = true;
            mmsg.Priority = MailPriority.High;
            try
            {
                await Client.SendMailAsync(mmsg);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
