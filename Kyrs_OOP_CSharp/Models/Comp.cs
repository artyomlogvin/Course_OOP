using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrs_OOP_CSharp
{
    public class Comp
    {
        public int Id { get; set; }
        public string NameComp { get; set; }
        public string Place { get; set; }
        public string OrgSurname { get; set; }
        public string Age { get; set; }

        public Comp()
        {
            NameComp = "";
            Place = "";
            OrgSurname = "";
            Age = "";
        }

        public Comp(int id, string nameComp, string place, string orgSurname, string age)
        {
            Id = id;
            NameComp = nameComp;
            Place = place;
            OrgSurname = orgSurname;
            Age = age;
        }

        public Comp(string nameComp, string place, string orgSurname, string age)
        {
            NameComp = nameComp;
            Place = place;
            OrgSurname = orgSurname;
            Age = age;
        }
    }
}
