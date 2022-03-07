using Model;
using NUnit.Framework;
using Race_Simulator;
using System;
using System.Collections.Generic;
using System.Text;
using static Model.IParticipant;

namespace ControllerTest
{
    [TestFixture]
    class RaceSimulator_ConsoleVisualisatie
    {

        #region graphics
        //  ═ ║   ╔   ╗   ╚  ╝
        private static string[] _finishHorizontal = { "════", " #1 ", " #2 ", "════" };
        private static string[] _finishVertical = { "║  ║", "║12║", "║##║", "║  ║" };
        private static string[] _startHorizontal = { "════", " *1 ", " *2 ", "════" };
        private static string[] _startVertical = { "║  ║", "║12║", "║**║", "║  ║" };
        private static string[] _straightHorizontal = { "════", " 1  ", " 2  ", "════" };
        private static string[] _straightVertical = { "║  ║", "║1 ║", "║ 2║", "║  ║" };
        private static string[] _corner1 = { "═══╗", "   ║", " 1 ║", "╗ 2║" };
        private static string[] _corner2 = { "║1 ╚", "║ 2 ", "║   ", "╚═══" };
        private static string[] _corner3 = { "╔═══", "║   ", "║  1", "║ 2╔" };
        private static string[] _corner4 = { "╝1 ║", "2  ║", "   ║", "═══╝" };


        #endregion

        [SetUp]
        public void SetUp()
        {
        }

        [TestCase("════", "════")]
        [TestCase("║12║", "║AB║")]
        [TestCase(" *1 ", " *A ")]
        [TestCase("║ 2 ", "║ B ")]
        public void DrawDrivers_StringReplace(string s, string expected)
        {
            IParticipant d1 = new Driver("A", new Car(10, 10, 10), TeamColors.Red);
            IParticipant d2 = new Driver("B", new Car(10, 10, 10), TeamColors.Blue);

            var result = ConsoleVisualisatie.DrawDrivers(s, d1, d2);

            Assert.AreEqual(result, expected);
        }

        [TestCase(ConsoleVisualisatie.Direction.North, ConsoleVisualisatie.Direction.East)]
        [TestCase(ConsoleVisualisatie.Direction.East, ConsoleVisualisatie.Direction.South)]
        [TestCase(ConsoleVisualisatie.Direction.South, ConsoleVisualisatie.Direction.West)]
        [TestCase(ConsoleVisualisatie.Direction.West, ConsoleVisualisatie.Direction.North)]
        public void SetNewDirection_RightCorner(ConsoleVisualisatie.Direction inital, ConsoleVisualisatie.Direction expected)
        {
            ConsoleVisualisatie.Direction d = ConsoleVisualisatie.SetNewDirection(Section.SectionTypes.RightCorner, inital);

            Assert.AreEqual(d, expected);
        }

        [TestCase(ConsoleVisualisatie.Direction.North, ConsoleVisualisatie.Direction.West)]
        [TestCase(ConsoleVisualisatie.Direction.West, ConsoleVisualisatie.Direction.South)]
        [TestCase(ConsoleVisualisatie.Direction.South, ConsoleVisualisatie.Direction.East)]
        [TestCase(ConsoleVisualisatie.Direction.East, ConsoleVisualisatie.Direction.North)]
        public void SetNewDirection_LeftCorner(ConsoleVisualisatie.Direction inital, ConsoleVisualisatie.Direction expected)
        {
            ConsoleVisualisatie.Direction d = ConsoleVisualisatie.SetNewDirection(Section.SectionTypes.LeftCorner, inital);

            Assert.AreEqual(d, expected);
        }


    }
}
