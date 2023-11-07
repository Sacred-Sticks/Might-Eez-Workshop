using System.Collections;
using Kickstarter.StateControllers;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private Toy desiredToy;
    private float waitingTime;
    private float price;

    private enum CustomerStatus
    {
        Arriving,
        Waiting,
        OrderFulfilled,
        OrderCancelled,
        Leaving,
    }

    private StateMachine<CustomerStatus> stateMachine;

    #region Unity Events
    private void Start()
    {
        stateMachine = new StateMachine<CustomerStatus>.Builder()
            .WithInitialState(CustomerStatus.Arriving)
            .WithTransition(CustomerStatus.Arriving, CustomerStatus.Waiting)
            .WithTransition(CustomerStatus.Waiting, CustomerStatus.OrderFulfilled)
            .WithTransition(CustomerStatus.Waiting, CustomerStatus.OrderCancelled)
            .WithTransition(CustomerStatus.OrderFulfilled, CustomerStatus.Leaving)
            .WithTransition(CustomerStatus.OrderCancelled, CustomerStatus.Leaving)
            .WithStateListener(CustomerStatus.Waiting, transitionType.Start, PlaceOrder)
            .WithStateListener(CustomerStatus.OrderFulfilled, transitionType.Start, PayForOrder)
            .WithStateListener(CustomerStatus.OrderCancelled, transitionType.Start, DeclineOrder)
            .WithStateListener(CustomerStatus.Leaving, transitionType.End, Leave)
            .Build();
    }
    #endregion

    #region State Changes
    private void PlaceOrder()
    {
        
    }

    private void PayForOrder()
    {
        
    }

    private void DeclineOrder()
    {
        
    }

    private void Leave()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
    #endregion

    private IEnumerator CustomerTimer()
    {
        yield return new WaitForSeconds(waitingTime);
        stateMachine.CurrentState = CustomerStatus.OrderCancelled;
    }
    
    #region Sub Classes
    public class Builder
    {
        private Toy targetToy;
        private float waitingTime;
        private float price;

        public Builder WithPatience(float timeToWait)
        {
            waitingTime = timeToWait;
            return this;
        }
        
        public Builder WithToy(Toy toy)
        {
            this.targetToy = toy;
            return this;
        }

        public Builder WithPrice(float price)
        {
            this.price = price;
            return this;
        }

        public Customer Build(GameObject targetGameObject)
        {
            var customer =  targetGameObject.AddComponent<Customer>();
            customer.waitingTime = waitingTime;
            customer.desiredToy = targetToy;
            customer.price = price;
            return customer;
        }
    }
    #endregion
}