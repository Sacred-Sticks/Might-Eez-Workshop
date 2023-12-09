using UnityEngine;
using UnityEngine.UIElements;

namespace Kickstarter.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class Buttons : UIElement<Button>
    {
        [SerializeField] private ButtonElement[] elements;

        private void OnEnable()
        {
            InitializeElements(elements);
        }

        private void OnDisable()
        {
            ClearElements();
        }
    }
}
