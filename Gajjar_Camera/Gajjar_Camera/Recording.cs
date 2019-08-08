using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;


namespace Gajjar_Camera
{
    public partial class Recording : Form
    {

        private VideoCaptureDevice FinalVideo = null;
        private Bitmap video;
        private SaveFileDialog saveAvi;

        public Recording()
        {
            InitializeComponent();
        }
        private FilterInfoCollection capturedevice;
        private VideoCaptureDevice FinalFrame;

        private void Recording_Load(object sender, EventArgs e)
        {
            capturedevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo Devices in capturedevice)
            {
                comboBox1.Items.Add(Devices.Name);
            }
            comboBox1.SelectedIndex = 0;
            FinalFrame = new VideoCaptureDevice();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FinalFrame = new VideoCaptureDevice(capturedevice[comboBox1.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_newFrame);
            FinalFrame.Start();
        }

        private void FinalFrame_newFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            video = (Bitmap)eventArgs.Frame.Clone();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            FinalFrame.Stop();
                           

            saveAvi = new SaveFileDialog();
            saveAvi.Filter = "Avi Files (*.avi)|*.avi";


            if (saveAvi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                //int h = capturedevic.
                //int w = capturedevice.VideoDevice.VideoResolution.FrameSize.Width;
                //FileWriter.Open(saveAvi.FileName, w, h, 25, VideoCodec.Default, 5000000);
                //FileWriter.WriteVideoFrame(video);

                //AVIwriter.Open(saveAvi.FileName, w, h);
                //FinalVideo = captureDevice.VideoDevice;
                //FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                //FinalVideo.Start();
            }
        }
    }
}
