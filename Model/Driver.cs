using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public IParticipant.TeamColors TeamColor { get; set; }

        public Driver(string name, IEquipment equipment, IParticipant.TeamColors teamcolor)
        {
            Name = name;
            Points = 0;
            Equipment = equipment;
            TeamColor = teamcolor;
        }
    }
}
