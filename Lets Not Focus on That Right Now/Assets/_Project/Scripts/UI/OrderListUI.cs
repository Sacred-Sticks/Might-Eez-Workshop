using System;
using Kickstarter.UI;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class OrderListUI : MonoBehaviour
{
    [SerializeField] private string listName;
    [SerializeField] private RadialProgress hi;

    private const string parentElementClass = "order-template";
    private const string timerClass = "timer";
    private const string timerBackgroundClass = "timer-background";
    private const string toyPartsClass = "toy-parts";
    private const string toyPartClass = "toy-part";
    
    private UIDocument document;
    private VisualElement ordersRoot;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        ordersRoot = document.rootVisualElement.Q<VisualElement>(listName);
    }

    private void Start()
    {
        var parentElement = new VisualElement();
        parentElement.AddToClassList(parentElementClass);
        
        var timerElement = new RadialProgress();
        timerElement.AddToClassList(timerClass);
        timerElement.progress = 50;
        
        var timerBackground = new VisualElement();
        timerBackground.AddToClassList(timerClass);
        timerBackground.AddToClassList(timerBackgroundClass);
        
        var pieces = new VisualElement();
        pieces.AddToClassList(toyPartsClass);
        
        //parentElement.contentContainer.Add(timerBackground);
        parentElement.contentContainer.Add(timerElement);
        parentElement.contentContainer.Add(pieces);

        ordersRoot.hierarchy.Add(parentElement);
    }

    [Serializable] private class RadialProgressCircle
    {
        [SerializeField] private string name;
    }
}
