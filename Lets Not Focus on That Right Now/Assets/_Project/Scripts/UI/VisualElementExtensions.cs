using System.Collections.Generic;
using UnityEngine.UIElements;

public static class VisualElementExtensions
{
    public static void AddChild(this VisualElement parent, VisualElement child)
    {
        parent.hierarchy.Add(child);
    }

    public static void CreateChild(this VisualElement parent, IEnumerable<string> childClasses)
    {
        var child = new VisualElement();
        foreach (string @class in childClasses)
            child.AddToClassList(@class);
        parent.AddChild(child);
    }
}
