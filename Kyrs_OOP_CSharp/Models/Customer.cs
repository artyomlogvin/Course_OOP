using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrs_OOP_CSharp
{
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Number { get; set; }
        public int IdComp { get; set; }
        public string InDate { get; set; }
        public string OutDate { get; set; }

        public Customer()
        {

        }

        public Customer(string name, string surname, string number, int idComp, string inDate, string outDate)
        {
            Name = name;
            Surname = surname;
            Number = number;
            IdComp = idComp;
            InDate = inDate;
            OutDate = outDate;
        }
    }
}
