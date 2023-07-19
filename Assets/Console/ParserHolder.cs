using UnityEngine;

namespace Console
{
    [CommandHolder]
    public static class ParserHolder
    {
        #region Vectors
        [Parser(3)]
        public static Vector3 Vector3Parser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            //TODO: Add options for the players pos, or where they are looking
            return new Vector3(float.Parse(strings[0]), float.Parse(strings[1]), float.Parse(strings[2]));
        }
        
        [Parser(3)]
        public static Vector3Int Vector3IntParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            //TODO: Add options for the players pos, or where they are looking
            return new Vector3Int(int.Parse(strings[0]), int.Parse(strings[1]), int.Parse(strings[2]));
        }
        
        [Parser(2)]
        public static Vector2 Vector2Parser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            //TODO: Add options for the players pos, or where they are looking
            return new Vector2(float.Parse(strings[0]), float.Parse(strings[1]));
        }
        
        [Parser(2)]
        public static Vector2Int Vector2IntParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            //TODO: Add options for the players pos, or where they are looking
            return new Vector2Int(int.Parse(strings[0]), int.Parse(strings[1]));
        }

        [Parser(4)]
        public static Quaternion QuaternionParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            //TODO: Options for player look dir?
            if (strings[0] == "e")
                return Quaternion.Euler(float.Parse(strings[1]), float.Parse(strings[2]), float.Parse(strings[3]));
            return new Quaternion(float.Parse(strings[0]), float.Parse(strings[1]), float.Parse(strings[2]), float.Parse(strings[3]));
        }
        #endregion

        #region Primitive Value Types
        
        [Parser]
        public static string StringParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return strings[0];
        }

        [Parser]
        public static bool BoolParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return bool.Parse(strings[0]);
        }
        
        [Parser]
        public static float FloatParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return float.Parse(strings[0]);
        }
        
        [Parser]
        public static double DoubleParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return double.Parse(strings[0]);
        }
        
        [Parser]
        public static decimal Decimal(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return decimal.Parse(strings[0]);
        }

        [Parser]
        public static char CharParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return char.Parse(strings[0]);
        }
        
        [Parser]
        public static byte ByteParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return byte.Parse(strings[0]);
        }
        
        [Parser]
        public static sbyte SByteParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return sbyte.Parse(strings[0]);
        }
        
        [Parser]
        public static short ShortParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return short.Parse(strings[0]);
        }
        
        [Parser]
        public static ushort UShortParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return ushort.Parse(strings[0]);
        }
        
        [Parser]
        public static int IntParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return int.Parse(strings[0]);
        }
        
        [Parser]
        public static uint UIntParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return uint.Parse(strings[0]);
        }

        [Parser]
        public static long LongParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return long.Parse(strings[0]);
        }
        
        [Parser]
        public static ulong ULongParser(string[] strings, ConsoleLogger.CommandCallInfo info)
        {
            return ulong.Parse(strings[0]);
        }

        #endregion
    }
}