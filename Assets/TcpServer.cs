using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System;
using System.Threading;


public class TcpServer : MonoBehaviour {

	public Thread mThread;
	public TcpListener server;
	public TcpClient client;
	public NetworkStream stream;
	string toSend;
	public String data;

	void Start(){
		ThreadStart ts = new ThreadStart(Update1);
		mThread = new Thread(ts);
		mThread.Start();
	}

	void OnApplicationQuit(){
		server.Stop();
		mThread.Abort ();
	}

	void Update1()
	{  
		print ("Starting server in a thread");
		server=null;  
		try
		{
			// Set the TcpListener on port 13000.
			Int32 port = 3333;
			IPAddress localAddr = IPAddress.Parse("192.168.0.242");

			// TcpListener server = new TcpListener(port);
			server = new TcpListener(IPAddress.Any, port);

			// Start listening for client requests.
			server.Start();

			// Buffer for reading data
			Byte[] bytes = new Byte[256];
			String data = null;

			// Enter the listening loop.
			while(true)
			{
				Thread.Sleep(10);

				Debug.Log("Waiting for a connection... ");

				// Perform a blocking call to accept requests.
				// You could also user server.AcceptSocket() here.
				client = server.AcceptTcpClient();  
				if(client!=null){

					Debug.Log("Connected!");
					//isConnection=true;
					//client.Close();
					//break;

				}
				data = null;

				// Get a stream object for reading and writing
				stream = client.GetStream();
				StreamWriter swriter=new StreamWriter(stream);
				int i;

				// Loop to receive all the data sent by the client.
				while((i = stream.Read(bytes, 0, bytes.Length))!=0)
				{  
					data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
					Debug.Log("Received:"+ data+"data");
				}
			}
		}
		catch(SocketException e)
		{
			Debug.Log("SocketException:"+ e);
		}
		finally
		{
			// Stop listening for new clients.
			server.Stop();
		}
	}
}
