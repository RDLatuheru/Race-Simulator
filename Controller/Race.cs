using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Model;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        private Timer _timer;
        private Dictionary<Section, SectionData> _positions;
        private Random _random;
        public Dictionary<IParticipant, int> registerRounds;
        private int _finishCounter;
        private Dictionary<IParticipant, int> _initialEquipment;
        private Dictionary<IParticipant, string> _initialName;
        private int rounds;
        private int positionCount;


        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        public event EventHandler<NextRaceEventsArgs> NextRace;
        

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;


            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            registerRounds = new Dictionary<IParticipant, int>();
            _finishCounter = 0;
            _initialEquipment = new Dictionary<IParticipant, int>();
            _initialName = new Dictionary<IParticipant, string>();
            StartTime = DateTime.Now;
            rounds = 10;
            positionCount = 0;

            RandomizeEquipment();

            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;

            InitializeSections();
            InitializeStartPositions();
            Start();
        }

        public SectionData GetSectionData(Section section)
        {
            if (!_positions.ContainsKey(section))
            {
                _positions.Add(section, new SectionData());
            }

            return _positions[section];
        }
        
        public void OnTimedEvent(object o, EventArgs e)
        {
            EquipmentFailed();
            Drive();
            DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track));
            EquipmentRepaired();
        }

        public void Drive()
        {
            Section prevSection = Data.CurrentRace.Track.FirstSection;
            foreach (var entry in _positions.Reverse())
            {
                SectionData sd = entry.Value;
                CheckSection(prevSection, entry.Key, sd, Section.SectionLength);
                prevSection = entry.Key;
            }
        }

        public void CheckSection(Section prevSection, Section currentSection, SectionData currentSD, int sectionLength)
        {
            if (positionCount > 5) positionCount = 0;
            if (currentSD.Right != null && !currentSD.Right.Equipment.IsBroken)
            {
                currentSD.DistanceRight += currentSD.Right.Equipment.Speed;
                if (currentSD.DistanceRight >= sectionLength)
                {
                    if (currentSection.SectionType == Section.SectionTypes.Finish)
                    {
                        if (registerRounds[currentSD.Right] < rounds)
                        {
                            registerRounds[currentSD.Right]++;
                            positionCount++;
                            currentSD.Right.Position = positionCount;
                            AddPointsToDriver(currentSD.Right, currentSD.Right.Position);
                            MoveDriver(prevSection, currentSD, currentSD.Right);
                            
                        }
                        else
                        {
                            Finish(currentSD, true);
                        }
                    }
                    else
                    {
                        MoveDriver(prevSection, currentSD, currentSD.Right);
                    }
                }
            }
            if (currentSD.Left != null && !currentSD.Left.Equipment.IsBroken)
            {
                currentSD.DistanceLeft += currentSD.Left.Equipment.Speed;
                if (currentSD.DistanceLeft >= sectionLength)
                {
                    if (currentSection.SectionType == Section.SectionTypes.Finish)
                    {
                        if (registerRounds[currentSD.Left] < rounds)
                        {
                            registerRounds[currentSD.Left]++;
                            positionCount++;
                            currentSD.Left.Position = positionCount;
                            AddPointsToDriver(currentSD.Left, currentSD.Left.Position);
                            MoveDriver(prevSection, currentSD, currentSD.Left);
                        }
                        else
                        {
                            Finish(currentSD, false);
                        }
                    }
                    else
                    {
                        MoveDriver(prevSection, currentSD, currentSD.Left);
                    }
                }
            }
        }

        public void MoveDriver(Section prevSection, SectionData sd, IParticipant driver)
        {
            if (_positions[prevSection].Right == null)
            {
                _positions[prevSection].Right = driver;
                _positions[prevSection].DistanceRight = driver.Equipment.Speed;
                if (sd.Right == driver)
                {
                    sd.DistanceRight = 0;
                    sd.Right = null;
                }
                else
                {
                    sd.DistanceLeft = 0;
                    sd.Left = null;
                }
            }else if (_positions[prevSection].Left == null)
            {
                _positions[prevSection].Left = driver;
                _positions[prevSection].DistanceLeft = driver.Equipment.Speed;
                if (sd.Right == driver)
                {
                    sd.DistanceRight = 0;
                    sd.Right = null;
                }
                else
                {
                    sd.DistanceLeft = 0;
                    sd.Left = null;
                }
            }
        }

        public void Finish(SectionData sd, bool leftOrRight)
        {
            IParticipant driver;
            if (leftOrRight)
            {
                driver = sd.Right;
                sd.Right = null;
                sd.DistanceRight = 0;

            }
            else
            {
                driver = sd.Left;
                sd.Left = null;
                sd.DistanceLeft = 0;
            }
            _finishCounter++;
            AddPointsToDriver(driver, _finishCounter);


            if (_finishCounter == Participants.Count())
            {
                Stop();
            }
        }

        public void AddPointsToDriver(IParticipant driver, int counter)
        {
            if (Data.Comp.points.ContainsKey(driver))
            {
                Data.Comp.points[driver] += 10 - counter;
            }
            else
            {
                Data.Comp.points.Add(driver, 10 - counter);
            }
        }

        public void ClearEvents()
        {
            if (DriversChanged.GetInvocationList() != null)
            {
                foreach (var handler in DriversChanged.GetInvocationList())
                {
                    DriversChanged -= (EventHandler<DriversChangedEventArgs>)handler;
                }
            }
            if (NextRace.GetInvocationList() != null)
            {
                foreach (var handler in NextRace.GetInvocationList())
                {
                    NextRace -= (EventHandler<NextRaceEventsArgs>)handler;
                }
            }
        }

        public void Start()
        {
            _timer.Start();
            StartTime = DateTime.Now;
        }

        public void Stop()
        {
            _timer.Stop();
            NextRace?.Invoke(this, new NextRaceEventsArgs(Track));
        }

        public void EquipmentFailed()
        {
            foreach (var driver in Participants)
            {
                if (!driver.Equipment.IsBroken && _random.Next(0, 1000) < 40)
                {
                    driver.Equipment.IsBroken = true;
                    driver.Equipment.Speed = 0;
                    driver.Name = "!";
                    Data.Comp.equipment[driver] = $"{driver.Equipment.Speed}";
                    Data.Comp.timesBrokenDown[driver]++;
                }
            }
        }

        public void EquipmentRepaired()
        {
            foreach (var driver in Participants)
            {
                if (driver.Equipment.IsBroken && _random.Next(0, 7) > 0)
                {
                    driver.Equipment.IsBroken = false;
                    driver.Name = _initialName[driver];
                    int speed = driver.Equipment.Speed <= 50 ? 50 : _initialEquipment[driver] - 10;
                    driver.Equipment.Speed = speed;
                    Data.Comp.equipment[driver] = $"{speed}";
                }
            }
        }

        public void RandomizeEquipment()
        {
            foreach (var deelnemer in Participants)
            {
                int p = deelnemer.Equipment.Performance = _random.Next(17, 20);
                int q = deelnemer.Equipment.Quality = _random.Next(3, 5);
                if (!Data.Comp.equipment.ContainsKey(deelnemer)) Data.Comp.equipment.Add(deelnemer, $"{q * p}");
                if (!Data.Comp.timesBrokenDown.ContainsKey(deelnemer)) Data.Comp.timesBrokenDown.Add(deelnemer, 0);
                {

                }
                deelnemer.Equipment.Speed = q * p;
                deelnemer.Equipment.IsBroken = false;

                _initialEquipment.Add(deelnemer, q * p);
                _initialName.Add(deelnemer, deelnemer.Name);
            }
        }

        public void InitializeStartPositions()
        {
            Stack<Section> sections = new Stack<Section>();
            foreach (var entry in _positions)
            {
                if (entry.Key.SectionType == Section.SectionTypes.StartGrid)
                {
                    sections.Push(entry.Key);
                }
            }

            for (int i = sections.Count * 2; i < Participants.Count; i++)
            {
                Data.Comp.Participants.RemoveAt(i);
            }
            
            for (int i = 0; i < _positions.Select(x => x.Key.SectionType == Section.SectionTypes.StartGrid).Count(); i += 2)
            {
                try
                {
                    SectionData sd = GetSectionData(sections.Pop());
                    sd.Left = Participants[i];
                    registerRounds.Add(Participants[i], 0);
                    sd.Right = Participants[i+1];
                    registerRounds.Add(Participants[i + 1], 0);
                }
                catch (InvalidOperationException)
                {
                    return;
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
