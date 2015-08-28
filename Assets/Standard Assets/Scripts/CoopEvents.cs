using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace CoopWorld
{


    public class CoopEvents {

        private static CoopEvents instance;

        private CoopEvents() { }

        public static CoopEvents Instance
	    {
		    get 
		    {
			    if (instance == null)
			    {
                    instance = new CoopEvents();
			    }
			    return instance;
		    }
	    }

        //private Action<int> action;
        //private bool didConnectOccur = false;

        private PlayerConnectData connectData;

        public void RegisterToConnectEvent(Action<int> method)
        {
            //action = method;
            connectData = new PlayerConnectData(method);
        }

        public void OnPlayerConnect(int playerId)
        {
            CoopWorldInput.AddNewInput(playerId);
            connectData.ConnectOccured(playerId);
        }

        public void HandleEvents()
        {
            if (connectData != null && connectData.IsValid())
            {
                connectData.spawnMethod(connectData.playerId);  //calls selected spawn method with playerId
                connectData.Empty();
            }
        }


    }

    public class PlayerConnectData
    {

        public int playerId = 0;
        public Action<int> spawnMethod;


        public PlayerConnectData(Action<int> spawnMethod)
        {
            this.spawnMethod = spawnMethod;
        }

        public void ConnectOccured(int playerId)
        {
            this.playerId = playerId;
        }

        public void Empty()
        {
            this.playerId = 0;  //makes object invalid
        }

        public bool IsValid()
        {
            return (playerId != 0 && spawnMethod !=null);
        }


    }

    
}