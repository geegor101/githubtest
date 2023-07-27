using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Console.UI
{
    public class ModalWindow : VisualElement
    {
        
        [Preserve]
        public new class UxmlFactory : UxmlFactory<ModalWindow> {}

        public Dragger topBar;
        public Resizer resizeable;
        
        public ModalWindow()
        {
            usageHints = UsageHints.GroupTransform;
            this.AddManipulator(topBar = new Dragger());
            
        }

        public void OnClick(EventBase @base)
        {
            
        }
        
    }
}