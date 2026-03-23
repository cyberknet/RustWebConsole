using System.Net.Sockets;
using System.Threading.Tasks;
using RustRcon;

namespace RustWebConsole.Web.Services.Validation
{
    public class ServerConnectionValidator
    {
        public async Task<bool> ValidateConnectionAsync(string hostname, int port, string password)
        {
            try
            {
                var rconClient = new RustRconClient("ValidationClient", hostname, port, password);

                if (rconClient.Connect() && rconClient.IsConnected)
                {
                    rconClient.Disconnect();
                    return true;
                }
            }
            catch
            {
                // Log exception if necessary
            }

            return false;
        }
    }
}