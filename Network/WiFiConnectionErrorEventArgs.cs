using System;
using System.Text;

namespace GreenHouseAssistant.Network
{
    public class WiFiConnectionErrorEventArgs : EventArgs
    {
        public WiFiConnectionErrorEventArgs(string connectionError, Exception exception)
        {
            ConnectionError = connectionError;
            Exception = exception;
        }
        public string ConnectionError { get; set; }
        public Exception Exception { get; set; }
    }
}

