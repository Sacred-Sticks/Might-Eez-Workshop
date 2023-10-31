using Kickstarter.Inputs;
using UnityEngine;

namespace Kickstarter.Identification
{
    public class Player : MonoBehaviour
    {
        public enum PlayerIdentifier
        {
            None,
            KeyboardAndMouse,
            ControllerOne,
            ControllerTwo,
            ControllerThree,
            ControllerFour,
        }

        [SerializeField] private PlayerIdentifier playerID;

        private IInputReceiver[] inputReceivers;

        public PlayerIdentifier PlayerID
        {
            get
            {
                return playerID;
            }
            set
            {
                foreach (var inputReceiver in inputReceivers)
                    inputReceiver.UnsubscribeToInputs(this);
                playerID = value;
                foreach (var inputReceiver in inputReceivers)
                    inputReceiver.SubscribeToInputs(this);
            }
        }

        private void Awake()
        {
            inputReceivers = GetComponents<IInputReceiver>();
        }

        private void Start()
        {
            foreach (var inputReceiver in inputReceivers)
            {
                inputReceiver.SubscribeToInputs(this);
            }
        }

        private void OnEnable()
        {
            foreach (var inputReceiver in inputReceivers)
            {
                inputReceiver.SubscribeToInputs(this);
            }
        }

        private void OnDisable()
        {
            foreach (var inputReceiver in inputReceivers)
            {
                inputReceiver.UnsubscribeToInputs(this);
            }
        }
    }
}