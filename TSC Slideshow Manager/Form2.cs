using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Net;
using System.Threading;
using System.IO;

namespace TSC_Slideshow_Manager
{
    public partial class Form2 : Form
    {
        public Spajanje spajanje;
        public string Username;
        public string Filename;
        public string Fullname;
        public string Server;
        public string Password;
        public string path;
        public string localdest;
        public Form2(Spajanje spajanje,string ime,string lozinka)
        {
            InitializeComponent();
            this.spajanje = spajanje;
            this.Username = ime;
            this.Password = lozinka;
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            //comboBox1.Items.Add("ftp://127.0.0.1:14147");
            SqlCommand naredba;
            SqlDataReader dataReader;
            String sql;
            sql = "select * from uredaji";
            this.spajanje.cnn.Open();
            naredba = new SqlCommand(sql, this.spajanje.cnn);
            dataReader = naredba.ExecuteReader();
            while (dataReader.Read())
            {
                comboBox1.Items.Add(dataReader.GetValue(0) + "-" + dataReader.GetValue(1) + "-" + dataReader.GetValue(2));
            }

            naredba.Dispose();
            this.spajanje.cnn.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (radioDown.Checked == true)
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Filename)));

                request.Credentials = new NetworkCredential(Username, Password);
                request.Method = WebRequestMethods.Ftp.DownloadFile;  


                FtpWebRequest request1 = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Filename)));
                request1.Credentials = new NetworkCredential(Username, Password);
                request1.Method = WebRequestMethods.Ftp.GetFileSize;  
                FtpWebResponse response = (FtpWebResponse)request1.GetResponse();
                double total = response.ContentLength;
                response.Close();

                FtpWebRequest request2 = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Filename)));
                request2.Credentials = new NetworkCredential(Username, Password);
                request2.Method = WebRequestMethods.Ftp.GetDateTimestamp; 
                FtpWebResponse response2 = (FtpWebResponse)request2.GetResponse();
                DateTime modify = response2.LastModified;
                response2.Close();


                Stream ftpstream = request.GetResponse().GetResponseStream();
                FileStream fs = new FileStream(localdest, FileMode.Create);

                byte[] buffer = new byte[1024];
                int byteRead = 0;
                double read = 0;
                do
                {
                    byteRead = ftpstream.Read(buffer, 0, 1024);
                    fs.Write(buffer, 0, byteRead);
                    read += (double)byteRead;
                    double percentage = read / total * 100;
                    backgroundWorker1.ReportProgress((int)percentage);
                }
                while (byteRead != 0);
                ftpstream.Close();
                fs.Close();


            }
            else
            {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Filename)));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(Username, Password);
                Stream ftpstream = request.GetRequestStream();
                FileStream fs = File.OpenRead(Fullname);

                byte[] buffer = new byte[1024];
                double total = (double)fs.Length;
                int byteRead = 0;
                double read = 0;
                do
                {
                    byteRead = fs.Read(buffer, 0, 1024);
                    ftpstream.Write(buffer, 0, byteRead);
                    read += (double)byteRead;
                    double percentage = read / total * 100;
                    backgroundWorker1.ReportProgress((int)percentage);
                }
                while (byteRead != 0);
                fs.Close();
                ftpstream.Close();

            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (radioDown.Checked == true)
            {
                labelStatus.Text = $"Downloaded {e.ProgressPercentage}%";
                labelStatus.Update();
                progressBar1.Value = e.ProgressPercentage;
                progressBar1.Update();
            }

            else if (radioUp.Checked == true)
            {
                labelStatus.Text = $"Uploaded {e.ProgressPercentage}%";
                labelStatus.Update();
                progressBar1.Value = e.ProgressPercentage;
                progressBar1.Update();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (radioDown.Checked == true)
            {
                labelStatus.Text = "Download Complete!";
            }

            else if (radioUp.Checked == true)
            {
                labelStatus.Text = "Upload Complete!";
            }
        }

        private void radioDown_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDown.Checked == false)
            {
                radioUp.Enabled = true;
                radioUp.Checked = true;
                button1.Text = @"UPLOAD";
                //label5.Enabled = false;
                labelStatus.Text = @"Uploaded 0%";
            }
        }

        private void radioUp_CheckedChanged(object sender, EventArgs e)
        {
            if (radioUp.Checked == false)
            {
                radioDown.Checked = true;
                button1.Text = @"DOWNLOAD";
                //label5.Enabled = true;
               // textBox4.Enabled = true;
                labelStatus.Text = @"Downloaded 0%";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioUp.Checked == true)
            {

                using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true, ValidateNames = true, Filter = "All Files|*.*" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fi = new FileInfo(ofd.FileName);
                        //Username = textBox1.Text;
                        // Password = textBox2.Text;
                        Server = comboBox1.SelectedItem.ToString();
                        Filename = fi.Name;
                        Fullname = fi.FullName;
                    }
                }
            }


            if (radioDown.Checked == true)
            {
                //Username = textBox1.Text;
                //Password = textBox2.Text;
                Server = comboBox1.SelectedItem.ToString();
                //Filename = textBox4.Text;
                path = @"C:\Users\MSI\";
                localdest = path + @"" + Filename;
                Fullname = Server + @"/" + Filename;
            }

            Thread.Sleep(1000);
            backgroundWorker1.RunWorkerAsync();
            Thread.Sleep(1000);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (spajanje.stanje)
            {
                Dodavanje dod = new Dodavanje(spajanje);
                dod.ShowDialog();
                this.Close();
            }
        }
    }
}
