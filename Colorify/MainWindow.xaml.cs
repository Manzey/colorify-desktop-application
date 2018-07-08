using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Colorify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Grabber grab = new Grabber();
        private HotKey hotKey;

        public MainWindow()
        {
            InitializeComponent();
            hotKey = new HotKey(Key.C, KeyModifier.Ctrl | KeyModifier.Shift, Trigger);
        }

        private void Trigger(HotKey hotKey)
        {
            grab.GrabColor();
        }
    }
}
