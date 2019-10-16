using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace ClassLibrary
{
    //TODO use gmail api
    public static class EmailSendingClass
    {
        public static string S { get; set; }
        public static string EmailSubject { get;  } = "annual report";
        public static string EmailBody { get; set; }
        public static string ShouldQuartelYearReportSendFilePath { get;  } = @"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Email\shouldquartelyearreportsend.dat";
        public static string ShouldHalfYearReportSendFilePath { get;  } = @"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Email\shouldhalfyearreportsend.dat";
        public static bool ShouldSendEmail { get; set; }


        public static void SendReportInEmail()
        {
            try
            {
                S = WriteAndReadClass.ReadFromFile(@"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Email\0.dat");
            }
            catch (Exception) { }

            var fromAddress = new MailAddress("attila85toth@gmail.com", "iAboutMoneyApp");
            var toAddress = new MailAddress("tothattila@rocketmail.com", "Attila Tóth");
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, S),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = EmailSubject,
                Body = EmailBody
            })
            {
                smtp.Send(message);
            }
        }

        
    }
}
