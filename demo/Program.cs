using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Fleck2.Interfaces;

namespace Fleck2.Demo
{
    class Server
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port_api = 80;
			string pathway = "";

			Console.WriteLine(
				"Usage:\n"+
				"\t>Fleck2.Demo.exe - try to found IP and bind there. Default port 80. \n"+
				"\t>Fleck2.Demo.exe \"PORT\" - try to found IP and listen specified port\n"+
				"\t>Fleck2.Demo.exe \"IP.IP.IP.IP\" \"PORT\" - bind to specified IP and listen specified port.\n"				
			);
			
            if(args.Length == 0){
				Console.WriteLine("Try to find external IP, and bind there...");
				string strHostName = Dns.GetHostName();
				Console.WriteLine("hostname: "+strHostName);
				IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
				foreach (var x in ipEntry.AddressList)
				{
					Console.WriteLine("Found Entry IP: "+x.ToString());
					if (x.ToString().StartsWith("192.168.") || x.ToString().StartsWith("10.200.")){
						Console.WriteLine("Used local IP: "+x.ToString());
						ip = x.ToString();
					}
				}
			}else if (args.Length == 1){
				port_api = int.Parse(args[0]);
			}
			else if(args.Length > 1){
				ip = args[0];
				port_api = int.Parse(args[1]);
				if(args.Length > 2 && args.Length == 3){
					pathway = args[2];
				}else{
					pathway = args[2];
					Console.WriteLine("Warning: Too many arguments specified!");
				}
			}
			
			Console.WriteLine("IP: "+ip+" port: "+port_api);
            string uri = string.Format("ws://{0}:{1}/"+pathway, ip, port_api);
            if (port_api == 80) uri = string.Format("ws://{0}/"+pathway, ip);

            Console.Title = uri;

            FleckLog.Level = LogLevel.Debug;
            var allSockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer(uri);
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                };
            });

            Process.Start("client.html");

            var input = Console.ReadLine();
            while (input != "exit")
            {
                foreach (var socket in allSockets.ToList())
                {
                    socket.Send(input);
                }
                input = Console.ReadLine();
            }

        }
    }
}