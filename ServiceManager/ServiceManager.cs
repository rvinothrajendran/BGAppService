using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace ServiceManager
{
    public static class AppServiceManager
    {
        private static AppServiceConnection _appService;
        private static AppServiceConnectionStatus _appStatus;

        public static async Task<bool> OpenAsync(string serviceName, string packageName)
        {
            try
            {
                _appService = new AppServiceConnection
                {
                    AppServiceName = serviceName,
                    PackageFamilyName = packageName
                };

                _appStatus = await _appService.OpenAsync();
            }
            catch (Exception exception)
            {

            }
            

            return _appStatus == AppServiceConnectionStatus.Success;
        }

        public static async Task<string> SendMessageAsync(string command,string message)
        {
            if(_appStatus != AppServiceConnectionStatus.Success)
                return null;

            var responseUpdate = new ValueSet();
            responseUpdate.Add("Command", command);
            responseUpdate.Add("serverMsg", message);

            var response = await _appService.SendMessageAsync(responseUpdate);
            var result = response.Message["Result"] as object;
            return (string)result;
        }

    }
}
