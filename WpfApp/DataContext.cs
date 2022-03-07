using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfApp
{
    public class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string TrackName => Data.CurrentRace?.Track.Name;
        public List<string> Equipment => Data.Comp.equipment.Select(x => $"{x.Key.Name} - Speed: {x.Value}").ToList();
        public List<string> Points => Data.Comp.points.Select(x => $"{x.Key.Name} - Points: {x.Value}").ToList();
        public string NextTrack => Data.Comp.Tracks.Count != 0 ? Data.Comp.Tracks.Peek().Name : "N/A";
        public string StartTime => Data.CurrentRace?.StartTime.ToString("dddd, dd MMMM HH:mm:ss");
        public List<string> TimesBrokenDown => Data.Comp.timesBrokenDown.Select(x => $"{x.Key.Name} - broken down: {x.Value}x").ToList();


        public DataContext()
        {
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += DriversChangedHandler;
            }
        }

        public void DriversChangedHandler(object s, DriversChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
