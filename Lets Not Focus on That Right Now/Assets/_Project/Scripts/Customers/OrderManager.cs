using System.Collections.Generic;
using System.Linq;

public class OrderManager
{
    private static readonly List<Customer> activeCustomers = new List<Customer>();

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