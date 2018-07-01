using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Colorify
{
    public class Grabber
    {
        private Bitmap screenshot = null;
        private Form background = null;

        /*
         * Function that is executed when the user requests to grab a color.
         * Adds click-event that does magic. 
         */
        public void GrabColor()
        {
            int x = 0;
            int y = 0;
            ShowTempForm();
            background.Cursor = Cursors.Cross;
            // Add a click-event that takes the screenshot then gets the pixeldata right after taking the picture, 
            // incase of moving background such as videos.
            background.MouseClick += (object sender, MouseEventArgs e) =>
            {
                x = e.X;
                y = e.Y;
                Console.WriteLine("X: " + e.X + " Y: " + e.Y);
                TakeScreenshot();
                GetPixelData(x, y);
                background.Close();
            };
        }

        /*
         * Takes a screenshot of the PrimaryScreen.
         * TODO: Make it so it screenshots all active monitors but split it into three images.
         */
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

        /*
         * Get the selected pixel from the previously taken screenshot.
         * Takes the pixel ARGB and checks the closest color in the enum KnownColor
         */
        private void GetPixelData(int x, int y)
        {
            Color color = screenshot.GetPixel(x, y);

            // Ghetto way of making an array to a list
            List<KnownColor> colors = ((KnownColor[])Enum.GetValues(typeof(KnownColor))).ToList();

            String colorName = GetClosestColor(colors, color).Name;
            Console.WriteLine(colorName);
        }

        /*
         * Create and display a form that will be used as a background so the 
         * click will not interfere with anything on the screen.
         */
        private void ShowTempForm()
        {
            Form form = new Form();
            form.FormBorderStyle = FormBorderStyle.None;
            form.WindowState = FormWindowState.Maximized;
            form.BackColor = Color.Black;
            //form.TransparencyKey = Color.Aquamarine;
            form.Opacity = 0.25;
            form.Show();

            background = form;
        }

        /*
         * Gets the closest color from the KnownColor enumerations and returns it.
         * @Color
         */
        private static Color GetClosestColor(List<KnownColor> tempArr, Color baseColor)
        {
            List<Color> colorArray = new List<Color>();
            tempArr.ForEach(c => colorArray.Add(Color.FromKnownColor(c)));

            var colors = colorArray.Select(x => new { Value = x, Diff = GetDiff(x, baseColor) }).ToList();
            var min = colors.Min(x => x.Diff);
            return colors.Find(x => x.Diff == min).Value;
        }

        private static int GetDiff(Color color, Color baseColor)
        {
            int a = color.A - baseColor.A,
                r = color.R - baseColor.R,
                g = color.G - baseColor.G,
                b = color.B - baseColor.B;
            return a * a + r * r + g * g + b * b;
        }
    }
}
