using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        
        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            Track result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track track = new Track("Track1", new[]
            {
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.LeftCorner, 
                Section.SectionTypes.Straight, Section.SectionTypes.Finish
            });
            _competition.Tracks.Enqueue(track);
            Track result = _competition.NextTrack();
            Assert.AreEqual(track, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Track track = new Track("Track1", new[]
            {
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.LeftCorner, 
                Section.SectionTypes.Straight, Section.SectionTypes.Finish
            });
            _competition.Tracks.Enqueue(track);
            Track result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track track = new Track("Track1", new[]
            {
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.LeftCorner, 
                Section.SectionTypes.Straight, Section.SectionTypes.Finish
            });
            Track track2 = new Track("Track2", new[]
            {
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.LeftCorner, 
                Section.SectionTypes.Straight, Section.SectionTypes.Finish
            });
            _competition.Tracks.Enqueue(track);
            _competition.Tracks.Enqueue(track2);
            _competition.NextTrack();
            Track result = _competition.NextTrack();
            Assert.AreEqual(track2, result);
        }
    }
}