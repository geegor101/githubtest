using System;
using System.Linq;
using System.Reflection;
using Code;
using JetBrains.Annotations;

namespace Managers
{
    public static class AutoInitialize
    {
        internal static void AutoInit()
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsDefined(typeof(ManagerAttribute), true))
                .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                .Where(info => info.IsDefined(typeof(InitializerAttribute), false))
                .ForEach(info => info.Invoke(null, Array.Empty<object>()));
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    [MeansImplicitUse]
    public sealed class InitializerAttribute : Attribute
    {
        public readonly int priority;
        
        public InitializerAttribute(int priority)
        {
            this.priority = priority;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ManagerAttribute : Attribute
    {
    }
}