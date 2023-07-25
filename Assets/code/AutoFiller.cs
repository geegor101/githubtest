using System;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace code
{
    public static class AutoFiller 
    {
        /*
        public class VGTBehaviorCallback
        {
            public static VGTBehaviorCallback Empty = new VGTBehaviorCallback();
        }
        
        public delegate void StartDelegate(VGTBehavior behavior, VGTBehaviorCallback callback);

        public static event StartDelegate OnStart;
        OnStart?.Invoke(this, VGTBehaviorCallback.Empty);
        */

        
        
        /*
        protected virtual void Start()
        {
            
        }
        */

        public static void AutofillAttributes(this MonoBehaviour behaviour)
        {
            foreach (FieldInfo fieldInfo in behaviour.GetType().GetRuntimeFields().Where(info => info.IsDefined(typeof(AutofillBehaviorAttribute))))
            {
                fieldInfo.SetValueOptimized(behaviour,  behaviour.GetComponent(fieldInfo.FieldType));
            }
        }

        public static void AutofillUIElements(this MonoBehaviour behaviour, UIDocument document)
        {
            foreach (FieldInfo fieldInfo in behaviour.GetType().GetFields().Where(info => info.IsDefined(typeof(AutofillUIElementAttribute))))
            {
                fieldInfo.SetValueOptimized(behaviour, 
                    document.rootVisualElement.Q(fieldInfo.GetCustomAttribute<AutofillUIElementAttribute>().name));
            }
        }
        
    }
    
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class AutofillBehaviorAttribute : Attribute
    {
        public AutofillBehaviorAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class AutofillUIElementAttribute : Attribute
    {
        public readonly string name;
        public AutofillUIElementAttribute(string name)
        {
            this.name = name;
        }
    }
}