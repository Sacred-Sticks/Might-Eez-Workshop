using Kickstarter.References;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CustomerBrain : MonoBehaviour, IObserver<Customer.CustomerStatus>
{
    [SerializeField] private Vector3Reference orderStationLocation;
    [SerializeField] private Vector3Reference waitingLocation;
    [SerializeField] private Vector3Reference exitLocation;
    
    private NavMeshAgent agent;
    
    #region Unity Events
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    #endregion
    
    public void OnNotify(Customer.CustomerStatus argument)
    {
        switch (argument)
        {
            case Customer.CustomerStatus.Arriving:
                SetTarget(orderStationLocation.Value);
                break;
            case Customer.CustomerStatus.Waiting:
                SetTarget(waitingLocation.Value);
                break;
            case Customer.CustomerStatus.OrderCancelled:
            case Customer.CustomerStatus.OrderFulfilled:
                SetTarget(exitLocation.Value);
                break;
        }
    }

    private void SetTarget(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }
}