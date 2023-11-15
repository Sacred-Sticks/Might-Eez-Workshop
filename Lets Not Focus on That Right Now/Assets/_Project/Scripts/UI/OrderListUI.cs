using System;
using System.Collections;
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
    private const string legClass = "leg";
    private const string torsoClass = "torso";
    private const string armClass = "arm";
    private const string headClass = "head";

    private UIDocument document;
    private VisualElement ordersRoot;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        ordersRoot = document.rootVisualElement.Q<VisualElement>(listName);
    }

    private IEnumerator Start()
    {
        for (;;)
        {
            InjectNewOrder();
            yield return new WaitForSeconds(1);
        }
    }

    private void InjectNewOrder()
    {
        var parentElement = new VisualElement();
        parentElement.AddToClassList(parentElementClass);

        var timerElement = new RadialProgress();
        timerElement.AddToClassList(timerClass);
        timerElement.progress = 0;

        var pieces = new VisualElement();
        pieces.AddToClassList(toyPartsClass);

        AddToyPart(legClass, pieces);
        AddToyPart(legClass, pieces);
        AddToyPart(torsoClass, pieces);
        AddToyPart(armClass, pieces);
        AddToyPart(armClass, pieces);
        AddToyPart(headClass, pieces);

        parentElement.contentContainer.Add(timerElement);
        parentElement.contentContainer.Add(pieces);

        ordersRoot.hierarchy.Add(parentElement);
    }

    private static void AddToyPart(string partClass, VisualElement parent)
    {
        var toyPart = new VisualElement();
        toyPart.AddToClassList(toyPartClass);
        toyPart.AddToClassList(partClass);

        parent.hierarchy.Add(toyPart);
    }
}
