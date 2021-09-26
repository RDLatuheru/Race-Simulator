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

        public static void Initialize()
        {
            Comp = new Competition();
            DeelnemerToevoegen();
            TrackToevoegen();
        }

        public static void DeelnemerToevoegen()
        {
            Comp.Participants.Add(new Driver("Driver01", new Car(10, 10, 10), IParticipant.TeamColors.Red));
            Comp.Participants.Add(new Driver("Driver02", new Car(10, 10, 10), IParticipant.TeamColors.Blue));
        }

        public static void TrackToevoegen()
        {
            Section.SectionTypes[] sections01 =
            {
                Section.SectionTypes.Straight,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.RightCorner
            };
            Comp.Tracks.Enqueue(new Track("Track01", sections01));

            Section.SectionTypes[] sections02 =
            {
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.Straight, 
                Section.SectionTypes.Straight, 
                Section.SectionTypes.Finish
            };
            Comp.Tracks.Enqueue(new Track("Track02", sections02));
        }

        public static void NextRace()
        {
            Track result = Comp.NextTrack();
            if (result != null)
            {
                CurrentRace = new Race(result, Comp.Participants);
            }
            else
            {
                Console.WriteLine("Geen race meer");
            }
        }
    }
}
