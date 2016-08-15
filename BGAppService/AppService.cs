using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace BGAppService
{
    public sealed class UwpAppService : IBackgroundTask
    {
        private BackgroundTaskDeferral _bgTaskDeferral;
        private AppServiceConnection _appServiceConnection;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _bgTaskDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += TaskInstanceCancel;
            var triggerAppService = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            if (triggerAppService == null) return;
            _appServiceConnection = triggerAppService.AppServiceConnection;
            _appServiceConnection.RequestReceived += AppServiceRequestReceived;
        }

        private void TaskInstanceCancel(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _bgTaskDeferral?.Complete();
        }

        private async void AppServiceRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var serviceMsg = args.GetDeferral();

            var storeData = args.Request.Message;
            string command = storeData["Command"] as string;
            

            ValueSet returnData = new ValueSet();

            if (command == "set") //Update information in the ApplicationData
            {
                string inventoryIndex = storeData["serverMsg"] as string;
                ApplicationData.Current.RoamingSettings.Values["set"] = inventoryIndex;
                returnData.Add("Result", "OK");
                await args.Request.SendResponseAsync(returnData);
            }
            else if (command == "get") //Read information from the ApplicationData
            {
                string inventoryIndex = (string) ApplicationData.Current.RoamingSettings.Values["set"];
                returnData.Add("Result", inventoryIndex);
                await args.Request.SendResponseAsync(returnData);
            }

            serviceMsg.Complete();
        }
    }
}
