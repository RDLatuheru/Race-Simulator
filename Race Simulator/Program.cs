using System;
using System.Threading;
using Model;
using Controller;
using Race_Simulator;

namespace Race_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            
            Data.Initialize();
            
            Data.NextRace();
            ConsoleVisualisatie.Initialize();
            ConsoleVisualisatie.DrawTrack(Data.CurrentRace.Track);
            Console.WriteLine();
            
            /*Data.NextRace();
            ConsoleVisualisatie.Initialize();
            ConsoleVisualisatie.DrawTrack(Data.CurrentRace.Track);
            
            Data.NextRace();*/

            for (;;)
            {
                Thread.Sleep(100);
            }
        }
    }
}
