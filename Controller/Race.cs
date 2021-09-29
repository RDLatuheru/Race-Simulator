using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        private Dictionary<Section, SectionData> _positions;
        private Random _random;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            InitializeSections();
            InitializeStartPositions();
        }

        public SectionData GetSectionData(Section section)
        {
            if (_positions[section] == null)
            {
                _positions.Add(section, new SectionData());
            }

            return _positions[section];
        }

        public void RandomizeEquipment()
        {
            foreach (var deelnemer in Participants)
            {
                deelnemer.Equipment.Performance = _random.Next(0,10);
                deelnemer.Equipment.Quality = _random.Next(0, 10);
            }
        }

        public void InitializeStartPositions()
        {
            Stack<Section> sections = new Stack<Section>();
            foreach (KeyValuePair<Section,SectionData> entry in _positions)
            {
                if (entry.Key.SectionType == Section.SectionTypes.StartGrid)
                {
                    sections.Push(entry.Key);
                }
            }
            
            int p = Participants.Count;
            for (int i = sections.Count; i > 0; i--)
            {
                SectionData SD = GetSectionData(sections.Pop());
                try
                {
                    SD.Left = Participants[p - 1];
                    SD.Right = Participants[p - 2];
                    p -= 2;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return;
                }
            }
        }

        public void InitializeSections()
        {
            foreach (Section section in Track.Sections)
            {
                _positions.Add(section, new SectionData());
            }
        }
    }
}
