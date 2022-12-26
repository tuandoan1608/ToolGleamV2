using GleamV2.lib;
using KAutoHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GleamV2
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);
        public JArray jsonObj;
        int screenWidth;
        int windowHeight;
        DataTable dt;
        List<IntPtr> listchrome;
        List<IntPtr> listchromeNew;
        Bitmap BMP_FOLLOW = ImageScanOpenCV.GetImage("image\\Screenshot_1.png");
        Bitmap BMP_TASK_COMPLETED = ImageScanOpenCV.GetImage("image\\Screenshot_1.png");
        Bitmap BMP_RETWEET = ImageScanOpenCV.GetImage("image\\Screenshot_1.png");
        Bitmap BMP_TWEET = ImageScanOpenCV.GetImage("image\\Screenshot_1.png");
        Bitmap BMP_BUTTON_FOLLOW = ImageScanOpenCV.GetImage("image\\Screenshot_1.png");
        Bitmap BMP_BUTTON_CONTINUTE = ImageScanOpenCV.GetImage("image\\Screenshot_1.png");
        Bitmap BMT_SAVE = ImageScanOpenCV.GetImage("image\\Screenshot_1.png");
        public Form1()
        {
            InitializeComponent();
            listchrome = new List<IntPtr>();
            listchromeNew = new List<IntPtr>();
        }
        private void openGologin(string userdata, string port)
        {
            string path = "C:\\Users\\thanh\\OneDrive\\Desktop\\62fc5305e8962841db90aade\\orbita-browser\\chrome.exe";
            var proc1 = new ProcessStartInfo();
            string anyCommand;
            proc1.UseShellExecute = false;
            anyCommand = "Start \"\" \""+path+ "\" --remote-debugging-port=" + port + " --user-data-dir=\"" + userdata + "\" --disable-popup-blocking";
            proc1.WorkingDirectory = @"C:\Windows\System32";
            proc1.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.Verb = "runas";
            proc1.Arguments = "/c " + anyCommand;
            proc1.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(proc1);


        }
        private static int findFreePort()
        {
            var socket = new Socket(
                AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                var localEP = new System.Net.IPEndPoint(IPAddress.Any, 0);
                socket.Bind(localEP);
                localEP = (System.Net.IPEndPoint)socket.LocalEndPoint;
                return localEP.Port;
            }
            finally
            {
                socket.Close();
            }
        }
        private void changePreferen(string path)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(File.ReadAllText(path + @"\Preferences"));
            dynamic goLogin = jsonObj["gologin"];
            JArray jsonObjss = (JArray)goLogin["startup_urls"];
            jsonObjss.RemoveAll();
            jsonObjss.Add("");
            JsonConvert.SerializeObject(jsonObjss, Formatting.Indented);
            goLogin["startupUrl"] = "";

            try
            {

                if (!Directory.Exists(path))
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex.Message);
            }
            File.WriteAllText(path + @"\Preferences", JsonConvert.SerializeObject(jsonObj));
        }
        private void moChrome()
        {


            try
            {
                Process[] processesChrome = Process.GetProcessesByName("chrome");
                if (processesChrome != null)
                {
                    foreach (Process process in processesChrome)
                    {
                        if (process.MainWindowHandle == IntPtr.Zero)
                            continue;
                        else
                        {
                            bool flag = !listchrome.Contains(process.MainWindowHandle);
                            if (flag)
                            {
                                listchrome.Add(process.MainWindowHandle);
                            }

                        }

                    }

                }
            }
            catch { }

            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
                //if ((Convert.ToBoolean(row.Cells[0].Value) == true))
                //{
                    //var path = row.Cells["path"].Value.ToString() + @"\Default";
                    var path =  @"C:\Users\thanh\OneDrive\Desktop\62fc5305e8962841db90aade\1";
                    changePreferen(path+ "\\Default");
                    string port = findFreePort().ToString();
                    openGologin(path, port);
                    Thread.Sleep(1000);
                    Process[] processesChromes = Process.GetProcessesByName("chrome");
                    if (processesChromes != null)
                    {
                        foreach (Process process in processesChromes)
                        {
                            if (process.MainWindowHandle == IntPtr.Zero)
                                continue;
                            else
                            {
                                bool flag = !listchrome.Contains(process.MainWindowHandle);
                                if (flag)
                                {

                                    if (!listchromeNew.Contains(process.MainWindowHandle))
                                    {

                                        listchromeNew.Add(process.MainWindowHandle);

                                    }

                                }

                            }

                        }

                    //}
                //}

            }
         

            System.Drawing.Rectangle resolution = Screen.PrimaryScreen.Bounds;
            screenWidth = resolution.Width;
            windowHeight = resolution.Height;
            RECT Rect = new RECT();
            int dx=0;
            int dy = 0;
            for (int i = 0; i < listchromeNew.Count; i++)
            {
                int windowPerRow = screenWidth / (500 + 5);
                int columnIndex = i % windowPerRow;
                int rowIndex = i / windowPerRow;
                dx = columnIndex * (500 + 5);
                dy = rowIndex * (500 + 5);
                Thread.Sleep(100);
                if (GetWindowRect(listchromeNew[i], ref Rect))
                    MoveWindow(listchromeNew[i], dx, dy, 500, 500, true);
            }
            ImageService img = new ImageService();
            var screen = CaptureHelper.CaptureImage(new Size(500, 500), new Point(dx, dy));
            screen.Save("f.png");
            var point = img.pointImage(screen, BMP_FOLLOW, 5);
            img.FindImage(point, 5);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            moChrome();
        }
    }
}
