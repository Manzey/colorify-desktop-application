using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Colorify
{
    public class MyApplicationContext : ApplicationContext
    {
        NotifyIcon notifyIcon = new NotifyIcon();
        public MyApplicationContext()
        {
            MenuItem configMenuItem = new MenuItem("Grab color", new EventHandler(GrabColor));
            MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));

            
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Colorify.Properties.Resources.AppIcon;
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[]
                { configMenuItem, exitMenuItem
                });
            notifyIcon.Visible = true;
        }

        void GrabColor(object sender, EventArgs e)
        {
           Grabber grab = new Grabber();
           // Should be performed at the same time, take screenshot then get colors.
           grab.GrabColor();           
        }

        void Exit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
