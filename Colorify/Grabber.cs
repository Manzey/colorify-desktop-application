using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Colorify
{
    public class Grabber
    {
        private Bitmap screenshot = null;
        private Form tempForm = null;

        public void GrabColor()
        {
            ShowTempForm();
            // Click event for mouse
            // Get pixel
            // GetPixelData(x,y)
            TakeScreenshot();
        }

        private void TakeScreenshot()
        {
            Bitmap bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmpScreenCapture);
            g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                Screen.PrimaryScreen.Bounds.Y,
                                0, 0,
                                bmpScreenCapture.Size,
                                CopyPixelOperation.SourceCopy);
            //bmpScreenCapture.Save(@"C:\Temp\temp.png", ImageFormat.Png);
            screenshot = bmpScreenCapture;
        }

        private void GetPixelData(int x, int y)
        {
            Color c = screenshot.GetPixel(x, y);
            Console.WriteLine("R: " + c.R + " G: " + c.G + " B: " + c.B);
        }

        private void ShowTempForm()
        {
            Form form = new Form();
            form.FormBorderStyle = FormBorderStyle.None;
            form.WindowState = FormWindowState.Maximized;
            form.BackColor = Color.Black;
            //form.TransparencyKey = Color.Aquamarine;
            form.Opacity = 0.25;
            form.Show();

            tempForm = form;
        }
    }
}
