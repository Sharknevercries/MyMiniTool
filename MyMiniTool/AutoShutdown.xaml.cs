using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMiniTool
{
    /// <summary>
    /// AutoShutdown.xaml 的互動邏輯
    /// </summary>
    public partial class AutoShutdown : UserControl
    {
        public event EventHandler SetStatusMessage;

        private const string SHUTDOWN = "shutdown.exe";
        private const string SHUTDOWN_SET_CMD = "/s /t {0}";
        private const string SHUTDOWN_ABORT_CMD = "/a";
        private const int WIFI_PASSWORD_LENGTH_MIN = 8;
        private const int WIFI_PASSWORD_LENGTH_MAX = 63;

        public AutoShutdown()
        {
            InitializeComponent();
        }

        private void SetShutdown_Click(object sender, RoutedEventArgs e)
        {
            var setProc = new System.Diagnostics.Process();
            setProc.StartInfo.FileName = SHUTDOWN;
            setProc.StartInfo.Arguments = string.Format(SHUTDOWN_SET_CMD, int.Parse(ShutdownTimeComboBox.Text));
            setProc.StartInfo.UseShellExecute = false;
            setProc.StartInfo.RedirectStandardOutput = true;
            setProc.StartInfo.CreateNoWindow = true;
            setProc.Start();
            SetStatusMessageText(string.Format("電腦將於{0}秒後關機", int.Parse(ShutdownTimeComboBox.Text)));
        }

        private void CancelShutdown_Click(object sender, RoutedEventArgs e)
        {
            var setProc = new System.Diagnostics.Process();
            setProc.StartInfo.FileName = SHUTDOWN;
            setProc.StartInfo.Arguments = SHUTDOWN_ABORT_CMD;
            setProc.StartInfo.UseShellExecute = false;
            setProc.StartInfo.RedirectStandardOutput = true;
            setProc.StartInfo.CreateNoWindow = true;
            setProc.Start();
            SetStatusMessageText("已取消電腦關機");
        }        

        private void SetStatusMessageText(string text)
        {
            SetStatusMessage?.Invoke(this, new MainWindow.StatusMessageEventArgs(text));
        }        
    }
}
