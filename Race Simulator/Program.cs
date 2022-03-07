using System;
using System.Threading;
using System.Threading.Channels;
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
            
            Data.Initialize(true);
            ConsoleVisualisatie.Initialize();
            

            for (;;)
            {
                Thread.Sleep(500);
            }
        }
    }
}
