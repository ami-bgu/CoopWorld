using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System;

namespace CoopWorld
{
	public class CoopWorldInput {

		public enum CoopKeyCode
		{
			Left,
			Right
		}

        private static TwoKeyDictionary<int, CoopKeyCode, bool> dict = new TwoKeyDictionary<int, CoopKeyCode, bool>();

        private static ThreadedTcpSrvr server = new ThreadedTcpSrvr();

		public static void Init()
		{
			Debug.Log ("CoopWorld Initialized!");
            new Thread(new ThreadStart(DiscoveryThread.Instance.run)).Start();
            new Thread(new ThreadStart(CoopWorldInput.server.run)).Start();
		}


        private static Dictionary<CoopKeyCode, bool> MakeKeyCodesDictionary()
        {
            Dictionary<CoopKeyCode,bool> innerDict = new Dictionary<CoopKeyCode,bool>();
            foreach (CoopKeyCode keycode in Enum.GetValues(typeof(CoopKeyCode)))
            {
                innerDict.Add(keycode, false);
            }
            return innerDict;
        }

        public static void AddNewInput(int playerId)
        {
            CoopWorldInput.dict.Add(playerId, MakeKeyCodesDictionary());
        }
         

		public static bool GetKey(int playerId, CoopKeyCode keycode)
		{
            bool retVal = false;
            try
            {
                retVal = CoopWorldInput.dict[playerId][keycode];
            }
            catch
            {

            }
            return retVal;
		}

        public static void PressKey(int playerId, CoopKeyCode keycode)
		{
			Debug.Log ("Key pressed");
            CoopWorldInput.dict[playerId][keycode] = true;
		}

        public static void UnpressKey(int playerId, CoopKeyCode keycode)
		{
			Debug.Log ("Key UNpressed");
            CoopWorldInput.dict[playerId][keycode] = false;

		}


        public static void Destroy()
        {
            DiscoveryThread.Instance.RequestStop();
            CoopWorldInput.server.RequestStop();
        }

    }


    public class TwoKeyDictionary<K1, K2, T> :
       Dictionary<K1, Dictionary<K2, T>> { }

  

}