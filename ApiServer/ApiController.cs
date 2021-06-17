using nanoFramework.WebServer;
using System;
using System.Net;
using System.Text;

namespace GreenHouseAssistant.ApiServer
{
    class ApiController
    {
        [Route("configuration")]
        [Method("GET")]
        public void Open(WebServerEventArgs e)
        {
            try
            {
                var rawUrl = e.Context.Request.RawUrl.Trim('/');
                var args = rawUrl.Split('/');
                if (args.Length != 1)
                {
                    e.SendBadResponse();
                    return;
                }

                var response = "Hello" + DateTime.UtcNow;
                e.SendOkResponse(response);

            }
            catch (Exception)
            {
                e.SendBadResponse();
            }
        }

    }
}
