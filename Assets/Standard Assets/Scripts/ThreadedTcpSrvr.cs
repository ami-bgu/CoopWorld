using System.Collections;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Net;
using UnityEngine;
using System.Collections.Generic;

namespace CoopWorld
{

	public class ThreadedTcpSrvr {

		private TcpListener tcpListener;

		private volatile bool _isRunning = true;

		public void RequestStop()
		{
			_isRunning = false;
            try
            {
                tcpListener.Stop();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
		}

		List<ConnectionThread> runningConnections = new List<ConnectionThread>();

		public void run()
		{
            Debug.Log("ACCEPTOR THREAD: Starting Tcp Listener");
			try
			{
				tcpListener = new TcpListener(IPAddress.Any, 4444);
				tcpListener.Start ();
				//new Thread (new ThreadStart (DiscoveryThread.Instance.run)).Start();
                Debug.Log("ACCEPTOR THREAD: Waiting for clients...");
				while (_isRunning) {
					while (!tcpListener.Pending()) {
						Thread.Sleep (1000);
					}
                    Debug.Log("ACCEPTOR THREAD: Client accepted!");
                    ConnectionThread newconnection = new ConnectionThread(tcpListener);
					runningConnections.Add(newconnection);
					new Thread (new ThreadStart (newconnection.HandleConnection)).Start();
                    Thread.Sleep(100);
				}
			}
			catch (Exception e)
			{
				Debug.LogError (e.ToString());
			}
            Debug.Log("ACCEPTOR THREAD: TCP Requesting Stop from " + runningConnections.Count + " open connections");
			foreach (ConnectionThread connection in runningConnections) {
                connection.RequestStop();
			}
            Debug.Log("ACCEPTOR THREAD: Ending...");
		}



	}
}