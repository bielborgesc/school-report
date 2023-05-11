using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolReport.Service
{
    public class DataBaseManager : IDatabse
    {
        private readonly IDatabse connection;
        public DataBaseManager(IDatabse databse)
        {
            connection = databse;
        }
        public bool DeleteDisciplineOfStudent(Discipline discipline, Student student)
        {
            return connection.DeleteDisciplineOfStudent(discipline, student);
        }

        public bool DeleteDiscipline(Discipline discipline)
        {
            return connection.DeleteDiscipline(discipline);
        }

        public bool DeleteEnrollmentByStudent(Student student)
        {
            return DeleteEnrollmentByStudent(student);
        }

        public bool DeleteStudent(Student student)
        {
            return connection.DeleteStudent(student);
        }

        public bool InsertDiscipline(Discipline discipline)
        {
            return connection.InsertDiscipline(discipline);
        }

        public bool InsertDisciplineInStudent(int codDiscipline, Discipline discipline, Student student)
        {
            return connection.InsertDisciplineInStudent(codDiscipline, discipline, student);
        }

        public bool InsertStudent(Student student)
        {
            return connection.InsertStudent(student);
        }

        public int SelectDisciplineByName(string name)
        {
            return connection.SelectDisciplineByName(name);
        }

        public List<Discipline> SelectDisciplineByStudent(Student student)
        {
            return connection.SelectDisciplineByStudent(student);
        }

        public bool UpdateDiscipline(Discipline discipline)
        {
            return connection.UpdateDiscipline(discipline);
        }

        public bool UpdateGrade(Discipline discipline, Student student)
        {
            return connection.UpdateGrade(discipline, student);
        }

        public bool UpdateGrade(Discipline discipline, Student student, int codDiscipline)
        {
            return connection.UpdateGrade(discipline, student, codDiscipline);
        }

        public bool UpdateStudent(Student student)
        {
            return connection.UpdateStudent(student);
        }

        public bool StudentAddDiscipline(Discipline discipline, Student student)
        {
            int codDisciplina = connection.SelectDisciplineByName(discipline.Name);
            if (codDisciplina == -1)
            {
                connection.InsertDiscipline(discipline);
                codDisciplina = connection.SelectDisciplineByName(discipline.Name);
            }
            bool hasDiscipline = VerifyStudentDisciplineList(student, discipline);
            if (hasDiscipline)
            {
                return connection.UpdateGrade(discipline, student, codDisciplina);
            }
            return connection.InsertDisciplineInStudent(codDisciplina, discipline, student);
        }

        private bool VerifyStudentDisciplineList(Student student, Discipline discipline)
        {
            ObservableCollection<Discipline> disciplines = student.Disciplines;
            if (disciplines.Count > 0)
            {
                foreach (Discipline thisDiscipline in disciplines)
                {
                    if (thisDiscipline.Name.Equals(discipline.Name)) { return true; }
                }
            }
            return false;
        }

        public List<Student> SelectStudentsWIthDiciplines()
        {
            List<Student> listStudents = new List<Student>(connection.SelectStudent());
            if (listStudents.Count > 0)
            {
                foreach (Student student in listStudents)
                {
                    student.Disciplines = new ObservableCollection<Discipline>(connection.SelectDisciplineByStudent(student));
                }
            }
            return listStudents;
        }

        public List<Student> SelectStudent()
        {
            return connection.SelectStudent();
        }

        public List<Discipline> SelectDiscipline()
        {
            return connection.SelectDiscipline();
        }
    }
}
