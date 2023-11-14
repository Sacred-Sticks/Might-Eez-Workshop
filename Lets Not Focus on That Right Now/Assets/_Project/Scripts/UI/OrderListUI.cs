using System.Collections;
using Kickstarter.UI;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class OrderListUI : MonoBehaviour
{
    [SerializeField] private string listName;
    [SerializeField] private VisualTreeAsset tree;

    private UIDocument document;
    private ListView listView;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        listView = document.rootVisualElement.Q<ListView>(listName);
        listView.makeItem = tree.CloneTree;
    }

    private IEnumerator Start()
    {
        for (;;)
        {
            var parentElement = new VisualElement();
            var timerElement = new RadialProgress();
            var pieces = new ListView();
            
            parentElement.contentContainer.Add(timerElement);
            parentElement.contentContainer.Add(pieces);
            
            listView.hierarchy.Add(parentElement);
            listView.Rebuild();
            yield return new WaitForSeconds(1);
        }
    }
}