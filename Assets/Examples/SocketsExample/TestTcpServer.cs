using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class TestTcpServer : TestTcpPeer
{
	public void Start(ushort port)
	{
		try
		{
			IPAddress ipAddress = new IPAddress(UnityEngine.N3DS.Socket.GetLocalIpAddr());

			listener = new TcpListener(ipAddress, port);
			listener.Start();

			Status = "Listening on addr:" + ipAddress + ", port:" + port;

			for (int i = 0; i < MaxConcurrentUsers; i++)
			{
				Thread t = new Thread(new ThreadStart(ThreadEntryPoint));
				t.Start();
			}
		}
		catch (Exception e)
		{
			Status = "Error: " + e.ToString();
		}
	}

	private void ThreadEntryPoint()
	{
		while (true)
		{
			Socket socket = listener.AcceptSocket();
			Status = "Connected: " + socket.RemoteEndPoint;

			try
			{
				Stream stream = new NetworkStream(socket);
				StreamReader reader = new StreamReader(stream);
				StreamWriter writer = new StreamWriter(stream);
				writer.AutoFlush = true;
				writer.WriteLine("Hello from server!");
				while (true)
				{
					Status = reader.ReadLine();
					writer.WriteLine("Hello from server!");
					Thread.Sleep(500);
				}
			}
			catch (Exception e)
			{
				Status = "Error: " + e.ToString();
			}

			Status = "Disconnected: " + socket.RemoteEndPoint;
			socket.Close();
		}
	}

	private TcpListener listener;
	private const int MaxConcurrentUsers = 5;
}