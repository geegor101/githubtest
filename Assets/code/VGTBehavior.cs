using System;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace code
{
    public class VGTBehavior : MonoBehaviour
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

        [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
        protected sealed class AutofillBehaviorAttribute : Attribute
        {
            public AutofillBehaviorAttribute()
            {
            }
        }
        
        
        protected virtual void Start()
        {
            foreach (FieldInfo fieldInfo in GetType().GetRuntimeFields().Where(info => info.IsDefined(typeof(AutofillBehaviorAttribute))))
            {
                fieldInfo.SetValueOptimized(this, GetComponent(fieldInfo.FieldType));
            }
        }
    }
}