using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
