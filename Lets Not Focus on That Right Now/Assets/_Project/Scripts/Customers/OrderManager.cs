using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private static List<Customer> activeCustomers;

    private void Start()
    {
        activeCustomers = new List<Customer>();
    }

    public static void AddCustomer(Customer customer)
    {
        if (!activeCustomers.Contains(customer))
            activeCustomers.Add(customer);
    }

    public static bool FillOrder(Toy producedToy)
    {
        var correctCustomer = activeCustomers.Where(c => c.DesiredToy == producedToy).OrderByDescending(c => c.RemainingTime).FirstOrDefault();
        if (correctCustomer == null)
            return false;
        correctCustomer.FillOrder();
        activeCustomers.Remove(correctCustomer);
        return true;
    }
}