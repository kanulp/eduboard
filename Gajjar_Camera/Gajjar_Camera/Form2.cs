using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using System.IO;

namespace Gajjar_Camera
{
    public partial class Form2 : Form
    {
        private FilterInfoCollection VideoCaptureDevices;

        private VideoCaptureDevice FinalVideo = null;
        private VideoCaptureDeviceForm captureDevice;
        private Bitmap video;
        //private AVIWriter AVIwriter = new AVIWriter();
        private VideoFileWriter FileWriter = new VideoFileWriter();
        private SaveFileDialog saveAvi;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                captureDevice = new VideoCaptureDeviceForm();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());

            }
           


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
        }
        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            //video = (Bitmap)eventArgs.Frame.Clone();
            //pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            //AVIwriter.Quality = 0;
            //AVIwriter.AddFrame(video);
            try
            {
                video = (Bitmap)eventArgs.Frame.Clone();
                pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            }
            catch (Exception ee) {
                MessageBox.Show(ee.ToString());

            }
        }

       

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (FinalVideo == null)
            { return; }
            if (FinalVideo.IsRunning)
            {
                //this.FinalVideo.Stop();
                FileWriter.Close();
                //this.AVIwriter.Close();
                pictureBox1.Image = null;
            }

            else
            {
                FileWriter.WriteVideoFrame(video);

                this.FinalVideo.Stop();
                FileWriter.Close();
                //this.AVIwriter.Close();
                pictureBox1.Image = null;
            }
        }

        private void button3_Click(object sender, EventArgs e)//save
        {
            saveAvi = new SaveFileDialog();
            saveAvi.Filter = "Avi Files (*.avi)|*.avi";
            if (saveAvi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int h = captureDevice.VideoDevice.VideoResolution.FrameSize.Height;
                int w = captureDevice.VideoDevice.VideoResolution.FrameSize.Width;
                FileWriter.Open(saveAvi.FileName, w, h, 25, VideoCodec.Default, 5000000);
                FileWriter.WriteVideoFrame(video);

                //AVIwriter.Open(saveAvi.FileName, w, h);
                //FinalVideo = captureDevice.VideoDevice;
                //FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                //FinalVideo.Start();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
