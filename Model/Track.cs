using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public Track(string name, Section.SectionTypes[] sections)
        {
            Sections = new LinkedList<Section>();
            for (int i = 0; i < sections.Length; i++)
            {
                Sections.AddLast(new Section(sections[i]));
            }
            Name = name;
        }
    }
}
