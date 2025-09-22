using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class TestTcpClient : TestTcpPeer
{
	public void Start(IPAddress ipAddress, ushort port)
	{
		tcpClient = new TcpClient();
		tcpClient.Connect(new IPEndPoint(ipAddress, port));

		Thread t = new Thread(new ThreadStart(ThreadEntryPoint));
		t.Start();
	}

	private void ThreadEntryPoint()
	{
		try
		{
			Stream stream = tcpClient.GetStream();
			StreamReader reader = new StreamReader(stream);
			StreamWriter writer = new StreamWriter(stream);
			writer.AutoFlush = true;
			Status = reader.ReadLine();
			while (true)
			{
				writer.WriteLine("Hello from client!");
				Status = reader.ReadLine();
			}
		}
		catch (Exception e)
		{
			Status = "Error: " + e.ToString();
		}
	}

	private TcpClient tcpClient;
}