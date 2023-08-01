using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Console
{

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class CommandParameterLengthAttribute : Attribute
    {
        public readonly int range;
        
        /// <param name="range">The length of the given array. 
        /// Negatives require that many trailing values, 0 fills any trailing values</param>
        public CommandParameterLengthAttribute(int range)
        {
            this.range = range;
            
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
    public sealed class CommandAttribute : ConsoleAttribute
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
    public sealed class ParserAttribute : ConsoleAttribute
    {
        public readonly int Strings;
        
        public ParserAttribute(int Strings = 1)
        {
            if (Strings < 1)
                Strings = 1;
            this.Strings = Strings;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ConsoleAttribute : Attribute
    {
        public ConsoleAttribute()
        {}
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

            if (methodInfo.GetParameters()[0].ParameterType != typeof(string[]) ||
                methodInfo.GetParameters()[1].ParameterType != typeof(CommandCallInfo))
                throw new CommandParseException($"Incorrect method parameters on parser : {methodInfo.Name}");
            
            parsers[methodInfo.ReturnType] =
                new ParameterParser(
                    Delegate.CreateDelegate(Expression.GetFuncType(
                        typeof(string[]), typeof(CommandCallInfo), methodInfo.ReturnType
                        ), methodInfo),
                    methodInfo.GetCustomAttribute<ParserAttribute>().Strings);
        }

        internal static Object getValue(Type type, string[] input, CommandCallInfo info)
        {
            if (parsers.ContainsKey(type))
            {
                return parsers[type].ParserDelegate.DynamicInvoke(input, info);
            }
            throw new CommandParseException($"Type could not be parsed: {type.Name}");
        }

        internal static int GetLength(Type type)
        {
            if (parsers.ContainsKey(type))
                return parsers[type].length;
            throw new CommandParseException($"Type could not be parsed: {type.Name}");
        }

        internal static Range GetRange(this ParameterInfo[] infos)
        {
            int lower = 0, upper = 0;
            if (infos.Length == 1)
                return ..0;
            if (infos.Length < 1 || infos[^1].ParameterType != typeof(CommandCallInfo))
                throw new CommandParseException("Command registered without command info");
            
            /*
            if (infos[^2].ParameterType.IsArray)
            {
                if (!infos[^2].IsDefined(typeof(CommandParameterLengthAttribute)))
                    throw new CommandParseException($"Command registered with array parameter without length attribute");
                int length = infos[^2].ParameterType.GetCustomAttribute<CommandParameterLengthAttribute>().range;

                int total = Math.Abs(length) * GetLength(infos[^2].ParameterType.GetElementType());
                lower += total;
                upper += total;

                if (length < 1)
                    upper = Int32.MaxValue;
            }
            */
            
            Length(infos[^2], ref lower, ref upper, true);
            if (infos.Length == 2) return lower..upper;
            foreach (ParameterInfo parameterInfo in infos[..^2])
            {
                Length(parameterInfo, ref lower, ref upper);
            }

            return lower..upper;
        }

        private static void Length(ParameterInfo info, ref int lower, ref int upper, bool isLast = false)
        {
            int length;
            if (info.ParameterType.IsArray)
            {
                if (info.IsDefined(typeof(CommandParameterLengthAttribute)))
                {
                    int paramLength = info.GetCustomAttribute<CommandParameterLengthAttribute>().range;
                    if (paramLength < 1)
                    {
                        if (!isLast)
                            throw new CommandParseException("Trailing arrays must be at the end of commands");
                        upper = 999999;
                    }
                    length = Math.Abs(paramLength) * GetLength(info.ParameterType.GetElementType());
                }
                else
                    throw new CommandParseException($"Command array parameter registered without length attribute: {info.ParameterType.GetElementType()}");
            }
            else
            {
                length = GetLength(info.ParameterType);
            }
            lower += length;
            upper += length;
        }

    }

    internal class ParameterParser
    {
        public readonly Delegate ParserDelegate;
        public readonly int length;
        
        public ParameterParser(Delegate parserDelegate, int length)
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

    /*
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
    */
}