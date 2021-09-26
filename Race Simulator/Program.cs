using System;
using Model;
using Controller;
using Race_Simulator;

namespace Race_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Raphael!");
            Data.Initialize();
            Data.NextRace();
            Console.WriteLine(Data.CurrentRace.Track.Name);
            Data.NextRace();
            Console.WriteLine(Data.CurrentRace.Track.Name);
            Data.NextRace();

            Console.ReadLine();
        }
    }
}
