using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kickstarter.UI;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class OrderListUI : MonoBehaviour
{
    [SerializeField] private string listName;

    private const string parentElementClass = "order-template";
    private const string timerClass = "timer";
    private const string toyPartsClass = "toy-parts";
    private const string toyPartClass = "toy-part";

    private UIDocument document;
    private static VisualElement ordersRoot;

    private static readonly Dictionary<Customer, VisualElement> customerOrders = new Dictionary<Customer, VisualElement>();

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        ordersRoot = document.rootVisualElement.Q<VisualElement>(listName);
    }

    public static void InjectNewOrder(Customer customer)
    {
        var parentElement = new VisualElement();
        parentElement.AddToClassList(parentElementClass);

        var timerElement = new RadialProgress();
        timerElement.AddToClassList(timerClass);
        timerElement.progress = 0;

        var pieces = new VisualElement();
        pieces.AddToClassList(toyPartsClass);

        AddToyParts(pieces, customer.DesiredToy.ToyParts);

        parentElement.AddChild(timerElement);
        parentElement.AddChild(pieces);

        ordersRoot.AddChild(parentElement);
        
        customerOrders.Add(customer, parentElement);
    }

    public static void RemoveOrder(Customer customer)
    {
        if (!customerOrders.ContainsKey(customer))
            return;
        ordersRoot.hierarchy.Remove(customerOrders[customer]);
        customerOrders.Remove(customer);
    }
    
    private static void AddToyParts(VisualElement parent, ToyPart[] parts)
    {
        foreach (var toyPart in parts)
        {
            string colorClass = toyPart.Color.ToString();
            string partClass = toyPart.Part.ToString();
            string materialClass = toyPart.Material.ToString();
            AddToyPart(parent, new[] { toyPartClass, colorClass, partClass, materialClass, });
        }
    }

    private static void AddToyPart(VisualElement parent, string[] partClasses) => parent.CreateChild(partClasses);
}
