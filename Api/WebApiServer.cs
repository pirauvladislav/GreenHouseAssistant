using nanoFramework.Networking;
using System;
using System.Net;
using System.Diagnostics;
using System.Threading;
using nanoFramework.WebServer;
using GreenHouseAssistant.Gpio;

namespace GreenHouseAssistant.Api
{
    public static class WebApiServer
    {
        private static WebServer server;
        public static bool StartWebServer()
        {
            if (server is not null)
                throw new InvalidOperationException("Server was allready started! Please stop it and then try to start it again");

            Debug.WriteLine("Startting WebServer...");
            server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(ApiController) });
            if (server.Start())
            {
                Debug.WriteLine($"Server started on IP Address: {GetCurrentIpAddress()}");
                return true;
            }
            return false;
        }

        public static void StopWebServer()
        {
            server.Stop();
        }

        public static string GetCurrentIpAddress()
        {
            return IPAddress.GetDefaultLocalAddress().ToString();
        }
    }
}

