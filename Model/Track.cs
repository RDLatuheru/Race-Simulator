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
            Name = name;
            Sections = InitializeSections(sections);
        }

        private LinkedList<Section> InitializeSections(Section.SectionTypes[] sections)
        {
            LinkedList<Section> sectionList = new LinkedList<Section>();
            for (int i = 0; i < sections.Length; i++)
            {
                sectionList.AddLast(new Section(sections[i]));
            }
            return sectionList;
        }
    }
}
