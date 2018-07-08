using System;
using System.Windows;
using System.Windows.Input;

namespace Colorify
{
    /// <summary>
    /// Interaction logic for ColorInfo.xaml
    /// </summary>
    public partial class ColorInfo : Window
    {
        public ColorInfo()
        {
            try { 
                InitializeComponent();                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}