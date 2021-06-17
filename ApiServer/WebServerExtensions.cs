using nanoFramework.WebServer;
using System;
using System.Net;
using System.Text;

namespace GreenHouseAssistant.ApiServer
{
    static class WebServerExtensions
    {
        public static void SendOkResponse(this WebServerEventArgs e, string response)
        {
            e.Context.Response.ContentType = "text/html";
            e.Context.Response.ContentLength64 = response.Length;
            WebServer.OutPutStream(e.Context.Response, response);
        }

        public static void SendBadResponse(this WebServerEventArgs e)
        {
            WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
        }
    }
}
