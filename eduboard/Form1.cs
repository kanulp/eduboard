using Accord.Video;
using Accord.Video.DirectShow;
using Accord.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace eduboard
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCaptureDevices;

        private VideoCaptureDevice FinalVideo = null;
        private VideoCaptureDeviceForm captureDevice;
        private Bitmap video;
        //private AVIWriter AVIwriter = new AVIWriter();
        private VideoFileWriter FileWriter = new VideoFileWriter();
        private SaveFileDialog saveAvi;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            captureDevice = new VideoCaptureDeviceForm();


        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            if (captureDevice.ShowDialog(this) == DialogResult.OK)
            {

                //VideoCaptureDevice videoSource = captureDevice.VideoDevice;
                FinalVideo = captureDevice.VideoDevice;
                FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                FinalVideo.Start();
            }
            saveAvi = new SaveFileDialog();
            saveAvi.Filter = "Avi Files (*.avi)|*.avi";
            if (saveAvi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int h = captureDevice.VideoDevice.VideoResolution.FrameSize.Height;
                int w = captureDevice.VideoDevice.VideoResolution.FrameSize.Width;

                FileWriter.Open(saveAvi.FileName, w, h, 20, VideoCodec.MPEG4,9000000);


                FileWriter.WriteVideoFrame(video);

                //AVIwriter.Open(saveAvi.FileName, w, h);
                button3.Text = "Stop Record";
                //FinalVideo = captureDevice.VideoDevice;
                //FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                //FinalVideo.Start();
            }

        }
        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (button3.Text == "Stop Record")
            {
                video = (Bitmap)eventArgs.Frame.Clone();
                pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
                //AVIwriter.Quality = 0;
                FileWriter.WriteVideoFrame(video);
                //AVIwriter.AddFrame(video);
            }
            else
            {
                video = (Bitmap)eventArgs.Frame.Clone();
                pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //saveAvi = new SaveFileDialog();
            //saveAvi.Filter = "Avi Files (*.avi)|*.avi";
            //if (saveAvi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    int h = captureDevice.VideoDevice.VideoResolution.FrameSize.Height;
            //    int w = captureDevice.VideoDevice.VideoResolution.FrameSize.Width;
            //    FileWriter.Open(saveAvi.FileName, w, h, 30, VideoCodec.Default);
            //    FileWriter.WriteVideoFrame(video);

            //    //AVIwriter.Open(saveAvi.FileName, w, h);
            //    button3.Text = "Stop Record";
            //    //FinalVideo = captureDevice.VideoDevice;
            //    //FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            //    //FinalVideo.Start();
            //}

            try
            {
                string MyConnection2 = "server =192.168.50.16; database=vishwakarma; port=3306;username=vishwakarma;password=vishwakarma";
                string Query = "SELECT * from student";
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MyConn2.Open();
                MySqlDataReader reader = MyCommand2.ExecuteReader();
                String ss = "";
                while (reader.Read())
                {
                    ss += reader["user"];
                }

                MessageBox.Show(ss);
               
                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Stop Record")
            {
                button3.Text = "Stop";
                if (FinalVideo == null)
                { return; }
                if (FinalVideo.IsRunning)
                {
                    //this.FinalVideo.Stop();
                    FileWriter.Close();
                    //this.AVIwriter.Close();
                    pictureBox1.Image = null;
                }
            }
            else
            {
                this.FinalVideo.Stop();
                FileWriter.Close();
                //this.AVIwriter.Close();
                pictureBox1.Image = null;
            }
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            //  FtpUploadFile(openFileDialog.FileName);
             // myupload(openFileDialog.FileName);
            Console.Write("mt",openFileDialog.FileName);

           // mov(openFileDialog.FileName);

            // mymyupload(openFileDialog.FileName);
           // FtpUploadFile(openFileDialog.FileName);
           
            mov(openFileDialog.FileName);
          // UploadFilesToRemoteUrl("http://192.168.43.159/AICTE/videos/", openFileDialog.FileName);
        }

        public static void mov(String filename) {

            using (var client = new WebClient())
            {
                 var response = client.UploadFile("http://192.168.43.159/AICTE/uploads.php?submit=true&action=upload", "POST", filename);

                Console.WriteLine("\nResponse Received.The contents of the file uploaded are:\n{0}",
                                System.Text.Encoding.ASCII.GetString(response));
            }
            

            //var fileName = Path.GetFileName(filename);
            //var request = (FtpWebRequest)WebRequest.Create("ftp://192.168.43.159/" + fileName);

            //request.Method = WebRequestMethods.Ftp.UploadFile;
            //request.Credentials = new NetworkCredential("sammy", "sammy");
            //request.UsePassive = true;
            //request.UseBinary = true;
            //request.KeepAlive = false;

            //using (var fileStream = File.OpenRead(filename))
            //{
            //    using (var requestStream = request.GetRequestStream())
            //    {
            //        fileStream.CopyTo(requestStream);
            //        requestStream.Close();
            //    }
            //}

            //var response = (FtpWebResponse)request.GetResponse();
            //Console.WriteLine("Upload done: {0}", response.StatusDescription);
            //response.Close();

            //FtpWebRequest request;
            //try
            //{

            //    string absoluteFileName = Path.GetFileName(filename);

            //    request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", "192.168.43.159", "ftp", absoluteFileName))) as FtpWebRequest;
            //    request.Method = WebRequestMethods.Ftp.UploadFile;
            //    request.UseBinary = true;
            //    request.UsePassive = true;
            //    request.KeepAlive = true;
            //    request.Credentials = new NetworkCredential("sammy", "sammy");
            //    //request.ConnectionGroupName = "group";

            //    using (FileStream fs = File.OpenRead(filename))
            //    {
            //        byte[] buffer = new byte[fs.Length];
            //        fs.Read(buffer, 0, buffer.Length);
            //        fs.Close();
            //        Stream requestStream = request.GetRequestStream();
            //        requestStream.Write(buffer, 0, buffer.Length);
            //        requestStream.Flush();
            //        requestStream.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}

            /*FileInfo fileInf = new FileInfo(filename);
           
            string uri = "ftp://192.168.43.159/" + fileInf.Name;
            FtpWebRequest reqFTP;

            // Create FtpWebRequest object from the Uri provided
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(
                      "ftp://192.168.43.159/" + fileInf.Name));

            // Provide the WebPermission Credintials
            reqFTP.Credentials = new NetworkCredential("sammy",
                                                       "sammy");

            // By default KeepAlive is true, where the control connection is 
            // not closed after a command is executed.
            reqFTP.KeepAlive = false;

            // Specify the command to be executed.
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            // Specify the data transfer type.
            reqFTP.UseBinary = true;

            // Notify the server about the size of the uploaded file
            reqFTP.ContentLength = fileInf.Length;

            // The buffer size is set to 2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            // Opens a file stream (System.IO.FileStream) to read 
            //the file to be uploaded
            FileStream fs = fileInf.OpenRead();

            try
            {
                // Stream to which the file to be upload is written
                Stream strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time
                contentLen = fs.Read(buff, 0, buffLength);

                // Till Stream content ends
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the 
                    // FTP Upload Stream
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                // Close the file stream and the Request Stream
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Upload Error");
            }*/

        }

        public static void mymyupload(String filename2) {

            try
            {
                string filename = Path.GetFileName(filename2);
                Console.Write(filename2);
               // string ftpfullpath = "@192.168.43.159/ftp";
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(@"ftp://192.168.43.159/ftp"));
                ftp.Credentials = new NetworkCredential("sammy", "sammy");

                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                FileStream fs = File.OpenRead(filename2);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    


        /*public static void myupload2(String filename) {
         

            WebClient wClient = new WebClient();

            string QueryString = "UserID=" + HttpUtility.UrlEncode(this.txtBx_UserInspectorID.Text)
              + "&InspectionID=" + HttpUtility.UrlEncode(this.txtBx_InspectionID.Text)
           + "&submit=" + HttpUtility.UrlEncode("submit")
           + "&FileName=" + HttpUtility.UrlEncode();

            byte[] Byte_response = wClient.UploadFile("http://localhost/Johny_FileUpload.php?" + QueryString, "POST", this.txtBx_ImagePath.Text);

            if (Byte_response != null)
            {
                //this.txtBox_DataToPass.Text = QueryString;
                MessageBox.Show("done");
//                this.richTextBox1.Text = System.Text.Encoding.ASCII.GetString(Byte_response);
            }



       
    }*/

       public static void myupload(String filename) {

            string endPoint = "http://192.168.43.159/AICTE/uploads.php";
            string filePath = filename;

            WebClient wc = new WebClient();

            // fired when upload progress is changed
            // this updates a progressbar named pbProgress
            //wc.UploadProgressChanged += (o, ea) =>
            //{
            //    if (ea.ProgressPercentage >= 0 && ea.ProgressPercentage <= 100)
            //        pbProgress.Value = ea.ProgressPercentage;
            //};

            // fired when the file upload is complete
            // fired if an error occurs or if it's successful
            try
            {

            
            wc.UploadFileCompleted += (o, ea) =>
            {
                // determine if upload failed or not
                if (ea.Error == null)
                {
                    // response will let us know if there
                    // was an error on the PHP side
                    string response = Encoding.UTF8.GetString(ea.Result);
                    if (response == "Success")
                        Console.Write("success");
                    // lblStatus.Text = "Upload Complete.";
                    else
                    {
                        // lblStatus.Text = "Upload Failed.";
                        MessageBox.Show(response);
                    }
                }
                else
                {
                   // lblStatus.Text = "Upload Failed.";
                    MessageBox.Show(ea.Error.Message);
                }
            };

            // set the status to Uploading
          //  lblStatus.Text = "Uploading...";

            // tell the webclient to upload the file
            wc.UploadFileAsync(new Uri(endPoint), filePath);
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.ToString());
            }
        }

        public static string UploadFilesToRemoteUrl(string url, string files, NameValueCollection formFields = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");



            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" +
                                    boundary;
            request.Method = "POST";
            request.KeepAlive = true;

            Stream memStream = new System.IO.MemoryStream();

            var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
                                                                    boundary + "\r\n");
            var endBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
                                                                        boundary + "--");


            string formdataTemplate = "\r\n--" + boundary +
                                        "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

            if (formFields != null)
            {
                foreach (string key in formFields.Keys)
                {
                    string formitem = string.Format(formdataTemplate, key, formFields[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }
            }

            string headerTemplate =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                "Content-Type: application/octet-stream\r\n\r\n";

            for (int i = 0; i < files.Length; i++)
            {
                memStream.Write(boundarybytes, 0, boundarybytes.Length);
                var header = string.Format(headerTemplate, "uplTheFile", files[i]);
                var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                memStream.Write(headerbytes, 0, headerbytes.Length);

                using (var fileStream = new FileStream(files, FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[1024];
                    var bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        memStream.Write(buffer, 0, bytesRead);
                    }
                }
            }

            memStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            request.ContentLength = memStream.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            }

            using (var response = request.GetResponse())
            {
                Stream stream2 = response.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                return reader2.ReadToEnd();
            }
        }

        private void FtpUploadFile(string filename)
        {
            try
            {
                String uriString = "http://192.168.43.159/AICTE/upload/";
                Console.Write("gajjar", filename);

                WebClient myWebClient = new WebClient();

                Console.WriteLine("\nPlease enter the fully qualified path of the file to be uploaded to the URI");
                string fileName = filename;
                Console.WriteLine("Uploading {0} to {1} ...", fileName, uriString);

                // Upload the file to the URI.
                // The 'UploadFile(uriString,fileName)' method implicitly uses HTTP POST method.
                byte[] responseArray = myWebClient.UploadFile(uriString, fileName);

                // Decode and display the response.
                Console.WriteLine("\nResponse Received.The contents of the file uploaded are:\n{0}",
                    System.Text.Encoding.ASCII.GetString(responseArray));
            }
            catch (Exception ee) {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}
