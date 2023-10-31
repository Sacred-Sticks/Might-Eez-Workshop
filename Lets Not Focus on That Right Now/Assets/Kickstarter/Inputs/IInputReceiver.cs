using Kickstarter.Identification;

namespace Kickstarter.Inputs
{
    public interface IInputReceiver
    {
        public void SubscribeToInputs(Player player);

        public void UnsubscribeToInputs(Player player);
    }
}
