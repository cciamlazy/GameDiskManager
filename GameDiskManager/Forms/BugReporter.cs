using GameDiskManager.Types.Data;
using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameDiskManager.Forms
{
    public partial class BugReporter : Form
    {
        private ErrorLog log;
        public BugReporter(ErrorLog errorLog = null)
        {
            InitializeComponent();
            log = errorLog;
            if (Setting.Value.Bugs_Remember == 1)
            {
                button1_Click(null, null);
            }
            else if (Setting.Value.Bugs_Remember == 2)
            {
                button2_Click(null, null);
            }
            else
                this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (log != null)
            {
                string path = Path.Combine(Program.DataPath + "ErrorLog", log.Date + " " + log.Time + " Error.json");
                //SendMail(MainData.Instance.SmtpAuth.Recipient, "Error Log on " + log.Date, (!File.Exists(path) ? Serializer<ErrorLog>.ObjectToJSONString(log) + "\r\n\r\n" : "") + textBox1.Text, path);
            }
            else
            {
                //SendMail(MainData.Instance.SmtpAuth.Recipient, "Bug", textBox1.Text);
            }
            if (checkBox1.Checked)
            {
                Setting.Value.Bugs_Remember = 1;
                Setting.Save();
            }
            this.Close();
        }

        public static void SendMail(string recipient, string subject, string body, string attachmentFilename = "")
        {
            /*SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential(MainData.Instance.SmtpAuth.Username, MainData.Instance.SmtpAuth.Password);
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(MainData.Instance.SmtpAuth.Username);

            // setup up the host, increase the timeout to 5 minutes
            smtpClient.Host = MainData.Instance.SmtpAuth.SmtpServer;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.Timeout = (60 * 5 * 1000);
            smtpClient.EnableSsl = true;

            message.From = fromAddress;
            message.Subject = subject;
            message.IsBodyHtml = false;
            message.Body = body;
            message.To.Add(recipient);

            if (attachmentFilename != null && attachmentFilename != "" && File.Exists(attachmentFilename))
            {
                Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                disposition.FileName = Path.GetFileName(attachmentFilename);
                disposition.Size = new FileInfo(attachmentFilename).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                message.Attachments.Add(attachment);
            }

            smtpClient.Send(message);*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Setting.Value.Bugs_Remember = 2;
                Setting.Save();
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(log.StackTrace);
        }
    }
}
