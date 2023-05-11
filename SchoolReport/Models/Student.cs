using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SchoolReport
{
    public class Student : PropertyChangedBase
    {
        private readonly string studentRegistration;
        private string name;
        private string email;
        private ObservableCollection<Discipline> disciplines = new ObservableCollection<Discipline>();

        public Student() { }

        public Student(string name)
        {
            this.name = name;
        }

        public Student(string name, string email)
        {
            this.name = name;
            this.email = email;
        }

        public Student(string name, string email, string studentRegistration)
        {
            this.studentRegistration = studentRegistration;
            this.name = name;
            this.email = email;
        }

        public string StudentRegistration
        {
            get { return studentRegistration; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; Notify(nameof(Name)); }
        }

        public string Email
        {
            get { return email; }
            set { email = value; Notify(nameof(Email)); }
        }

        public ObservableCollection<Discipline> Disciplines
        {
            get { return disciplines; }
            set { disciplines = value; Notify(nameof(Disciplines)); }
        }

        public void AddDiscipline(Discipline discipline)
        {
            disciplines.Add(discipline);
        }

        public Student ShallowCopy()
        {
            return (Student)this.MemberwiseClone();
        }
    }
}
