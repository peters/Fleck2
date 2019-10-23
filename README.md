Fleck2
=====

Fleck is a WebSocket server implementation in C# with NET 2.0 support.

This project is a fork of [Fleck](http://github.com/statianzo/Fleck.git) branched from the
[Nugget][nugget] project. Fleck requires no inheritance, container, or additional references.

Install via NuGet
----------
    Install-Package Fleck2

Building
--------
```
1. "Clone or Download" -> "Download zip" -> Unzip it.
2. Run build.bat from the commandline and release dll will be
created for NET 2.0, 3.5, 4.0 and 4.5.
```
```
To do building just demo and test this:
1. Go to "demo/" and start "Compile.bat", or start "/build.bat".
2. Wait compilation. Compiled files will be saved in "/demo/bin/Release", and Fleck2.dll there is exists too.
3. Run "Fleck2.Demo.exe" from the folder "\demo\bin\Release\".
4. Type any messages in opened "client.html", and see this in the server console.
```

Requirements
------------
NET Framework 2.0 or newer.

Example
---

The following is an example that will echo to a client.

```c#

var server = new WebSocketServer("ws://localhost:8181");
server.Start(socket =>
  {
    socket.OnOpen = () => Console.WriteLine("Open!");
    socket.OnClose = () => Console.WriteLine("Close!");
    socket.OnMessage = message => socket.Send(message);
  });
        
```

Supported WebSocket Versions
---

Fleck supports several WebSocket versions of modern web browsers

- Hixie-Draft-76/Hybi-00 (Safari 5, Chrome < 14, Firefox 4 (when enabled))
- Hybi-07 (Firefox 6)
- Hybi-10 (Chrome 14-16, Firefox 7)
- Hybi-13 (Chrome 17+)

Secure WebSockets (wss://)
---

Enabling secure connections requires two things: using the scheme `wss` instead
of `ws`, and pointing Fleck to an x509 certificate containing a public and
private key

```cs
var server = new WebSocketServer("wss://localhost:8431");
server.Certificate = new X509Certificate2("MyCert.pfx");
server.Start(socket =>
  {
    //...use as normal
  });
```

License
---

The MIT License

Copyright (c) 2013 Peter Sunde

Copyright (c) 2010-2012 Jason Staten

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.



[nugget]: http://nugget.codeplex.com/ 
