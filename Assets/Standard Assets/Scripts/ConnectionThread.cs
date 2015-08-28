using System.Collections;
using System.Net.Sockets;
using System;
using System.Text;
using UnityEngine;

namespace CoopWorld
{

	public class ConnectionThread {

        public ConnectionThread(TcpListener tcpListener)
        {
            threadListener = tcpListener;
        }

		private TcpListener threadListener;
		private static int connections = 0;

        private TcpClient client;
		private volatile bool _isRunning = true;

        private static int nextID = 1;
        private int id;

		public void RequestStop()
		{
			_isRunning = false;
            try
            {
                client.GetStream().Close();
                client.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
		}

		public void HandleConnection()
		{
            id = ConnectionThread.nextID++;
            Debug.Log("CONNECTION (" + id + " THREAD: Starting...");
			int recv;
			byte[] data = new byte[1024];
			try
			{
				client = threadListener.AcceptTcpClient();
				NetworkStream ns = client.GetStream();
				connections++;
				Debug.Log ("New client accepted: " + connections + " active connections");

                CoopEvents.Instance.OnPlayerConnect(id);   //Raises Event

				string welcome = "Welcome to my test server";
				data = Encoding.ASCII.GetBytes(welcome);
				ns.Write(data, 0, data.Length);
				
				while(_isRunning)
				{
					data = new byte[1024];
					recv = ns.Read(data, 0, data.Length);
					handleBytes(data);
					if (recv == 0)
						break;
					
					ns.Write(data, 0, recv);
				}
				ns.Close();
				client.Close();
				connections--;
				Debug.Log("Client disconnected: "+connections+" active connections");
			}
			catch(Exception e)
			{
				Debug.LogError(e.ToString());
			}

            Debug.Log("CONNECTION (" + id + " THREAD: Ending...");

		}

		private void handleBytes(byte[] data)
		{
			string str = Encoding.ASCII.GetString (data);
			Debug.Log (str);
			char c = str [0];
			if 		(c=='L')		CoopWorldInput.PressKey (id, CoopWorldInput.CoopKeyCode.Left);
			else if (c=='R')		CoopWorldInput.PressKey (id, CoopWorldInput.CoopKeyCode.Right);
			else if (c=='l')		CoopWorldInput.UnpressKey (id, CoopWorldInput.CoopKeyCode.Left);
			else if (c=='r')		CoopWorldInput.UnpressKey (id, CoopWorldInput.CoopKeyCode.Right);
		}
	}
}