using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Comp { get; set; }
        public static Race CurrentRace { get; set; }

        private static bool isConsole;

        public static void Initialize(bool console)
        {
            Comp = new Competition();
            isConsole = console;
            DeelnemerToevoegen();
            TrackToevoegen();
            NextRace();
        }

        public static void DeelnemerToevoegen()
        {
            Comp.Participants.Add(new Driver("A", new Car(10, 10, 10), IParticipant.TeamColors.Red));
            Comp.Participants.Add(new Driver("B", new Car(10, 10, 10), IParticipant.TeamColors.Blue));
            Comp.Participants.Add(new Driver("C", new Car(10, 10, 10), IParticipant.TeamColors.Green));
            Comp.Participants.Add(new Driver("D", new Car(10, 10, 10), IParticipant.TeamColors.Grey));
            Comp.Participants.Add(new Driver("E", new Car(10, 10, 10), IParticipant.TeamColors.Yellow));
        }

        public static void TrackToevoegen()
        {
            Section.SectionTypes[] ride01 =
            {
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.Finish,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
            };
            Comp.Tracks.Enqueue(new Track("Warming up", ride01));

            /*Section.SectionTypes[] sections01 =
            {
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Finish,
            };
            Comp.Tracks.Enqueue(new Track("Race 1", sections01));*/
        }

        public static void NextRace()
        {
            
            Track result = Comp.NextTrack();
            if (isConsole) Console.Clear();
            if (result != null)
            {
                CurrentRace = new Race(result, Comp.Participants);
            }
            else
            {
                CurrentRace = null;
                Console.WriteLine("No more races");
            }
        }
    }
}
