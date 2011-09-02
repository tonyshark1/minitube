using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MiniTube
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _IsResizing = false;
        private DispatcherTimer _ResizeTimer;
        private OpenDialog _OpenDialog;

        private const int WM_SYSCOMMAND = 0x112;
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public static MainWindow Current
        {
            get
            {
                return (MainWindow)((App)Application.Current).MainWindow;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Browser.Navigated += new NavigatedEventHandler(Browser_Navigated);
            this.Browser.Visibility = Visibility.Collapsed;
            this.TopMostCheckBox.IsChecked = true;
        }

        private void OpenDialogClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.IsEnabled = true;
        }

        public void Open(string YouTubePath)
        {
            if (!string.IsNullOrEmpty(YouTubePath))
            {
                this.Browser.NavigateToString(string.Format("<html><head><title>MiniTube</title></head><body bgcolor=\"#232323\" style=\"margin:0px;padding:0px;overflow:hidden;overflow-x:hidden;overflow-y:hidden;\"><object width=\"100%\" height=\"100%\"><param name=\"movie\" value=\"http://www.youtube.com/v/{0}&autohide=1&version=2&showinfo=0&showsearch=1\"</param><param name=\"allowFullScreen\" value=\"true\"></param><param name=\"allowScriptAccess\" value=\"always\"></param> <embed src=\"http://www.youtube.com/v/{0}&version=2&autohide=1&showinfo=0&showsearch=1\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" allowscriptaccess=\"always\" width=\"100%\" height=\"100%\"></embed></object></body></html>", YouTubePath));
            }
            else
            {
                this.Browser.Visibility = System.Windows.Visibility.Collapsed;
                this.InfoTxt.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void Browser_Navigated(object sender, NavigationEventArgs e)
        {
            this.Browser.Visibility = Visibility.Visible;
            this.InfoTxt.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void CloseButtonClicked(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MinimizeButtonClicked(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void WindowResize(object sender, MouseButtonEventArgs e)
        {
            this._IsResizing = true;
            _ResizeTimer = new DispatcherTimer();
            _ResizeTimer.Tick += new EventHandler(ResizeTimerTick);
            _ResizeTimer.Interval = TimeSpan.FromMilliseconds(10);
            _ResizeTimer.Start();
        }

        private void ResizeTimerTick(object sender, EventArgs e)
        {
            if (this._IsResizing && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var hwndSource = PresentationSource.FromVisual((Visual)this) as System.Windows.Interop.HwndSource;
                SendMessage(hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61448), IntPtr.Zero);
            }
            else
            {
                this._IsResizing = false;
                _ResizeTimer.Stop();
                _ResizeTimer.Tick -= ResizeTimerTick;
            }
        }

        private void OpenBtnClick(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            _OpenDialog = new OpenDialog();
            _OpenDialog.FormClosed += OpenDialogClosed;
            _OpenDialog.Show();
        }

        private void OnTopChecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void OnTopUnchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }
    }
}
