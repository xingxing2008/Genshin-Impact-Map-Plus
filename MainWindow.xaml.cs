using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NHotkey;
using NHotkey.Wpf;

namespace Genshin_Map
{
    
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public bool minibutton = true;

        [DllImport("user32.dll", EntryPoint = "keybd_event", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern void Keybd_event(byte vk, byte scan, int flags, int extrainfo);
        const int KEYEVENTF_EXTENDEDKEY = 0x1;
        const int KEYEVENTF_KEYUP = 0x2;

        [DllImport("user32.dll")]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        /// <summary>
        /// 得到当前活动的窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern System.IntPtr GetForegroundWindow();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "data.txt")) { 
                mainwebbrowser.Source=new Uri(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "data.txt"));
            }
            mini.Visibility = Visibility.Collapsed;
            MainPage.Visibility = Visibility.Visible;
            HotkeyManager.Current.AddOrReplace("visiblemap", Key.M, ModifierKeys.Control,setmap);
            HotkeyManager.Current.AddOrReplace("collapsedminibutton", Key.L, ModifierKeys.Control, collapsedminibutton);
            HotkeyManager.Current.AddOrReplace("e3", Key.D3, ModifierKeys.Alt,e3);
        }

        private void e3(object sender, HotkeyEventArgs e)
        {
            Task.Run(() => {
                Keybd_event((byte)12, 50, KEYEVENTF_EXTENDEDKEY, 0);
                Thread.Sleep(200);
                Keybd_event((byte)12, 69, KEYEVENTF_EXTENDEDKEY, 0);
                //Thread.Sleep(new Random().Next(500,1000));
                Keybd_event((byte)12, 69, KEYEVENTF_KEYUP, 0);
            });

        }
        private void setmap(object sender, HotkeyEventArgs e)
        {
            if (MainPage.Visibility == Visibility.Collapsed) 
            {
                SetWindowPos(new WindowInteropHelper(this).Handle, -1, 0, 0, 0, 0, 1 | 2);
                WindowState = WindowState.Normal;
                mini.Visibility = Visibility.Collapsed;
                MainPage.Visibility = Visibility.Visible;
            }
            else
            {
                MainPage.Visibility = Visibility.Collapsed;
                if(minibutton)  mini.Visibility = Visibility.Visible;
            }
        }

        private void collapsedminibutton(object sender,HotkeyEventArgs e)
        {
            mini.Visibility = Visibility.Collapsed;
            minibutton = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Visibility = Visibility.Collapsed;
            mini.Visibility = Visibility.Visible;
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void mainwebbrowser_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
        {
            if (mainwebbrowser.Source.ToString().Length >= 58)
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "data.txt"))
                {
                    FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "data.txt", FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(mainwebbrowser.Source.ToString());
                    sw.Flush();
                    sw.Close();
                }
            }
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SetWindowPos(new WindowInteropHelper(this).Handle, -1, 0, 0, 0, 0, 1 | 2);
            WindowState = WindowState.Normal;
            mini.Visibility = Visibility.Collapsed;
            MainPage.Visibility = Visibility.Visible;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        { 
           DragMove();
        }
    }
}
