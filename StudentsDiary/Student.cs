using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDiary
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Math { get; set; }
        public string Technology { get; set; }
        public string Physics { get; set; }
        public string PolishLang { get; set; }
        public string ForeighLang { get; set; }
        public string Remarks { get; set; }
        private string _aditionalActivities;
        public string AditionalActivities 
        {
            get
            {
                if (_aditionalActivities=="TAK")
                    return "TAK";
                else
                    return "NIE";
            }
            set
            {
                _aditionalActivities = value;
            } 
        }
    }
}
