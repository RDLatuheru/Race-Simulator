using System;
using Controller;
using Model;

namespace Race_Simulator
{
    public static class ConsoleVisualisatie
    {
        public static Direction _direction;
        public static int _x, _y, _size;

        #region graphics
        //  ═ ║   ╔   ╗   ╚  ╝
        private static string[] _finishHorizontal = {"════", " #1 ", " #2 ", "════"};
        private static string[] _finishVertical = {"║  ║", "║12║", "║##║", "║  ║"};
        private static string[] _startHorizontal = {"════", " *1 ", " *2 ", "════"};
        private static string[] _startVertical = {"║  ║", "║12║", "║**║", "║  ║"};
        private static string[] _straightHorizontal = {"════", " 1  ", " 2  ", "════"};
        private static string[] _straightVertical = {"║  ║", "║1 ║", "║ 2║", "║  ║"};
        private static string[] _corner1 = {"═══╗", "   ║", " 1 ║", "╗ 2║"};
        private static string[] _corner2 = {"║1 ╚", "║ 2 ", "║   ", "╚═══"};
        private static string[] _corner3 = {"╔═══", "║   ", "║  1", "║ 2╔"};
        private static string[] _corner4 = {"╝1 ║", "2  ║", "   ║", "═══╝"};
        

        #endregion

        public enum Direction
        {
            North, East, South, West
        }

        public static void Initialize()
        {
            Console.CursorVisible = false;
            _direction = Direction.East;
            _size = 4;
            Data.CurrentRace.DriversChanged += DriversChangedHandler;
            Data.CurrentRace.NextRace += NextRaceHandler;
        }
        
        public static void DrawTrack(Track t)
        {
            PreDrawTrack(t);
            
            foreach (Section section in t.Sections)
            {
                switch (section.SectionType)
                {
                    case Section.SectionTypes.StartGrid:
                        if (_direction == Direction.East || _direction == Direction.West)
                            GenerateSection(_startHorizontal, section);
                        else
                            GenerateSection(_startVertical, section); 
                        break;
                    case Section.SectionTypes.Straight:
                        if (_direction == Direction.East || _direction == Direction.West)
                            GenerateSection(_straightHorizontal, section);
                        else
                            GenerateSection(_straightVertical, section);
                        break;
                    case Section.SectionTypes.LeftCorner:
                        if (_direction == Direction.North)
                            GenerateSection(_corner1, section);
                        else if (_direction == Direction.East)
                            GenerateSection(_corner4, section);
                        else if (_direction == Direction.South)
                            GenerateSection(_corner2, section);
                        else
                            GenerateSection(_corner3, section);
                        _direction = SetNewDirection(Section.SectionTypes.LeftCorner, _direction);
                        break;
                    case Section.SectionTypes.RightCorner:
                        if (_direction == Direction.North)
                            GenerateSection(_corner3, section);
                        else if (_direction == Direction.East)
                            GenerateSection(_corner1, section);
                        else if (_direction == Direction.South)
                            GenerateSection(_corner4, section);
                        else
                            GenerateSection(_corner2, section);
                        _direction = SetNewDirection(Section.SectionTypes.RightCorner, _direction);
                        break;
                    case Section.SectionTypes.Finish:
                        if (_direction == Direction.East || _direction == Direction.West)
                            GenerateSection(_finishHorizontal, section);
                        else
                            GenerateSection(_finishVertical, section);
                        break;
                }

                switch (_direction)
                {
                    case Direction.North:
                        _y -= _size;
                        break;
                    case Direction.East:
                        _x += _size;
                        break;
                    case Direction.South:
                        _y += _size;
                        break;
                    case Direction.West:
                        _x -= _size;
                        break;
                }
                Console.SetCursorPosition(_x, _y);
            }
        }
        
        public static string DrawDrivers(string s, IParticipant d1, IParticipant d2)
        {
            if (d1 != null)
            {
                s = s.Replace("1", d1.Name.Substring(0, 1));
            }
            else
            {
                s = s.Replace("1", " ");
            }

            if (d2 != null)
            {
                s = s.Replace("2", d2.Name.Substring(0, 1));
            }
            else
            {
                
                s = s.Replace("2", " ");
            }
            
            return s;
        }

        public static void GenerateSection(string[] sectionStrings, Section section)
        {
            _y -= 4;
            IParticipant d1, d2;
            foreach (string s in sectionStrings)
            {
                string ss = s;
                d1 = Data.CurrentRace.GetSectionData(section).Left;
                d2 = Data.CurrentRace.GetSectionData(section).Right;
                ss = DrawDrivers(s, d1, d2);
                Console.SetCursorPosition(_x, _y);
                Console.WriteLine(ss);
                _y++;
            }
        }

        public static void PreDrawTrack(Track t)
        {
            int x, y;
            x = 0;
            y = 0;

            foreach (Section section in t.Sections)
            {
                if (section.SectionType == Section.SectionTypes.LeftCorner)
                {
                    _direction = SetNewDirection(section.SectionType, _direction);
                }else if (section.SectionType == Section.SectionTypes.RightCorner)
                {
                    _direction = SetNewDirection(section.SectionType, _direction);
                }

                switch (_direction)
                {
                    case Direction.North:
                        y -= _size;
                        break;
                    case Direction.East:
                        x += _size;
                        break;
                    case Direction.South:
                        y += _size;
                        break;
                    case Direction.West:
                        x -= _size;
                        break;
                }

                if (_x > x)
                {
                    _x = x;
                }

                if (_y > y)
                {
                    _y = y;
                }
            }
            Console.SetCursorPosition(-_x, -_y);
            _direction = Direction.East;
            _x = -_x;
            _y = -(_y - (_size + _size));
        }

        public static Direction SetNewDirection(Section.SectionTypes sectionType, Direction r)
        {
            switch (sectionType)
            {
                case Section.SectionTypes.LeftCorner:
                    return r == Direction.North ? Direction.West : r - 1;
                case Section.SectionTypes.RightCorner:
                    return r == Direction.West ? Direction.North : r + 1;
            }
            return Direction.East;
        }

        public static void DriversChangedHandler(object s, DriversChangedEventArgs e)
        {
            DrawTrack(e.Track);
        }

        public static void NextRaceHandler(object s, NextRaceEventsArgs e)
        {
            Data.CurrentRace.ClearEvents();
            Data.NextRace();
            if (Data.CurrentRace != null)
            {
                Initialize();
            }
        }
    }
}