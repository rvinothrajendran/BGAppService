using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ServiceManager;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppServiceClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string AppServiceName = "com.microsoft.BgAppService";
        private const string PackageFamilyName = "69533c06-5ce5-4f1a-93fe-151ca737537a_kz0gnaa3h8516";
        public MainPage()
        {
            this.InitializeComponent();
            OpenService();
        }

        private async void OpenService()
        {
            await AppServiceManager.OpenAsync(AppServiceName, PackageFamilyName);
        }

        public async void CreateService(string sendMsg)
        {
            var textMsg = await AppServiceManager.SendMessageAsync("set", sendMsg);
            TxtSendStatus.Text = textMsg;
        }

        private void TxtStatus_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter) return;
            TxtSendStatus.Text = "Send Msg";
            var sendMsg = TxtStatus.Text;
            CreateService(sendMsg);
        }
    }
}
