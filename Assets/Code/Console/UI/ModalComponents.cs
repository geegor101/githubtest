using System;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Code.Console.UI
{
    public class Draggable : VisualElement
    {
        [Preserve]
        public new class UxmlFactory : UxmlFactory<Draggable, UxmlTraits> {}

        private DraggingManipulator _draggingManipulator;
        
        public Draggable()
        {
            this.AddManipulator(_draggingManipulator = new DraggingManipulator());
        }
        
    }

    public class Resizeable : VisualElement
    {

        [Preserve]
        public new class UxmlFactory : UxmlFactory<Resizeable, UxmlTraits> {}
        
        private ResizeManipulator _resizeManipulator;

        public Resizeable()
        {
            this.AddManipulator(_resizeManipulator = new ResizeManipulator());
        }
    }

    public abstract class BaseDraggingManipulator : MouseManipulator
    {
        protected BaseDraggingManipulator()
        {
            activators.Add(new ManipulatorActivationFilter()
            {
                button = MouseButton.LeftMouse
            });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private bool _active = false;

        private void OnMouseMove(MouseMoveEvent @event)
        {
            if (_active && @event.pressedButtons == 1)
            {
                SelectedMouseMove(@event);
                @event.StopPropagation();
                
            }
        }

        private void OnMouseDown(MouseDownEvent @event)
        {
            if (!_active)
            {
                if (CanStartManipulation(@event))
                {
                    target.CaptureMouse();
                    _active = true;
                }
            }
            else
            {
                @event.StopPropagation();
            }
        }
        
        private void OnMouseUp(MouseUpEvent @event)
        {
            if (_active)
            {
                _active = false;
                target.ReleaseMouse();
            }
        }

        protected abstract void SelectedMouseMove(MouseMoveEvent @event);

    }
    
    public class DraggingManipulator : BaseDraggingManipulator 
    {
        protected override void SelectedMouseMove(MouseMoveEvent @event)
        {
            Vector3 initial = target.parent.transform.position;
            target.parent.transform.position = 
                new Vector3(initial.x + @event.mouseDelta.x, initial.y + @event.mouseDelta.y, initial.z);
        }
    }

    public class ResizeManipulator : BaseDraggingManipulator
    {
        protected override void SelectedMouseMove(MouseMoveEvent @event)
        {
            //Vector3 initial = target.parent.transform.scale;
            //target.parent.transform.scale = new Vector3(initial.x + @event.mouseDelta.x, initial.y + @event.mouseDelta.y, initial.z);
            IStyle style = target.parent.style;
            float height = Math.Clamp(style.height.value.value + @event.mouseDelta.y, 
                style.maxHeight.value.value, style.minHeight.value.value);
            float width = Math.Clamp(style.width.value.value + @event.mouseDelta.x, 
                style.maxWidth.value.value, style.minWidth.value.value);
            style.height = height;
            style.width = width;
            //target.parent.style.height = target.parent.style.height.value.value + @event.mouseDelta.y;
        }
    }
}