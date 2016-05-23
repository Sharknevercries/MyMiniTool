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
    /// WifiSharing.xaml 的互動邏輯
    /// </summary>
    public partial class WifiSharing : UserControl
    {
        public event EventHandler SetStatusMessage;

        private const string NETSH = "netsh.exe";
        private const string WIFI_SHARING_SETTING_CMD = "wlan set hostednetwork mode=allow ssid={0} key={1}";
        private const string WIFI_SHARING_START_CMD = "wlan start hostednetwork";
        private const string WIFI_SHARING_STOP_CMD = "wlan stop hostednetwork";
        private const int WIFI_PASSWORD_LENGTH_MIN = 8;
        private const int WIFI_PASSWORD_LENGTH_MAX = 63;

        public WifiSharing()
        {
            InitializeComponent();

            LoadConfig();
        }

        private void LoadConfig()
        {
            WifiSSIDTextBox.Text = Properties.Settings.Default.WifiSSID;
            WifiPasswordPasswordBox.Password = Properties.Settings.Default.WifiPassword;
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
            string ssid = WifiSSIDTextBox.Text;
            string password = WifiPasswordPasswordBox.Password;

            if (!IsWifiSSIDLegal(ssid))
            {
                SetStatusMessageText("開啟網路分享失敗");
                MessageBox.Show("開啟網路分享失敗。原因：SSID不合法。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!IsWifiPasswordLegal(password))
            {
                SetStatusMessageText("開啟網路分享失敗");
                MessageBox.Show("開啟網路分享失敗。原因：Password不合法。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                SetWifiSharing(ssid, password);
                StartWifiSharing();
                SetStatusMessageText("已成功開啟網路分享");
            }
        }

        private void StopWifiSharingButton_Click(object sender, RoutedEventArgs e)
        {
            var stopProc = new System.Diagnostics.Process();
            stopProc.StartInfo.FileName = NETSH;
            stopProc.StartInfo.Arguments = WIFI_SHARING_STOP_CMD;
            stopProc.StartInfo.UseShellExecute = false;
            stopProc.StartInfo.RedirectStandardOutput = true;
            stopProc.StartInfo.CreateNoWindow = true;
            stopProc.Start();
            SetStatusMessageText("已成功關閉網路分享");
        }

        private void SaveWifiSharingConfigButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WifiSSID = WifiSSIDTextBox.Text;
            Properties.Settings.Default.WifiPassword = WifiPasswordPasswordBox.Password;
            Properties.Settings.Default.Save();
            SetStatusMessageText("已成功儲存分享設定");
        }

        private void SetStatusMessageText(string text)
        {
            SetStatusMessage?.Invoke(this, new MainWindow.StatusMessageEventArgs(text));
        }

        
    }
}
