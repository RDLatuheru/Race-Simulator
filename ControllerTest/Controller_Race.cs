using Controller;
using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static Model.Section;

namespace ControllerTest
{
    [TestFixture]
    class Controller_Race
    {
        private Race testRace;

        [SetUp]
        public void SetUp()
        {
            Data.Initialize(false);
            testRace = new Race(Data.CurrentRace.Track, Data.CurrentRace.Participants);
        }

        [Test]
        public void GetSectionData_ReturnsData()
        {
            Section section = Data.CurrentRace.Track.Sections.First.Value;
            SectionData sectionData = testRace.GetSectionData(section);
            Assert.NotNull(sectionData);
        }

        [Test]
        public void InitializeStartPositions_RemovesExcessParticipants()
        {
            Competition competition = new Competition();
            Data.Comp = competition;

            Data.Comp.Participants.Add(new Driver("T", new Car(10, 10, 10), IParticipant.TeamColors.Yellow));
            Data.Comp.Participants.Add(new Driver("T", new Car(10, 10, 10), IParticipant.TeamColors.Yellow));
            Data.Comp.Participants.Add(new Driver("T", new Car(10, 10, 10), IParticipant.TeamColors.Yellow));
            Data.Comp.Participants.Add(new Driver("T", new Car(10, 10, 10), IParticipant.TeamColors.Yellow));

            SectionTypes[] testride =
            {
                SectionTypes.StartGrid,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            };
            Data.Comp.Tracks.Enqueue(new Track("test", testride));

            int result = Data.Comp.Participants.Count;
            Data.NextRace();
            new Race(Data.CurrentRace.Track, Data.Comp.Participants);
            int expected = Data.Comp.Participants.Count;

            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void InitializeStartPositions_ParticipantsOnStartGrid()
        {
            int count = 0;
            foreach (Section section in testRace.Track.Sections)
            {
                SectionData sectionData = testRace.GetSectionData(section);

                if (section.SectionType == SectionTypes.StartGrid && sectionData.Right != null)
                {
                    count++;
                }
                if (section.SectionType == SectionTypes.StartGrid && sectionData.Left != null)
                {
                    count++;
                }
            }

            Assert.AreEqual(count, testRace.Participants.Count);
        }


    }
}
