using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fleck2.Interfaces;

namespace Fleck2.Demo
{
    class Server
    {
        static void Main()
        {
            FleckLog.Level = LogLevel.Debug;
            var allSockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer("ws://localhost:8181");
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };
				
				//This is triggering twise and calling double closing, when tab is closed in browser. How to fix this?
                socket.OnClose = () =>
                {
                    Console.Write("Try to close... ");
                    try{
						allSockets.Remove(socket);
						Console.Write(" Successfuly Closed!\n");
					}catch{
						Console.WriteLine(" Was been already closed!\n");
					}
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