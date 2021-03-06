/*
 * Copyright (c) 2000, 2020, Oracle and/or its affiliates.
 *
 * Licensed under the Universal Permissive License v 1.0 as shown at
 * http://oss.oracle.com/licenses/upl.
 */
﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using Tangosol.Run.Xml;

namespace Tangosol.Net.Ssl
{
    [TestFixture]
    public class SslTest 
    {
        private const string serverCert = @".\Net\Ssl\Server.cer";
        private const string clientCert = @".\Net\Ssl\Client.pfx";
        private const string clientCertPassword = @"password";

        private SslServer server;

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }


        [Test]
        public void TestSslServerAuthentication()
        {
            var location = new IPEndPoint(IPAddress.Loopback, 5055);
            Console.WriteLine(Directory.GetCurrentDirectory());
            server = new SslServer(location)
            {
                ServerCertificate =
                        SslServer.LoadCertificate(serverCert)
            };
            server.Start();

            try
            {
                SslClient client =
                 new SslClient(location)
                 {
                     ServerName = "MyServerName",
                     Protocol = SslProtocols.Default
                 };
                client.Connect();

                string echo = client.Echo("Hello World");
                Assert.AreEqual(echo, "Hello World");
            }
            finally
            {
                server.Stop();  
            }
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void TestSslClientAuthenticationException()
        {
            var location = new IPEndPoint(IPAddress.Loopback, 5055);
            server = new SslServer(location)
                         {
                                 ServerCertificate =
                                         SslServer.LoadCertificate(
                                         serverCert),
                                 AuthenticateClient = true
            };
            server.Start();
            try
            {
                SslClient client =
                        new SslClient(location)
                            {
                                ServerName = "MyServerName",
                                Protocol = SslProtocols.Default
                            };
                client.Connect();

                string echo = client.Echo("Hello World");
                Assert.AreEqual(echo, "Hello World");
            }
            finally
            {
                server.Stop();
            }
        }

        [Test]
        public void TestSslClientAuthentication()
        {
            var location = new IPEndPoint(IPAddress.Loopback, 5055);
            server = new SslServer(location)
            {
                ServerCertificate =
                        SslServer.LoadCertificate(
                        serverCert),
                AuthenticateClient = true
            };
            server.Start();

            try
            {
                SslClient client =
                        new SslClient(location)
                            {
                                ServerName = "MyServerName",
                                Protocol = SslProtocols.Default
                            };
                client.AppendCertificate( new X509Certificate(clientCert, clientCertPassword));
                client.Connect();

                string echo = client.Echo("Hello World");
                Assert.AreEqual(echo, "Hello World");
            }
            finally
            {
                server.Stop();
            }
        }

        [Test]
        public void TestSslClientConfiguration()
        {
            var location = new IPEndPoint(IPAddress.Loopback, 5055);
            server = new SslServer(location)
            {
                ServerCertificate =
                        SslServer.LoadCertificate(
                        serverCert),
                AuthenticateClient = true
            };
            server.Start();
            TcpClient client = new TcpClient();
            try
            {
                IXmlDocument xmlDoc = XmlHelper.LoadXml("./Net/Ssl/Configs/config.xml");
                
                IStreamProvider streamProvider = StreamProviderFactory.CreateProvider(xmlDoc);

                client.Connect(location);
                Stream stream = streamProvider.GetStream(client);
                
                string echo = SslClient.Echo(stream, "Hello World");
                Assert.AreEqual(echo, "Hello World");
            }
            finally
            {
                client.Close();
                server.Stop();
            }
        }

        [Test]
        [ExpectedException(typeof(TypeLoadException))]
        public void TestSslClientConfiguration2()
        {
            var location = new IPEndPoint(IPAddress.Loopback, 5055);
            server = new SslServer(location)
            {
                ServerCertificate =
                        SslServer.LoadCertificate(
                        serverCert),
                AuthenticateClient = true
            };
            server.Start();
            TcpClient client = new TcpClient();
            try
            {
                IXmlDocument xmlDoc = XmlHelper.LoadXml("./Net/Ssl/Configs/config2.xml");

                IStreamProvider streamProvider = StreamProviderFactory.CreateProvider(xmlDoc);

                client.Connect(location);
                Stream stream = streamProvider.GetStream(client);

                string echo = SslClient.Echo(stream, "Hello World");
                Assert.AreEqual(echo, "Hello World");
            }
            finally
            {
                client.Close();
                server.Stop();
            }
        }

        [Test]
        public void TestSslClientConfiguration3()
        {
            var location = new IPEndPoint(IPAddress.Loopback, 5055);
            server = new SslServer(location)
            {
                ServerCertificate =
                        SslServer.LoadCertificate(
                        serverCert),
                AuthenticateClient = true
            };
            server.Start();
            TcpClient client = new TcpClient();
            try
            {
                IXmlDocument xmlDoc = XmlHelper.LoadXml("./Net/Ssl/Configs/config3.xml");

                IStreamProvider streamProvider = StreamProviderFactory.CreateProvider(xmlDoc);

                client.Connect(location);
                Stream stream = streamProvider.GetStream(client);

                string echo = SslClient.Echo(stream, "Hello World");
                Assert.AreEqual(echo, "Hello World");
            }
            finally
            {
                client.Close();
                server.Stop();
            }
        }

        [Test]
        [ExpectedException(typeof(AuthenticationException))]
        public void TestSslClientConfiguration4()
        {
            var location = new IPEndPoint(IPAddress.Loopback, 5055);
            server = new SslServer(location)
            {
                ServerCertificate =
                        SslServer.LoadCertificate(
                        serverCert),
                AuthenticateClient = true
            };
            server.Start();
            TcpClient client = new TcpClient();
            try
            {
                IXmlDocument xmlDoc = XmlHelper.LoadXml("./Net/Ssl/Configs/config4.xml");

                IStreamProvider streamProvider = StreamProviderFactory.CreateProvider(xmlDoc);

                client.Connect(location);
                Stream stream = streamProvider.GetStream(client);

                string echo = SslClient.Echo(stream, "Hello World");
                Assert.AreEqual(echo, "Hello World");
            }
            finally
            {
                client.Close();
                server.Stop();
            }
        }

        [Test]
        [ExpectedException(typeof(CryptographicException))]
        public void TestSslClientConfiguration5()
        {
            var location = new IPEndPoint(IPAddress.Loopback, 5055);
            server = new SslServer(location)
            {
                ServerCertificate =
                        SslServer.LoadCertificate(
                        serverCert),
                AuthenticateClient = true
            };
            server.Start();
            TcpClient client = new TcpClient();
            try
            {
                IXmlDocument xmlDoc = XmlHelper.LoadXml("./Net/Ssl/Configs/config5.xml");

                IStreamProvider streamProvider = StreamProviderFactory.CreateProvider(xmlDoc);

                client.Connect(location);
                Stream stream = streamProvider.GetStream(client);

                string echo = SslClient.Echo(stream, "Hello World");
                Assert.AreEqual(echo, "Hello World");
            }
            finally
            {
                client.Close();
                server.Stop();
            }
        }

    }
}
