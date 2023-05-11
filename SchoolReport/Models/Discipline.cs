using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SchoolReport
{
    public class Discipline: PropertyChangedBase
    {
        private readonly int cod;
        private string name;
        private double grade;
           
        public Discipline() { }

        public Discipline(string name)
        {
            this.name = name;
        }

        public Discipline(string name, double grade)
        {
            this.name = name;
            this.grade = grade;
        }

        public Discipline(int cod,string name, double grade)
        {
            this.cod = cod;
            this.name = name;
            this.grade = grade;
        }

        public int Cod
        {
            get { return cod; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; Notify(nameof(Name)); }
        }

        public double Grade
        {
            get { return grade; }
            set { grade = value; Notify(nameof(Grade)); }
        }

        public Discipline ShallowCopy()
        {
            return (Discipline)this.MemberwiseClone();
        }

    }
}
