using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Threading;

public class DiscoveryThread {

	private static DiscoveryThread instance;
	
	private DiscoveryThread() {}
	
	public static DiscoveryThread Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new DiscoveryThread();
			}
			return instance;
		}
	}
	
	UdpClient udpClient;


    private volatile bool _isRunning = true;

	public void run() {
        Debug.Log("DISCOVERY THREAD: started...");
		try {
			udpClient = new UdpClient(8888);
			//udpClient.EnableBroadcast = true; //only 4 send

            while (_isRunning)
            {
				Debug.Log(GetType().Name + ">>>Ready to receive broadcast packets!");

				// Receive a packet
				//IPEndPoint object will allow us to read datagrams sent from any source.
				IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8888);

				// Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint); //updates RemoteIpEndPoint to be the sender
				string returnData = Encoding.ASCII.GetString(receiveBytes);
				
				// Uses the IPEndPoint object to determine which of these two hosts responded.
				Debug.Log("This is the message you received " +
				                  returnData.ToString());
				Debug.Log("This message was sent from " +
				                  RemoteIpEndPoint.Address.ToString() +
				                  " on their port number " +
				                  RemoteIpEndPoint.Port.ToString());
		
				// See if the packet holds the right command (message)
				string message = returnData.ToString().Trim();
				if (message.Equals("DISCOVER_FUIFSERVER_REQUEST")) {
                    Byte[] sendBytes = Encoding.ASCII.GetBytes("DISCOVER_FUIFSERVER_RESPONSE");
                    udpClient.Send(sendBytes, sendBytes.Length, RemoteIpEndPoint);
				}

			}
		} catch (Exception e) {
			Debug.LogError(e.ToString());
		}
        Debug.Log("DISCOVERY THREAD: ending...");
	}

    public void RequestStop()
    {
        _isRunning = false;
        udpClient.Close();
    }
}
