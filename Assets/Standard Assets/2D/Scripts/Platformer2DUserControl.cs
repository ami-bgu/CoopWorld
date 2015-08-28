using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using CoopWorld;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        public void CoopStart(int playerId)
        {
            coopPlayerId = playerId;
        }

        private int coopPlayerId;

        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            //float h = CrossPlatformInputManager.GetAxis("Horizontal");

            
			//my code
			float h = 0;

            if (CoopWorldInput.GetKey(coopPlayerId, CoopWorldInput.CoopKeyCode.Left))
            {
				h = -1;
				Debug.Log("Moving Character Left");
			}
            else if (CoopWorldInput.GetKey(coopPlayerId, CoopWorldInput.CoopKeyCode.Right))
            {
				h = 1;
				Debug.Log("Moving Character Right");
			}
			
			
				//float h = CoopWorldInput;
				//	CrossPlatformInputManager.VirtualButton btn = new CrossPlatformInputManager.VirtualButton ("remoteLeft");
		//	CrossPlatformInputManager.RegisterVirtualButton (btn);
		//	if (Input.GetKeyDown (KeyCode.K)) {
		//		btn.Pressed ();
		//	}


            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
