using System.Collections.Generic;
using System.Linq;

public class OrderManager
{
    private static readonly List<Customer> activeCustomers = new List<Customer>();
    private const int maxCustomers = 8;

    public static void ClearCustomers()
    {
        activeCustomers.Clear();
    }
    
    public static void AddCustomer(Customer customer)
    {
        if (activeCustomers.Contains(customer))
            return;
        activeCustomers.Add(customer);
        OrderListUI.InjectNewOrder(customer);
        if (activeCustomers.Count > maxCustomers)
            TriggerGameLoss();
    }

    public static bool FillOrder(Toy producedToy)
    {
        var correctCustomer = activeCustomers.Where(c => c.DesiredToy == producedToy).OrderByDescending(c => c.RemainingTime).FirstOrDefault();
        if (correctCustomer == null)
            return false;
        correctCustomer.FillOrder();
        activeCustomers.Remove(correctCustomer);
        OrderListUI.RemoveOrder(correctCustomer);
        if (activeCustomers.Count == 0)
        {
            CustomerSpawner.instance.CreateCustomer(CustomerSpawner.instance.SpawnDelay);
        }
        return true;
    }

    private static void TriggerGameLoss()
    {
        GameManager.instance.EndGame(GameManager.EndGameStatus.Lose);
    }
}
