using UnityEngine; 
using System.Threading; 
using System.Net.Sockets; 
using System.IO;


public class SocketServer : MonoBehaviour { 
	private bool mRunning;

	string msg = "";
	Thread mThread;
	TcpListener tcp_Listener = null;

	void Start()
	{
		mRunning = true;
		ThreadStart ts = new ThreadStart(SayHello);
		mThread = new Thread(ts);
		mThread.Start();
		print("Thread done...");
	}

	public void stopListening()
	{
		mRunning = false;
	}

	void SayHello()
	{
		try
		{
			tcp_Listener = new TcpListener(52432);
			tcp_Listener.Start();
			print("Server Start");
			while (mRunning)
			{
				// check if new connections are pending, if not, be nice and sleep 100ms
				if (!tcp_Listener.Pending())
				{
					Thread.Sleep(100);
				}
				else
				{
					print("1");
					TcpClient client = tcp_Listener.AcceptTcpClient();
					print("2");
					NetworkStream ns = client.GetStream();
					print("3");
					StreamReader reader = new StreamReader(ns);
					print("4");
					msg = reader.ReadLine();
					print(msg);
					reader.Close();
					client.Close();
				}
			}
		}
		catch (ThreadAbortException)
		{
			print("exception");
		}
		finally
		{
			mRunning = false;
			tcp_Listener.Stop();
		}
	}
	void OnApplicationQuit()
	{
		// stop listening thread
		stopListening();
		// wait fpr listening thread to terminate (max. 500ms)
		mThread.Join(500);
	}
}