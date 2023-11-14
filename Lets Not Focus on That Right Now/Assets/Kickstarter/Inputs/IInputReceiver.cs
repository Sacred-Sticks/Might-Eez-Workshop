using Kickstarter.Identification;

namespace Kickstarter.Inputs
{
    public interface IInputReceiver
    {
        public bool SubscribeToInputs(Player player);

        public bool UnsubscribeToInputs(Player player);
    }
}
