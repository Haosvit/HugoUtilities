using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WordScrambler.Helpers;
using WordScrambler.Models;

namespace WordScrambler.Controllers
{
    public class EmailHelperController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Send(MailModel mailModel, HttpPostedFileBase attachment)
        {
            var sentAmong = 0;
            List<string> wrongEmailList = new List<string>();
            if (attachment != null)
            {
                using (var resultPackage = new ExcelPackage(attachment.InputStream))
                {
                    ExcelWorksheet wsheet = resultPackage.Workbook.Worksheets[1];
                  
                    for (int r = 2; wsheet.Cells[r, 2].Value != null; r++)
                    {
                        var content = new StringBuilder(mailModel.Content);
                        string fullName = "";
                        string dateTime = "";
                        string toAddress = "";

                        for (int c = 1; wsheet.Cells[r, c].Value != null; c++)
                        {
                            var value = wsheet.Cells[r, c].Value;
                          
                            switch (c)
                            {
                                case (int) ResultSheetColumn.FullName:
                                    fullName = value.ToString();
                                    break;
                                case (int)ResultSheetColumn.Email:
                                    toAddress = value.ToString();
                                    break;
                                case (int)ResultSheetColumn.Dow:
                                    dateTime = value.ToString();
                                    break;
                                case (int)ResultSheetColumn.Date:
                                    if (value != null)
                                    {
                                        DateTime date;
                                        if (DateTime.TryParse(value.ToString(), out date))
                                        {
                                            dateTime += " (" + date.ToString("dd/MM/yyyy") + ")";
                                        }
                                    }

                                    break;
                                case (int)ResultSheetColumn.Time:
                                    dateTime += ", " + value.ToString();
                                    break;
                            }
                        }

                        content.Replace(Constants.NamePlaceholder, fullName);
                        content.Replace(Constants.TimePlaceholder, dateTime);
                        var sendResult = SendEmail(mailModel.FromAddress, mailModel.Password, toAddress, mailModel.Subject, content.ToString());
                        if (sendResult)
                        {
                            sentAmong++;
                        }
                        else
                        {
                            wrongEmailList.Add(toAddress);
                        }
                    }
                }
            }

            ViewBag.SentAmong = sentAmong;
            ViewBag.ErrorEmails = wrongEmailList;
            return View("Result");
        }

        private bool SendEmail(string from, string password, string to, string subject, string content)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, password)
            };
            smtp.EnableSsl = true;

            using (var message = new MailMessage(new MailAddress(from, Constants.FromAddressDisplayName), new MailAddress(to))
            {
                Subject = subject,
                Body = content,
                IsBodyHtml = true,
            })
            {
                try
                {
                    smtp.Send(message);
                    return true;
                }
                catch (SmtpFailedRecipientException ex)
                {
                    return false;
                }
            }
        }
    }
}