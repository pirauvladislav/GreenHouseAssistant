using System;
using System.Text;

namespace GreenHouseAssistant.ApiServer
{
    public class WebServerConnectionErrorEventArgs : EventArgs
    {
        public WebServerConnectionErrorEventArgs(string connectionError, Exception exception)
        {
            ConnectionError = connectionError;
            Exception = exception;
        }
        public string ConnectionError { get; set; }
        public Exception Exception { get; set; }
    }
}

