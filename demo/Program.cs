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
            int port_api = 80;
            if (args.Length > 0) port_api = int.Parse(args[0]);

            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            string ip = "127.0.0.1";
            foreach (var x in ipEntry.AddressList)
            {
                if (x.ToString().StartsWith("192.168.") || x.ToString().StartsWith("10.200."))
                    ip = x.ToString();
            }

            string uri = string.Format("ws://{0}:{1}/ios-offline", ip, port_api);
            if (port_api == 80) uri = string.Format("ws://{0}/ios-offline", ip);

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

            //Process.Start("client.html");

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