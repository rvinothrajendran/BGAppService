using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ServiceManager;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppServiceUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private const string AppServiceName = "com.microsoft.BgAppService";
        //private const string PackageFamilyName = "69533c06-5ce5-4f1a-93fe-151ca737537a_kz0gnaa3h8516";

        private const string AppServiceName = "SingleAppService";
        private const string PackageFamilyName = "b6512b79-4bc1-4b5a-a8d7-86c0c74aa169_kz0gnaa3h8516";

        public MainPage()
        {
            this.InitializeComponent();
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            dispatcherTimer.Start();
        }

        private async void DispatcherTimer_Tick(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, RequestService);
        }

        
        public async void RequestService()
        {
            try
            {
                await AppServiceManager.OpenAsync(AppServiceName, PackageFamilyName);

                RecvBlock.Text = await AppServiceManager.SendMessageAsync("get", "");
            }
            catch (Exception exception)
            {

            }
        }

       
    }
}
