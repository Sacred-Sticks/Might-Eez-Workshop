using System.Collections;
using Kickstarter.Observer;
using Kickstarter.StateControllers;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[SelectionBase]
public class Customer : Observable
{

    private NavMeshAgent agent;
    
    public Toy DesiredToy { get; private set; }
    private Vector3 orderPosition;
    private Vector3 waitingPosition;
    private Vector3 exitPosition;
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
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        exitPosition = transform.position;
        var part0 = DesiredToy.ToyParts[0];
        var part1 = DesiredToy.ToyParts[1];
        var part2 = DesiredToy.ToyParts[2];
        var part3 = DesiredToy.ToyParts[3];
        var part4 = DesiredToy.ToyParts[4];
        var part5 = DesiredToy.ToyParts[5];
        
        stateMachine = new StateMachine<CustomerStatus>.Builder()
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
            .WithInitialState(CustomerStatus.Arriving)
            .Build();

        StartCoroutine(CustomerTimer());
    }

    private void Update()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
            return;
        stateMachine.CurrentState = stateMachine.CurrentState switch
        {
            CustomerStatus.Arriving => CustomerStatus.Waiting,
            _ => stateMachine.CurrentState,
        };
    }
    #endregion

    #region State Changes
    private void Arrive()
    {
        NotifyObservers(CustomerStatus.Arriving);
        agent.SetDestination(orderPosition);
    }
    
    private void PlaceOrder()
    {
        NotifyObservers(CustomerStatus.Waiting);
        agent.SetDestination(waitingPosition);
    }

    private void PayForOrder()
    {
        NotifyObservers(CustomerStatus.OrderFulfilled);
        agent.SetDestination(exitPosition);
    }

    private void DeclineOrder()
    {
        NotifyObservers(CustomerStatus.OrderCancelled);
        OrderListUI.BlockOrder(this);
        OrderListUI.AddCustomerWhenEmpty();
        agent.SetDestination(exitPosition);
    }

    private void Leave()
    {
        NotifyObservers(CustomerStatus.Leaving);
        StopAllCoroutines();
        Destroy(gameObject);
    }
    #endregion

    private IEnumerator CustomerTimer()
    {
        RemainingTime = waitingTime;
        for (;;)
        {
            yield return new WaitForSeconds(timeStep);
            RemainingTime -= timeStep;
            OrderListUI.SetTimer(this, RemainingTime / waitingTime * 100);
            if (RemainingTime > 0)
                continue;
            stateMachine.CurrentState = CustomerStatus.OrderCancelled;
            break;
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
        private Vector3 orderPosition;
        private Vector3 waitingPosition;

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

        public Builder WithOrderPosition(Vector3 position)
        {
            orderPosition = position;
            return this;
        }

        public Builder WithWaitingPosition(Vector3 position)
        {
            waitingPosition = position;
            return this;
        }

        public Customer Build(GameObject targetGameObject)
        {
            var customer =  targetGameObject.AddComponent<Customer>();
            customer.waitingTime = waitingTime;
            customer.DesiredToy = targetToy;
            customer.price = toyPrice;
            customer.orderPosition = orderPosition;
            customer.waitingPosition = waitingPosition;
            return customer;
        }
    }
    #endregion
}