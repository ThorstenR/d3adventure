using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Drawing;
using Utilities.ScreenShot;
using Utilities.PixelPower;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;

namespace Utilities.WebTools
{
    public static class WT
    {
        private static string _subject = "";
        private static string _body = "";
        private static Attachment _attachment = null;

        public static void SendEmail(string subject, string body)
        {
            if (subject == "" || body == "")
            {
                throw new Exception("Email needs a subject and body!");
            }

            _subject = subject;
            _body = body;

            Thread myThread = new Thread(sendEmailThread);
            myThread.Name = "Send Email Thread";
            myThread.Start();
        }

        public static void SendEmail(string subject, string body, Attachment attachment)
        {
            _attachment = attachment;
            SendEmail(subject, body);
        }

        private static void sendEmailThread()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                
                mail.From = new MailAddress("noreply@2csUG.com");
                mail.To.Add("2csUG.SC2@gmail.com");
                mail.Subject = _subject;
                mail.Body = _body;

                if (_attachment != null)
                {
                    mail.Attachments.Add(_attachment);
                }

                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("2csUG.SC2", "2csUGftw");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);

                if (_attachment != null)
                {
                    _attachment.Dispose();
                }
            }
            catch
            {
                // Bad attachment? Can't connect to server? etc?
            }
        }

        public static Stream FetchPageStream(string httpUrl)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(httpUrl);// prepare the web page we will be asking for
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response.GetResponseStream();// we will read data via the response stream
            }
            catch
            {
                return null;
            }
        }

        // http://www.csharp-station.com/HowTo/HttpWebFetch.aspx
        public static string FetchPage(string httpUrl)
        {
            StringBuilder sb = new StringBuilder();

            byte[] buf = new byte[8192]; // buffer for request

            Stream resStream = FetchPageStream(httpUrl);

            if (resStream == null)
                return null;

            string tempString = null;
            int count = 0;
            do
            {
                try
                {
                    count = resStream.Read(buf, 0, buf.Length);// fill the buffer with data
                }
                catch
                {
                    return null;
                }

                if (count != 0)// make sure we read some data
                {
                    tempString = Encoding.ASCII.GetString(buf, 0, count);// translate from bytes to ASCII text
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?

            return sb.ToString();
        }

        private static string[] blacklistedHosts = { "qazzy-desktop" };
        public static void ReportCrash(Exception ex, string appplicationTitle, IntPtr? handleToScreenShot = null, Size? screenShotSize = null, bool messageBox = true)
        {
            Attachment attachment = null;
            if (handleToScreenShot != null)
            {
                Bitmap snapshotOriginal = CaptureScreen.GetWindowImage(handleToScreenShot.Value);
                Bitmap snapshot = PixelTools.FixedSize(snapshotOriginal, screenShotSize.Value.Width, screenShotSize.Value.Height);
                MemoryStream ms = new MemoryStream();
                snapshot.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                ms.Position = 0;

                snapshot.Dispose();
                snapshotOriginal.Dispose();

                attachment = new Attachment(ms, "snapshot.jpg");
            }

            string dns = Dns.GetHostName();
            string crashReport = "Computer: " + dns;
            crashReport += "\n\n";
            crashReport += ex.Message;
            crashReport += "\n\n";
            crashReport += "Main Exception";
            crashReport += "\n";
            crashReport += ex.StackTrace;

            Exception innerException = ex.InnerException;

            for (int i = 1; innerException != null; i++)
            {
                crashReport += "\n\n";
                crashReport += "Inner Exception #" + i;
                crashReport += "\n";
                crashReport += innerException.StackTrace;

                innerException = innerException.InnerException;
            }

            if (messageBox == true)
            {
                DialogResult result = MessageBox.Show("An unexpected error has occursed. This problem has been reported. Would you like to help debug this problem?", "Unexpected Crash", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                bool isBlackedListed = blacklistedHosts.FirstOrDefault(f => f.ToLower() == dns.ToLower()) != null;

                if (isBlackedListed == true)
                    return;

                if (result == DialogResult.Yes)
                {
                    string email = "";
                    result = InputBox(appplicationTitle + ": Crash Report", "Enter an email where you can be contacted at.", ref email);

                    if (result == DialogResult.OK)
                        crashReport = "Contact Email: " + email + "\n\n" + crashReport;
                }
            }

            SendEmail(appplicationTitle + ": Crash Report", crashReport, attachment);
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        public static void CheckForSourceForgeUpdates(string sourceForgeProjectName, string currentVersion)
        {
            string url = "https://sourceforge.net/projects/" + sourceForgeProjectName + "/files/";
            string rawPage = FetchPage(url);

            if (string.IsNullOrEmpty(rawPage) == true)
                return;

            int projectTitleIndexStart = rawPage.IndexOf("<title>") + 7;
            int projectTitleIndexEnd = rawPage.IndexOf("</title>", projectTitleIndexStart);

            string projectTitle = rawPage.Substring(projectTitleIndexStart, projectTitleIndexEnd - projectTitleIndexStart);
            projectTitle = projectTitle.Remove(projectTitle.IndexOf(" - "));

            int tableIndex = rawPage.IndexOf("<table id=\"files_list\"");
            int tbodyIndex = rawPage.IndexOf("<tbody>", tableIndex);
            int titleIndexStart = rawPage.IndexOf("title=\"", tbodyIndex) + 7;
            int titleIndexEnd = rawPage.IndexOf("\"", titleIndexStart);

            string title = rawPage.Substring(titleIndexStart, titleIndexEnd - titleIndexStart);
            title = title.Replace(projectTitle, "").Replace("beta", "").Replace(" ", "").Replace(".zip", "").Replace(".msi", "");
            title = title.Remove(title.LastIndexOf('.'));

            Version version = Version.Parse(currentVersion);
            Version latestVersion = Version.Parse(title);
            
            if (version >= latestVersion)
                return;

            DialogResult result = MessageBox.Show("There is a new version availiable. Would you like to open the download page?", "New Version", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                int downloadIndexStart = rawPage.IndexOf("href=\"", tbodyIndex) + 6;
                int downloadIndexEnd = rawPage.IndexOf("\"", downloadIndexStart);
                string downloadUrl = rawPage.Substring(downloadIndexStart, downloadIndexEnd - downloadIndexStart);
                Process.Start(downloadUrl);
            }
        }
    }
}
