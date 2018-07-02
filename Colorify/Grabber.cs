using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Controls;

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

            String colorName = Regex.Replace(GetClosestColor(colors, color).Name, "(\\B[A-Z])", " $1");
            Console.WriteLine(colorName);
            ShowColorInfo(color, colorName);
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
         * Opens a form displaying the selected color and name.
         * Only temporary, will create a seperate form to use.
         */
        private void ShowColorInfo(Color rgb, String name)
        {
            // Show WPF-window.
            ColorInfo window = new Colorify.ColorInfo();
            TextBlock RGB = (TextBlock)window.FindName("RGB");
            TextBlock HEX = (TextBlock)window.FindName("HEX");
            TextBlock ColorName = (TextBlock)window.FindName("ColorName");
            Grid Background = (Grid)window.FindName("Background");
            Background.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(rgb.A, rgb.R, rgb.G, rgb.B));
            RGB.Text = "RGB: (" + rgb.R + ", " + rgb.G + ", " + rgb.B + ")";
            HEX.Text = "HEX: #" + string.Format("{0:X2}{1:X2}{2:X2}", rgb.R, rgb.G, rgb.B);
            ColorName.Text = "COLOR: " + name;
            window.Show();
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
