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
        for (int i = 0; i < 8; i++)
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

        AddToyPart(pieces, new[] { toyPartClass, legClass, });
        AddToyPart(pieces, new[] { toyPartClass, legClass, });
        AddToyPart(pieces, new[] { toyPartClass, torsoClass, });
        AddToyPart(pieces, new[] { toyPartClass, armClass, });
        AddToyPart(pieces, new[] { toyPartClass, armClass, });
        AddToyPart(pieces, new[] { toyPartClass, headClass, });

        parentElement.contentContainer.Add(timerElement);
        parentElement.contentContainer.Add(pieces);

        ordersRoot.hierarchy.Add(parentElement);
    }

    private static void AddToyPart(VisualElement parent, string[] partClasses) => parent.CreateChild(partClasses);
}
