using System.Collections;
using Kickstarter.Observer;
using Kickstarter.StateControllers;
using UnityEngine;

public class Customer : Observable
{
    public Toy DesiredToy { get; private set; }
    private float waitingTime;
    private float price;
    
    public float RemainingTime { get; private set; }

    private const float timeStep = 1;

    public enum CustomerStatus
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
        var part0 = DesiredToy.ToyParts[0];
        var part1 = DesiredToy.ToyParts[1];
        var part2 = DesiredToy.ToyParts[2];
        var part3 = DesiredToy.ToyParts[3];
        var part4 = DesiredToy.ToyParts[4];
        var part5 = DesiredToy.ToyParts[5];
        
        stateMachine = new StateMachine<CustomerStatus>.Builder()
            .WithInitialState(CustomerStatus.Arriving)
            .WithTransition(CustomerStatus.Arriving, CustomerStatus.Waiting)
            .WithTransition(CustomerStatus.Waiting, CustomerStatus.OrderFulfilled)
            .WithTransition(CustomerStatus.Waiting, CustomerStatus.OrderCancelled)
            .WithTransition(CustomerStatus.OrderFulfilled, CustomerStatus.Leaving)
            .WithTransition(CustomerStatus.OrderCancelled, CustomerStatus.Leaving)
            .WithStateListener(CustomerStatus.Arriving, transitionType.Start, Arrive)
            .WithStateListener(CustomerStatus.Waiting, transitionType.Start, PlaceOrder)
            .WithStateListener(CustomerStatus.OrderFulfilled, transitionType.Start, PayForOrder)
            .WithStateListener(CustomerStatus.OrderCancelled, transitionType.Start, DeclineOrder)
            .WithStateListener(CustomerStatus.Leaving, transitionType.Start, Leave)
            .Build();
    }
    #endregion

    #region State Changes
    private void Arrive()
    {
        NotifyObservers(stateMachine.CurrentState); // state is Arriving
    }
    
    private void PlaceOrder()
    {
        NotifyObservers(stateMachine.CurrentState); // state is Waiting
    }

    private void PayForOrder()
    {
        NotifyObservers(stateMachine.CurrentState); // state is OrderFilled
    }

    private void DeclineOrder()
    {
        NotifyObservers(stateMachine.CurrentState); // state is OrderDenied
    }

    private void Leave()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
    #endregion

    private IEnumerator CustomerTimer()
    {
        for (;;)
        {
            yield return new WaitForSeconds(timeStep);
            RemainingTime -= timeStep;
            if (RemainingTime <= 0)
                stateMachine.CurrentState = CustomerStatus.OrderCancelled;
        }
    }

    public void FillOrder()
    {
        stateMachine.CurrentState = CustomerStatus.OrderFulfilled;
    }
    
    #region Sub Classes
    public class Builder
    {
        private Toy targetToy;
        private float waitingTime;
        private float toyPrice;

        public Builder WithPatience(float timeToWait)
        {
            waitingTime = timeToWait;
            return this;
        }
        
        public Builder WithToy(Toy toy)
        {
            targetToy = toy;
            return this;
        }

        public Builder WithPrice(float price)
        {
            toyPrice = price;
            return this;
        }

        public Customer Build(GameObject targetGameObject)
        {
            var customer =  targetGameObject.AddComponent<Customer>();
            customer.waitingTime = waitingTime;
            customer.DesiredToy = targetToy;
            customer.price = toyPrice;
            return customer;
        }
    }
    #endregion
}