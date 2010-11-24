﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SuperWebSocket;
using SuperSocket.SocketBase.Config;
using System.Net.Sockets;
using System.IO;
using System.Net;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using SuperSocket.Common;

namespace SuperWebSocketTest
{
    [TestFixture]
    public class WebSocketTest
    {
        private WebSocketServer m_WebSocketServer;

        [TestFixtureSetUp]
        public void Setup()
        {
            LogUtil.Setup(new ConsoleLogger());

            m_WebSocketServer = new WebSocketServer();
            m_WebSocketServer.Setup(new ServerConfig
                {
                    Port = 911,
                    Ip = "Any",
                    MaxConnectionNumber = 100,
                    Mode = SocketMode.Async,
                    Name = "SuperWebSocket Server"
                }, SocketServerFactory.Instance);

            m_WebSocketServer.CommandHandler += new CommandHandler<WebSocketSession, WebSocketCommandInfo>(m_WebSocketServer_CommandHandler);
            m_WebSocketServer.NewSessionConnected += new SessionEventHandler(m_WebSocketServer_NewSessionConnected);
            m_WebSocketServer.SessionClosed += new SessionEventHandler(m_WebSocketServer_SessionClosed);
        }

        void m_WebSocketServer_SessionClosed(WebSocketSession session)
        {

        }

        void m_WebSocketServer_NewSessionConnected(WebSocketSession session)
        {

        }

        void m_WebSocketServer_CommandHandler(WebSocketSession session, WebSocketCommandInfo commandInfo)
        {

        }

        [SetUp]
        public void StartServer()
        {
            m_WebSocketServer.Start();
        }

        [TearDown]
        public void StopServer()
        {
            m_WebSocketServer.Stop();
        }

        [Test]
        public void HeaderTest()
        {
            var ip = "127.0.0.1";
            var port = 911;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var address = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.Connect(address);

                var stream = new NetworkStream(socket);

                using(var reader = new StreamReader(stream, Encoding.UTF8, false))
                using(var writer = new StreamWriter(stream, Encoding.UTF8, 1024 * 10))
                {
                    writer.WriteLine("GET /websock HTTP/1.1");
                    writer.WriteLine("Upgrade: WebSocket");
                    writer.WriteLine("Connection: Upgrade");
                    writer.WriteLine("Sec-WebSocket-Key2: 12998 5 Y3 1  .P00");
                    writer.WriteLine("Host: example.com");
                    writer.WriteLine("Sec-WebSocket-Key1: 4 @1  46546xW%0l 1 5");
                    writer.WriteLine("Origin: http://example.com");
                    writer.WriteLine("WebSocket-Protocol: sample");
                    writer.WriteLine("");
                    writer.Write("^n:ds[4U");
                    writer.Flush();

                    for (var i = 0; i < 6; i++)
                        Console.WriteLine(reader.ReadLine());

                    char[] buffer = new char[20];

                    int read = reader.Read(buffer, 0, buffer.Length);

                    Assert.AreEqual("8jKS'y:G*Co,Wxa-", new string(buffer.Take(read).ToArray()));                    
                }
            }
        }
    }
}
