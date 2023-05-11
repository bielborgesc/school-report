using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolReport.Service
{
    public interface IDatabse
    {
        bool InsertStudent(Student student);
        bool InsertDiscipline(Discipline discipline);
        bool InsertDisciplineInStudent(int codDiscipline, Discipline discipline, Student student);
        List<Student> SelectStudentsWIthDiciplines();
        int SelectDisciplineByName(string name);
        List<Discipline> SelectDisciplineByStudent(Student student);
        bool UpdateStudent(Student student);
        bool UpdateDiscipline(Discipline discipline);
        bool UpdateGrade(Discipline discipline, Student student);
        bool UpdateGrade(Discipline discipline, Student student, int codDiscipline);
        bool DeleteStudent(Student student);
        bool DeleteDiscipline( Discipline discipline );
        bool DeleteEnrollmentByStudent(Student student);
        bool DeleteDisciplineOfStudent(Discipline discipline, Student student);
        List<Student> SelectStudent();
        List<Discipline> SelectDiscipline();
    }
}
