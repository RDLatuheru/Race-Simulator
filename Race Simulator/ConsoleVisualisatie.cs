using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Race_Simulator
{
    public static class ConsoleVisualisatie
    {
        static int richting;
        public static int X { get; set; }
        public static int Y { get; set; }

        #region graphics
        //  ═ ║   ╔   ╗   ╚  ╝
        private static string[] _finishHorizontal = {"════", " #  ", " #  ", "════"};
        private static string[] _finishVertical = {"║  ║", "║  ║", "║##║", "║  ║"};
        private static string[] _startHorizontal = {"════", " *  ", " *  ", "════"};
        private static string[] _startVertical = {"║  ║", "║  ║", "║**║", "║  ║"};
        private static string[] _straightHorizontal = {"════", "    ", "    ", "════"};
        private static string[] _straightVertical = {"║  ║", "║  ║", "║  ║", "║  ║"};
        private static string[] _corner1 = {"═══╗", "   ║", "   ║", "╗  ║"};
        private static string[] _corner2 = {"║  ╚", "║   ", "║   ", "╚═══"};
        private static string[] _corner3 = {"╔═══", "║   ", "║   ", "║  ╔"};
        private static string[] _corner4 = {"╝  ║", "   ║", "   ║", "═══╝"};
        

        #endregion
        
        public static void Initialize()
        {
            richting = 1;
        }

        public static void DrawTrack(Track t)
        {
            CalculateCursorPos(t);
            foreach (Section section in t.Sections)
            {
                switch (section.SectionType)
                {
                    case Section.SectionTypes.StartGrid:
                        if (richting == 1 || richting == 3)
                            GenerateSection(_startHorizontal);
                        else
                            GenerateSection(_startVertical); 
                        break;
                    case Section.SectionTypes.Straight:
                        if (richting == 1 || richting == 3)
                            GenerateSection(_straightHorizontal);
                        else
                            GenerateSection(_straightVertical);
                        break;
                    case Section.SectionTypes.LeftCorner:
                        if (richting == 0)
                            GenerateSection(_corner1);
                        else if (richting == 1)
                            GenerateSection(_corner4);
                        else if (richting == 2)
                            GenerateSection(_corner2);
                        else
                            GenerateSection(_corner3);
                        BepaalRichting(Section.SectionTypes.LeftCorner);
                        break;
                    case Section.SectionTypes.RightCorner:
                        if (richting == 0)
                            GenerateSection(_corner3);
                        else if (richting == 1)
                            GenerateSection(_corner1);
                        else if (richting == 2)
                            GenerateSection(_corner4);
                        else
                            GenerateSection(_corner2);
                        BepaalRichting(Section.SectionTypes.RightCorner);
                        break;
                    case Section.SectionTypes.Finish:
                        if (richting == 1 || richting == 3)
                            GenerateSection(_finishHorizontal);
                        else
                            GenerateSection(_finishVertical);
                        break;
                }

                switch (richting)
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

        public static void CalculateCursorPos(Track t)
        {
            int x, y, xx, yy;
            x = y = xx = yy = 0;

            foreach (Section section in t.Sections)
            {
                if (section.SectionType == Section.SectionTypes.LeftCorner)
                {
                    BepaalRichting(section.SectionType);
                }else if (section.SectionType == Section.SectionTypes.RightCorner)
                {
                    BepaalRichting(section.SectionType);
                }

                switch (richting)
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
            richting = 1;
            X = -xx;
            Y = -(yy - 5);
        }

        private static void BepaalRichting(Section.SectionTypes sectionType)
        {
            switch (sectionType)
            {
                case Section.SectionTypes.LeftCorner:
                    if (richting < 1)
                        richting = 3;
                    else
                        richting -= 1;
                    break;
                case Section.SectionTypes.RightCorner:
                    if (richting > 2)
                        richting = 0;
                    else
                         richting += 1;
                    break;
                default:
                    richting = -1;
                    break;
            }
        }
    }
}