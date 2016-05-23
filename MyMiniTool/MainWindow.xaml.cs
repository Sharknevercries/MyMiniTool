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
        public MainWindow()
        {
            InitializeComponent();

            WifiSharing.SetStatusMessage += SetStatusTextBlock;
            AutoShutdown.SetStatusMessage += SetStatusTextBlock;
        }

        private void SetStatusTextBlock(object sender, EventArgs e)
        {
            StatusMessageTextBlock.Text = (e as StatusMessageEventArgs).Message;
        }

        public class StatusMessageEventArgs : EventArgs
        {
            public string Message;

            public StatusMessageEventArgs(string message)
            {
                this.Message = message;
            }
        }

    }
}
