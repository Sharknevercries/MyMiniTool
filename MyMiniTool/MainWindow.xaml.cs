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
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string NETSH = "netsh.exe";
        private const string WIFI_SHARING_SETTING_CMD = "wlan set hostednetwork mode=allow ssid={0} key={1}";
        private const string WIFI_SHARING_START_CMD = "wlan start hostednetwork";
        private const int WIFI_PASSWORD_LENGTH_MIN = 8;
        private const int WIFI_PASSWORD_LENGTH_MAX = 63;

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool IsWifiSSIDLegal(string ssid)
        {
            return !string.IsNullOrEmpty(ssid);
        }

        private bool IsWifiPasswordLegal(string password)
        {
            return password.Length >= WIFI_PASSWORD_LENGTH_MIN && password.Length <= WIFI_PASSWORD_LENGTH_MAX;
        }

        private void SetWifiSharing(string ssid, string password)
        {
            var settingProc = new System.Diagnostics.Process();
            settingProc.StartInfo.FileName = NETSH;
            settingProc.StartInfo.Arguments = string.Format(WIFI_SHARING_SETTING_CMD, ssid, password);
            settingProc.StartInfo.UseShellExecute = false;
            settingProc.StartInfo.RedirectStandardOutput = true;
            settingProc.StartInfo.CreateNoWindow = true;
            settingProc.Start();
        }

        private void StartWifiSharing()
        {
            var startProc = new System.Diagnostics.Process();
            startProc.StartInfo.FileName = NETSH;
            startProc.StartInfo.Arguments = WIFI_SHARING_START_CMD;
            startProc.StartInfo.UseShellExecute = false;
            startProc.StartInfo.RedirectStandardOutput = true;
            startProc.StartInfo.CreateNoWindow = true;
            startProc.Start();
        }

        private void StartWifiSharingButton_Click(object sender, RoutedEventArgs e)
        {
            string ssid = SSIDTextBox.Text;
            string password = WifiPasswordPasswordBox.Password;

            if (!IsWifiSSIDLegal(ssid))
            {
                StatusMessageTextBlock.Text = "開啟網路分享失敗";
                MessageBox.Show("開啟網路分享失敗。原因：SSID不合法。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!IsWifiPasswordLegal(password))
            {
                StatusMessageTextBlock.Text = "開啟網路分享失敗";
                MessageBox.Show("開啟網路分享失敗。原因：Password不合法。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TaskScheduler context = TaskScheduler.FromCurrentSynchronizationContext();
                
                Task task = Task.Factory.StartNew(() => SetWifiSharing(ssid, password))
                                        .ContinueWith(t => StartWifiSharing())
                                        .ContinueWith(t =>
                                        {
                                            StatusMessageTextBlock.Text = "已成功開啟網路分享";
                                        },
                                        System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, context);
            }
        }
    }
}
