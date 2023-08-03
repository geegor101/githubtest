using System.Collections;
using System.Reflection;
using Code.Console;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Code.UnitTesting.EditorTests
{
    public class ParserTest
    {
        [Test]
        public void TestSimpleParsing()
        {
            Assert.That(true.Equals(ParserHolder.BoolParser(new []{"true"}, null)));
            Assert.That(new Vector3(0, 4, 5).Equals(ParserHolder.Vector3Parser(new []{"0", "4", "5"}, null)));
        }

        [Test]
        public void TestRangeLength()
        {
            ConsoleLogger.Initialize();
            
            ParameterInfo[] parameterInfos = typeof(ParserTest)
                .GetMethod("SampleCommand", BindingFlags.Static | BindingFlags.NonPublic)
                .GetParameters();
            Assert.That(parameterInfos.GetRange().Equals(4..4));
        }

        [Test]
        public void TestCallCommandPass()
        {
            ConsoleLogger.Initialize();
            ConsoleLogger.SendCommandString("$unittest str 3 4 5");
            Assert.That(Commands.testPassed);
        }

        [Test]
        public void TestCallCommandFail()
        {
            ConsoleLogger.Initialize();
            ConsoleLogger.SendCommandString("$unittest hell 3 4 2");
            Assert.False(Commands.testPassed);
        }

        [Command("sampleName", true, true)]
        private static void SampleCommand(string str, [CommandParameterLength(3)] int[] ints, CommandCallInfo info)
        {
            
        }
    

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ParserTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            //yield return new WaitForSeconds(4);
            yield break;
        }
    }
}
