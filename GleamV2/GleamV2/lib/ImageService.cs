using KAutoHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GleamV2.lib
{
    public class ImageService
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, int dwExtraInfo);
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;
        void sendMouseDoubleClick(Point p)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)684, (uint)64, 0, 0);
        }
        public void FindImage(Bitmap screen,Bitmap subCreen,int check)
        {
            bool result = false;
            while (result == false)
            {

                Point resImagePoint = (Point)ImageScanOpenCV.FindOutPoint(screen, subCreen);
                Thread.Sleep(5000);
                if (resImagePoint != null)
                {
                    sendMouseDoubleClick(resImagePoint);
                    result = true;
                    // AutoControl.MouseClick(resImagePoint,EMouseKey.LEFT);
                }
            }
        }
    }
}
