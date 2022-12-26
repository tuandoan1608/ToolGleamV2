using GleamV2.lib;
using KAutoHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GleamV2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var screen = CaptureHelper.CaptureImage(new Size(1000, 1000), new Point(0, 0));
            screen.Save("f.png");
            var sub = ImageScanOpenCV.GetImage("image\\Screenshot_1.png");
            ImageService img = new ImageService();
            img.FindImage(screen, sub, 0);
        }
    }
}
