namespace TimeSheet.Controllers
{
    #region namespace

    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;
    using Repository;

    #endregion namespace

    /// <summary>
    /// Hold messeging functions
    /// </summary>
    public class MessageController
    {
        public DashboardRepository dashboardRepository;
        public int isError = 0;
        public void SendEmailInBackgroundThread(MailMessage mailMessage)
        {
            Thread bgThread = new Thread(new ParameterizedThreadStart(SendMail));
            bgThread.IsBackground = true;
            bgThread.Start(mailMessage);
        }

        /// <summary>
        /// Method to send mail
        /// </summary>
        /// <returns></returns>
        public async void SendMail(Object mailMsg)
        {
            MailMessage msg = (MailMessage)mailMsg;

            //msg.From = new MailAddress("hr.dtinnovativegroup@gmail.com");
            //SmtpClient client = new SmtpClient();
            //client.Host = "smtp.gmail.com";
            //client.Port = 587;
            //client.EnableSsl = true;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = true;
            //client.Credentials = new NetworkCredential("hr.dtinnovativegroup@gmail.com", "hr@1234567");
            //client.Timeout = 20000;

            var fromAddress = new MailAddress("hr.dtinnovativegroup@gmail.com");
            var toAddress = new MailAddress(msg.To.First().Address);
            const string fromPassword = "hr@1234567";

            try
            {
                //await Task.Run(() => client.SendMailAsync(msg));
                //Console.Write("Mail has been successfully sent!");
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = msg.Subject,
                    Body = msg.Body,
                    IsBodyHtml = true,
                })
                {
                    foreach (var ccAddress in msg.CC)
                    {
                        message.CC.Add(ccAddress);
                    }

                    smtp.Send(message);
                }

            }
            catch (Exception ex)
            {
                Console.Write("Fail Has error" + ex.Message);
            }
            finally
            {
                msg.Dispose();
            }
        }

        ///private Timer timer;
        public void SetUpTimer()
        {
            var DailyTime = ConfigurationManager.AppSettings["MailTime"];
            var timeParts = DailyTime.Split(new char[1] { ':' });

            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                       int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
            TimeSpan ts;
            if (date > dateNow)
                ts = date - dateNow;
            else
            {
                date = date.AddDays(1);
                ts = date - dateNow;
            }

            //waits certan time and run the code
            Task.Delay(ts).ContinueWith((x) => FindDefaulter());
            isError = 0;
        }

        public async Task FindDefaulter()
        {
            try
            {
                /****Check Sunday*Start*****/

                var isExecute = false;
                var today = DateTime.Now.DayOfWeek;
                if (today == DayOfWeek.Sunday)
                {
                    isExecute = false;
                }
                else
                {
                    isExecute = true;
                }

                /****Check Sunday*End*****/

                if (isExecute)
                {
                    var previousDay = DateTime.Now.AddDays(-1);

                    if (today == DayOfWeek.Monday)
                    {
                        previousDay.AddDays(-1);
                    }

                    var previousDayString = previousDay.Date.ToString("dd-MM-yyyy");

                    this.dashboardRepository = new DashboardRepository();


                    //var employeeList = await this.dashboardRepository.GetEmployeeList();

                    LoginApi api = new LoginApi();
                    var employeeList = await api.EmployeeListApi();

                    var defaulterMailBody = "Hi <nameValue>,<br/><br/> Please fill your TimeSheet.<br/><br/><table border='1'><thead><tr>" +
                            "<th>Date</th><th>Project</th><th>Hours</th><th>Remark</th></tr></thead>" +
                            "<tbody><tr><td><DateValue></td><td>-</td><td>0</td><td>" +
                            "-</td></tr></tbody></table><br/><br/>http://staff.bisplindia.in";

                    if (employeeList != null)
                    {
                        foreach (var emp in employeeList)
                        {
                            var list = await this.dashboardRepository.GetDefaulterWorkDetail(emp.id, previousDayString);

                            if (list == null || list.Count == 0)
                            {
                                /**Code to find email address of reporting person**/

                                var mailBody = defaulterMailBody.Replace("<nameValue>", emp.first_name + " " + emp.last_name).Replace("<DateValue>", previousDayString + " (" + previousDay.DayOfWeek.ToString() + ") ");
                                MailMessage msg = new MailMessage();

                                //var reportingEmailId = from p in employeeList where p.EmployeeId == emp.EmployeeId select p.EmailId;
                                //if (reportingEmailId != null && reportingEmailId.Count() > 0)
                                //{
                                //    var copy = new MailAddress(reportingEmailId.First());
                                //    msg.CC.Add(copy);
                                //}

                                /****Add Default email Address*****/

                                var defaultEmailAddress = new MailAddress(ConfigurationManager.AppSettings["default1"]);
                                msg.CC.Add(defaultEmailAddress);

                                defaultEmailAddress = new MailAddress(ConfigurationManager.AppSettings["reportTo"]);
                                msg.CC.Add(defaultEmailAddress);

                                defaultEmailAddress = new MailAddress(ConfigurationManager.AppSettings["default2"]);
                                msg.CC.Add(defaultEmailAddress);

                                defaultEmailAddress = new MailAddress(ConfigurationManager.AppSettings["default3"]);
                                msg.CC.Add(defaultEmailAddress);

                                defaultEmailAddress = new MailAddress(ConfigurationManager.AppSettings["default4"]);
                                msg.CC.Add(defaultEmailAddress);

                                /****Add Default email Address - End*****/

                                msg.To.Add(emp.email);
                                msg.Subject = "DT Timesheet Reminder";
                                msg.Body = mailBody;
                                msg.IsBodyHtml = true;

                                SendEmailInBackgroundThread(msg);
                            }
                        }
                    }
                }

                /****Setup timer for next day***/
            }
            catch (Exception e)
            {
                isError++;
                if (isError == 2)
                {
                    SetUpTimer();
                }
                else
                {
                    await FindDefaulter();
                    Console.Write(e.InnerException);
                }

            }

            SetUpTimer();
        }
    }
}