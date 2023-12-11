using Kickstarter.Variables;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Endgame : MonoBehaviour
{
    [SerializeField] private string label;
    [SerializeField] private StringVariable endgameStatus;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Label>(label).text = endgameStatus.Value;
    }
}