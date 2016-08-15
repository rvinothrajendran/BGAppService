using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace SingleAppService
{
    sealed partial class App
    {
        private AppServiceConnection _appServiceConnection;
        private BackgroundTaskDeferral _bgTaskDeferral;
        private IBackgroundTaskInstance _taskInstance;
        private void RunBackGroundTask(IBackgroundActivatedEventArgs args)
        {
            _taskInstance = args.TaskInstance;
            _taskInstance.Canceled += TaskInstance_Canceled;

            _bgTaskDeferral = _taskInstance.GetDeferral();

            var triggerAppService = _taskInstance.TriggerDetails as AppServiceTriggerDetails;

            if (triggerAppService == null) return;
            _appServiceConnection = triggerAppService.AppServiceConnection;
            _appServiceConnection.RequestReceived += AppServiceRequestReceived;
            _appServiceConnection.ServiceClosed += _appServiceConnection_ServiceClosed;
        }

        private void _appServiceConnection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            _bgTaskDeferral.Complete();
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _bgTaskDeferral.Complete();
        }

        private async void AppServiceRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var serviceMsg = args.GetDeferral();

            var storeData = args.Request.Message;
            string command = storeData["Command"] as string;


            ValueSet returnData = new ValueSet();

            if (command == "get") //Read information from the ApplicationData
            {
                string inventoryIndex = (string)ApplicationData.Current.RoamingSettings.Values["set"];
                returnData.Add("Result", inventoryIndex);
                await args.Request.SendResponseAsync(returnData);
            }

            serviceMsg.Complete();
            
        }
    }
}
