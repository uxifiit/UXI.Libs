using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UXI.SystemApi;
using UXI.SystemApi.Screen;

namespace UXI.SystemApi.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ScreenParametersProvider _provider = new ScreenParametersProvider();
        private Job _job;

        public MainWindow()
        {
            InitializeComponent();
            _job = new Job();
            _job.AddProcess(Process.GetCurrentProcess().Id);

        }

        private void RefreshButton_Click(object sender, RoutedEventArgs args)
        {
            ScreenInfo screenInfo;
            if (_provider.TryGetScreenInfo((int)this.Left, (int)this.Top, out screenInfo))
            {
                PositionTextBox.Text = $"{screenInfo.Left} , {screenInfo.Top}";
                ResolutionTextBox.Text = $"{screenInfo.Width} x {screenInfo.Height}";
            }

            uint dpiX;
            uint dpiY;
            _provider.GetScreenDpi(screenInfo, ScreenInterop.DpiType.Effective, out dpiX, out dpiY);

            DPITextBox.Text = $"{dpiX} x {dpiY}";
        }


        private void SpawnProcessButton_Click(object sender, RoutedEventArgs args)
        {
            var process = Process.Start("notepad.exe");
          //  _job.AddProcess(process.Id);
        }
    }
}
