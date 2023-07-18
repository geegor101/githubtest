using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Console
{

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class CommandParameterAttribute : Attribute
    {
        public CommandParameterAttribute()
        {
            
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    sealed class CommandOverloadedAttribute : Attribute
    {
        public CommandOverloadedAttribute()
        {}
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CommandHolderAttribute : Attribute
    {}
    
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class CommandAttribute : Attribute
    { 
        public readonly bool isServer;
        public readonly bool isClient;
        /*
         * 0 - Clientside
         * 1 - spectator
         * 2 - player
         * 3 - mod
         * 4 - admin
         * 5 - dev
         */
        public readonly int permissionLevel;
        public readonly string name;
            
        public CommandAttribute(string name, bool isServer, bool isClient, int permissionLevel = 0)
        {
            this.name = name;
            this.isServer = isServer;
            this.isClient = isClient;
            this.permissionLevel = permissionLevel;
        }

        public static CommandAttribute GetCommandAttribute(MethodInfo mi)
        {
            return mi.GetCustomAttribute<CommandAttribute>();
        }
            
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class CommandDescriptionAttribute : Attribute
    {
        public readonly string description;
            
        public CommandDescriptionAttribute(string s)
        {
            description = s;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    sealed class ParserAttribute : Attribute
    {
        public readonly int Strings;
        
        public ParserAttribute(int Strings = 1)
        {
            this.Strings = Strings;
        }
    }
    
    public static class Parsers
    {
        private static readonly Dictionary<Type, ParameterParser> parsers 
            = new Dictionary<Type, ParameterParser>();

        internal static void AddParser(MethodInfo methodInfo)
        {
            //AddParser(methodInfo.ReturnType, (ParserDelegate) methodInfo.CreateDelegate(typeof(ParserDelegate)));

            if (parsers.ContainsKey(methodInfo.ReturnType))
            {
                Debug.LogWarning($"Multiple parsers attempted to be registered for {methodInfo.ReturnType.Name}");
                return;
            }

            parsers[methodInfo.ReturnType] =
                new ParameterParser((ParserDelegate)methodInfo.CreateDelegate(typeof(ParserDelegate)), 
                    methodInfo.GetCustomAttribute<ParserAttribute>().Strings);

            //ev += Commands.Vector3Parser;

        }

        public static event ParserDelegate ev;
        
        
        public delegate object ParserDelegate(string[] s);
        
        internal static Object getValue(Type type, string[] input)
        {
            if (parsers.ContainsKey(type))
            {
                return parsers[type].ParserDelegate.Invoke(input);
            }
            throw new CommandParseException($"Type could not be parsed: {type.Name}");
        }

        internal static int GetLength(Type type)
        {
            if (parsers.ContainsKey(type))
                return parsers[type].length;
            throw new CommandParseException($"Type could not be parsed: {type.Name}");
        }

        internal static object TryParse<T>(string[] input)
        {
            if (parsers.TryGetValue(typeof(T), out ParameterParser parserDelegate))
            {
                return parserDelegate.ParserDelegate.Invoke(input);
            }
            throw new CommandParseException($"Type could not be parsed: {nameof(T)}");
        }
        
    }

    internal class ParameterParser
    {
        public readonly Parsers.ParserDelegate ParserDelegate;
        public readonly int length;
        
        public ParameterParser(Parsers.ParserDelegate parserDelegate, int length)
        {
            ParserDelegate = parserDelegate;
            this.length = length;
        }
    }

    public class CommandParseException : Exception
    {
        public CommandParseException(string message) : base(message)
        {
            
        }
    }

    public static class DelegateUtil
    {

        public static bool ArgumentsMatch(this Delegate @delegate)
        {
            return true;
        }
    }

    public class Holder<T> where T : struct
    {
        public readonly T Value;

        private Holder(T value)
        {
            Value = value;
        }

        public static Holder<T> Of(T obj)
        {
            return new Holder<T>(obj);
        } 
    }
}