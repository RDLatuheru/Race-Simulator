using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    public static class WpfVisualisatie
    {
        #region Graphics

        private const string dRed = "C://Users//rapha//Documents//School//Windesheim//Jaar 2//CS//Race Simulator//WpfApp//Visuals//carRed.png";
        private const string dGreen = "C://Users//rapha//Documents//School//Windesheim//Jaar 2//CS//Race Simulator//WpfApp//Visuals//carGreen.png";
        private const string dYellow = "C://Users//rapha//Documents//School//Windesheim//Jaar 2//CS//Race Simulator//WpfApp//Visuals//carYellow.png";
        private const string dGrey = "C://Users//rapha//Documents//School//Windesheim//Jaar 2//CS//Race Simulator//WpfApp//Visuals//carGrey.png";
        private const string dBlue = "C://Users//rapha//Documents//School//Windesheim//Jaar 2//CS//Race Simulator//WpfApp//Visuals//carBlue.png";

        private const string start = "C://Users//rapha//Documents//School//Windesheim//Jaar 2//CS//Race Simulator//WpfApp//Visuals//Start.png";
        private const string straight = "C://Users//rapha//Documents//School//Windesheim//Jaar 2//CS//Race Simulator//WpfApp//Visuals//Straight.png";
        private const string cornerRight = "C://Users//rapha//Documents//School//Windesheim//Jaar 2//CS//Race Simulator//WpfApp//Visuals//CornerRight.png";
        private const string cornerLeft = "C://Users//rapha//Documents//School//Windesheim//Jaar 2//CS//Race Simulator//WpfApp//Visuals//CornerLeft.png";
        private const string finish = "C://Users//rapha//Documents//School//Windesheim//Jaar 2//CS//Race Simulator//WpfApp//Visuals//Finish.png";

        #endregion

        private enum Direction
        {
            North, East, South, West
        }

        private static Direction _direction;
        private static int _width, _height, _x, _y;
        private const int _size = 64;


        public static BitmapSource DrawTrack(Track track)
        {
            if (track != null)
            {
                PreDrawTrack(track);
                Bitmap emptyImg = ImageProcessor.CreateBitmap(_width * _size, _height * _size);
                Bitmap trackImg = PlaceSections(track, emptyImg);
                Bitmap driverImg = PlaceDrivers(track, trackImg);

                return ImageProcessor.CreateBitmapSourceFromGdiBitmap(driverImg);
            }
            return ImageProcessor.CreateBitmapSourceFromGdiBitmap(ImageProcessor.CreateBitmap(256,256));
        }

        private static void DrawDriver(Bitmap bm, Direction r, int x, int xx, int y, int yy, Graphics g)
        {
            switch (r)
            {
                case Direction.North:
                    break;
                case Direction.East:
                    bm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Direction.South:
                    bm.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case Direction.West:
                    bm.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }
            g.DrawImage(bm, new Point(x * _size + xx, y * _size + yy));
        }

        private static Bitmap GenerateDriver(Bitmap bm, Direction r, int x, int y, Section section)
        {
            int xr, xl, yr, yl;
            xr = 0;
            xl = 0;
            yr = 0;
            yl = 0;

            switch (r)
            {
                case Direction.North:
                    xl = 10;
                    yl = 10;
                    xr = 40;
                    yr = 10;
                    break;
                case Direction.East:
                    xl = 10;
                    yl = 10;
                    xr = 10;
                    yr = 40;
                    break;
                case Direction.South:
                    xl = 40;
                    yl = 10;
                    xr = 10;
                    yr = 10;
                    break;
                case Direction.West:
                    xl = 10;
                    yl = 40;
                    xr = 10;
                    yr = 10;
                    break;
            }

            IParticipant d1 = Data.CurrentRace?.GetSectionData(section).Left;
            IParticipant d2 = Data.CurrentRace?.GetSectionData(section).Right;
            Graphics g = Graphics.FromImage(bm);
            if (d1 != null)
            {
                Bitmap d1Bitmap = new Bitmap(ImageProcessor.GetBitmap(GetDriverPath(d1.TeamColor)));
                DrawDriver(d1Bitmap, r, x, xl, y, yl, g);
            }
            if (d2 != null)
            {
                Bitmap d2Bitmap = new Bitmap(ImageProcessor.GetBitmap(GetDriverPath(d2.TeamColor)));
                DrawDriver(d2Bitmap, r, x, xr, y, yr, g);
            }

            return bm;
        }

        private static Bitmap PlaceDrivers(Track t, Bitmap bm)
        {
            int x = -_x;
            int y = -_y;
            _direction = Direction.East;

            foreach (Section section in t.Sections)
            {
                switch (section.SectionType)
                {
                    case Section.SectionTypes.StartGrid:
                        GenerateDriver(bm, _direction, x, y, section);
                        break;
                    case Section.SectionTypes.Straight:
                        GenerateDriver(bm, _direction, x, y, section);
                        break;
                    case Section.SectionTypes.LeftCorner:
                        GenerateDriver(bm, _direction, x, y, section);
                        SetNewDirection(Section.SectionTypes.LeftCorner);
                        break;
                    case Section.SectionTypes.RightCorner:
                        GenerateDriver(bm, _direction, x, y, section);
                        SetNewDirection(Section.SectionTypes.RightCorner);
                        break;
                    case Section.SectionTypes.Finish:
                        GenerateDriver(bm, _direction, x, y, section);
                        break;
                }

                switch (_direction)
                {
                    case Direction.North:
                        y--;
                        break;
                    case Direction.East:
                        x++;
                        break;
                    case Direction.South:
                        y++;
                        break;
                    case Direction.West:
                        x--;
                        break;
                }
            }

            return bm;
        }

        public static string GetDriverPath(IParticipant.TeamColors c)
        {
            switch (c)
            {
                case IParticipant.TeamColors.Blue:
                    return dBlue;
                case IParticipant.TeamColors.Green:
                    return dGreen;
                case IParticipant.TeamColors.Grey:
                    return dGrey;
                case IParticipant.TeamColors.Yellow:
                    return dYellow;
                case IParticipant.TeamColors.Red:
                    return dRed;
            }
            return "";
        }

        public static void PreDrawTrack(Track t)
        {
            int x, y, xx, yy;
            x = 0;
            y = 0;
            xx = 0;
            yy = 0;

            _direction = Direction.East;
            foreach (Section section in t.Sections)
            {
                if (section.SectionType == Section.SectionTypes.LeftCorner)
                {
                    SetNewDirection(section.SectionType);
                }
                else if (section.SectionType == Section.SectionTypes.RightCorner)
                {
                    SetNewDirection(section.SectionType);
                }

                switch (_direction)
                {
                    case Direction.North:
                        y--;
                        break;
                    case Direction.East:
                        x++;
                        break;
                    case Direction.South:
                        y++;
                        break;
                    case Direction.West:
                        x--;
                        break;
                }

                if (yy < y)
                {
                    yy = y;
                }
                else if (_y > y)
                {
                    _y = y;
                }

                if (xx < x)
                {
                    xx = x;
                }
                else if (_x > x)
                {
                    _x = x;
                }
            }

            _direction = Direction.East;
            _width = xx - _x + 1;
            _height = yy - _y + 1;
        }

        public static Bitmap PlaceSections(Track t, Bitmap bitmap)
        {
            int x = -_x;
            int y = -_y;
            Graphics graphics = Graphics.FromImage(bitmap);

            foreach (Section section in t.Sections)
            {
                switch (section.SectionType)
                {
                    case Section.SectionTypes.StartGrid:
                        DrawSection(ImageProcessor.GetBitmap(start), _direction, x, y, graphics);
                        break;
                    case Section.SectionTypes.Straight:
                        DrawSection(ImageProcessor.GetBitmap(straight), _direction, x, y, graphics);
                        break;
                    case Section.SectionTypes.LeftCorner:
                        DrawSection(ImageProcessor.GetBitmap(cornerLeft), _direction, x, y, graphics);
                        SetNewDirection(Section.SectionTypes.LeftCorner);
                        break;
                    case Section.SectionTypes.RightCorner:
                        DrawSection(ImageProcessor.GetBitmap(cornerRight), _direction, x, y, graphics);
                        SetNewDirection(Section.SectionTypes.RightCorner);
                        break;
                    case Section.SectionTypes.Finish:
                        DrawSection(ImageProcessor.GetBitmap(finish), _direction, x, y, graphics);
                        break;
                }

                switch (_direction)
                {
                    case Direction.North:
                        y--;
                        break;
                    case Direction.East:
                        x++;
                        break;
                    case Direction.South:
                        y++;
                        break;
                    case Direction.West:
                        x--;
                        break;
                }
            }

            _direction = Direction.East;
            return bitmap;
        }

        private static void DrawSection(Bitmap bitmap, Direction r, int x, int y, Graphics g)
        {
            Bitmap bm = new Bitmap(bitmap);

            switch (r)
            {
                case Direction.North:
                    g.DrawImage(bm, new Point(x * _size, y * _size));
                    break;
                case Direction.East:
                    bm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    g.DrawImage(bm, new Point(x * _size, y * _size));
                    break;
                case Direction.South:
                    bm.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(bm, new Point(x * _size, y * _size));
                    break;
                case Direction.West:
                    bm.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    g.DrawImage(bm, new Point(x * _size, y * _size));
                    break;
            }
        }

        private static void SetNewDirection(Section.SectionTypes sectionType)
        {
            switch (sectionType)
            {
                case Section.SectionTypes.LeftCorner:
                    _direction = _direction == Direction.North ? Direction.West : _direction - 1;
                    break;
                case Section.SectionTypes.RightCorner:
                    _direction = _direction == Direction.West ? Direction.North : _direction + 1;
                    break;
            }
        }
    }
}
