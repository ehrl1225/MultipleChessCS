using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.SignalR;

namespace Server
{
    public class Server
    {
        public int port{ get; private set; }
        private bool is_run{ get; set; }

        public Server(int port)
        {
            this.port = port;
            
        }

        public async void Run()
        {
            if (is_run) return;
            is_run = false;
        }

        private void HandleClient(TcpClient client)
        {
            
        }
        
    }
    
}