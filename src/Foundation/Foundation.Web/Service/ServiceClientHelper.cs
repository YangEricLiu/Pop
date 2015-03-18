using System.ServiceModel;
using System;

namespace SE.DSP.Foundation.Web
{
    public static class ServiceClientHelper
    {
        public static void Close<TChannel>(ClientBase<TChannel> serviceClient) where TChannel : class
        {
            try
            {
                serviceClient.Close();
            }
            catch
            {
                serviceClient.Abort();
            }
        }

        public static void Using<T>(this T client, Action<T> action) where T : ICommunicationObject
        {
            try
            {
                action(client);

                client.Close();
            }
            catch
            {
                client.Abort();

                throw;
            }
        }
    }
}