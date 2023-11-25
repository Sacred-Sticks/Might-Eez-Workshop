using System.Collections.Generic;
using System.Linq;
using Kickstarter.Inputs;
using UnityEngine;

namespace Kickstarter.Identification
{
    [SelectionBase]
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
        private readonly List<IInputReceiver> registeredListeners = new List<IInputReceiver>();

        public PlayerIdentifier PlayerID
        {
            get
            {
                return playerID;
            }
            set
            {
                DeregisterInputs();
                playerID = value;
                RegisterInputs();
            }
        }

        private void Awake()
        {
            inputReceivers = GetComponents<IInputReceiver>();
        }

        private void Start() => RegisterInputs();

        private void OnEnable() => RegisterInputs();

        private void OnDisable() => DeregisterInputs();

        private void RegisterInputs()
        {
            foreach (var inputReceiver in inputReceivers.Where(i => !registeredListeners.Contains(i)))
            {
                if (inputReceiver.SubscribeToInputs(this))
                    registeredListeners.Add(inputReceiver);
            }
        }

        private void DeregisterInputs()
        {
            foreach (var inputReceiver in inputReceivers.Where(i => registeredListeners.Contains(i)))
            {
                if (inputReceiver.UnsubscribeToInputs(this))
                    registeredListeners.Remove(inputReceiver);
            }
        }
    }
}
