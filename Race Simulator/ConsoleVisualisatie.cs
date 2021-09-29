using System;
using Model;

namespace Race_Simulator
{
    public static class ConsoleVisualisatie
    {
        static int _richting;
        public static int X { get; set; }
        public static int Y { get; set; }

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
        
        public static void Initialize()
        {
            _richting = 1;
        }

        public static string DrawDriver(String s, IParticipant d1, IParticipant d2)
        {
            s = s.Replace("1", d1.Name.Substring(0, 1));
            s = s.Replace("2", d2.Name.Substring(0, 1));
            
            return s;
        }

        public static void DrawTrack(Track t)
        {
            PreDrawTrack(t);
            foreach (Section section in t.Sections)
            {
                switch (section.SectionType)
                {
                    case Section.SectionTypes.StartGrid:
                        if (_richting == 1 || _richting == 3)
                            GenerateSection(_startHorizontal);
                        else
                            GenerateSection(_startVertical); 
                        break;
                    case Section.SectionTypes.Straight:
                        if (_richting == 1 || _richting == 3)
                            GenerateSection(_straightHorizontal);
                        else
                            GenerateSection(_straightVertical);
                        break;
                    case Section.SectionTypes.LeftCorner:
                        if (_richting == 0)
                            GenerateSection(_corner1);
                        else if (_richting == 1)
                            GenerateSection(_corner4);
                        else if (_richting == 2)
                            GenerateSection(_corner2);
                        else
                            GenerateSection(_corner3);
                        BepaalRichting(Section.SectionTypes.LeftCorner);
                        break;
                    case Section.SectionTypes.RightCorner:
                        if (_richting == 0)
                            GenerateSection(_corner3);
                        else if (_richting == 1)
                            GenerateSection(_corner1);
                        else if (_richting == 2)
                            GenerateSection(_corner4);
                        else
                            GenerateSection(_corner2);
                        BepaalRichting(Section.SectionTypes.RightCorner);
                        break;
                    case Section.SectionTypes.Finish:
                        if (_richting == 1 || _richting == 3)
                            GenerateSection(_finishHorizontal);
                        else
                            GenerateSection(_finishVertical);
                        break;
                }

                switch (_richting)
                {
                    case 0:
                        Y -= 4;
                        break;
                    case 1:
                        X += 4;
                        break;
                    case 2:
                        Y += 4;
                        break;
                    case 3:
                        X -= 4;
                        break;
                }
                Console.SetCursorPosition(X, Y);
            }
        }

        public static void GenerateSection(string[] section)
        {
            Y -= 4;
            foreach (string s in section)
            {
                Console.SetCursorPosition(X, Y);
                Console.WriteLine(s);
                Y++;
            }
        }

        public static void PreDrawTrack(Track t)
        {
            int x, y, xx, yy;
            x = 0;
            y = 0;
            xx = 0;
            yy = 0;

            foreach (Section section in t.Sections)
            {
                if (section.SectionType == Section.SectionTypes.LeftCorner)
                {
                    BepaalRichting(section.SectionType);
                }else if (section.SectionType == Section.SectionTypes.RightCorner)
                {
                    BepaalRichting(section.SectionType);
                }

                switch (_richting)
                {
                    case 0:
                        y -= 4;
                        break;
                    case 1:
                        x += 4;
                        break;
                    case 2:
                        y += 4;
                        break;
                    case 3:
                        x -= 4;
                        break;
                }

                if (xx > x)
                {
                    xx = x;
                }

                if (yy > y)
                {
                    yy = y;
                }
            }
            Console.SetCursorPosition(-xx, -yy);
            _richting = 1;
            X = -xx;
            Y = -(yy - 4);
        }

        private static void BepaalRichting(Section.SectionTypes sectionType)
        {
            switch (sectionType)
            {
                case Section.SectionTypes.LeftCorner:
                    if (_richting == 0)
                        _richting = 3;
                    else
                        _richting -= 1;
                    break;
                case Section.SectionTypes.RightCorner:
                    if (_richting == 3)
                        _richting = 0;
                    else
                         _richting += 1;
                    break;
            }
        }
    }
}