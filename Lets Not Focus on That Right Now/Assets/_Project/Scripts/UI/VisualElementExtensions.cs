using System.Collections.Generic;
using System.Linq;
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

    public static void RemoveChildren(this VisualElement parent)
    {
        var children = parent.hierarchy.Children().ToArray();
        if (children.Length == 0)
            return;
        for (int i = 0; i < children.Length; i++)
        {
            var child = children[i];
            child.RemoveChildren();
            parent.hierarchy.Remove(child);
        }
    }
}
