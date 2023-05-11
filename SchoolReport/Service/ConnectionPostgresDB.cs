using Npgsql;
using SchoolReport.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;

namespace SchoolReport
{
    public class ConnectionPostgresDB :  IDatabse
    {
        private NpgsqlConnection connection;
        private NpgsqlCommand command;
        private readonly string user = "postgres";
        private readonly string password = "1234";
        private readonly string host = "localhost";
        private readonly string port = "5555";
        private readonly string database = "postgres";

        public ConnectionPostgresDB() 
        {
            connection = new NpgsqlConnection($"User ID={user};Password={password};Host={host};Port={port};Database={database};");
            command = new NpgsqlCommand();
            command.Connection = connection;
        }

        public bool CreateDatabase()
        {
            try
            {
                connection.Open();
                command.CommandText = "DROP TABLE IF EXISTS Enrollment; DROP TABLE IF EXISTS Student; DROP TABLE IF EXISTS Discipline;";
                command.ExecuteNonQuery();

                command.CommandText = @"
                    CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";
                    CREATE TABLE Discipline (
                        cod SERIAL PRIMARY KEY,
                        name TEXT UNIQUE
                    );
                    CREATE TABLE Student (
                        studentRegistration uuid DEFAULT uuid_generate_v4() PRIMARY KEY,
                        name TEXT,
                        email TEXT UNIQUE
                    );
                    CREATE TABLE Enrollment (
                        discipline_id INTEGER REFERENCES Discipline(cod),
                        student_id uuid REFERENCES Student(studentRegistration)  ON DELETE CASCADE,
                        grade DOUBLE PRECISION,
                        PRIMARY KEY(discipline_id, student_id)
                    );";
                command.ExecuteNonQuery();

                Console.WriteLine("Banco de dados criado com sucesso.");
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Erro ao criar o banco de dados: " + ex.Message);
                return false;
            } finally { connection.Close(); }
            return true;
        }

        public bool InsertStudent(Student student)
        {
            try
            {
                string commandString = $"insert into student (name, email) VALUES ('{student.Name}', '{student.Email}')";
                command.CommandText = commandString;

                connection.Open();
                int lines = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }
        public bool InsertDiscipline(Discipline discipline)
        {
            try
            {
                string commandString = $"insert into discipline (name) values ('{discipline.Name}')";
                command.CommandText = commandString;
                
                connection.Open();
                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }
        public bool InsertDisciplineInStudent(int codDiscipline, Discipline discipline, Student student)
        {
            try
            {
                connection.Open();
                string commandStringEnrollment = $"insert into enrollment (discipline_id, student_id, grade) values ({codDiscipline}, '{student.StudentRegistration}', {discipline.Grade})";
                command.CommandText = commandStringEnrollment;
                int lines = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }
        
        public List<Student> SelectStudentsWIthDiciplines()
        {
            List<Student> listStudents = new List<Student>();
            try
            {
                string commandString = $"Select * from student order by name";
                command.CommandText = commandString;

                connection.Open();
                NpgsqlDataReader dr = command.ExecuteReader();

                if (dr.HasRows)
                {
                    FillStudentList(dr, listStudents);
                }

            } catch (Exception ex) { MessageBox.Show(ex.Message ); 
            } finally { connection.Close(); }
            return listStudents;
        }
        public int SelectDisciplineByName(string name)
        {
            int findedCod = -1;
            try
            {
                string commandString = $"select * from discipline where name = '{name}'";
                command.CommandText = commandString;

                connection.Open();
                NpgsqlDataReader dr = command.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        findedCod = dr.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
            return findedCod;
        }

        public List<Discipline> SelectDiscipline()
        {
            List<Discipline> listDisciplines = new List<Discipline>();
            try
            {
                string commandString = $"select * from discipline;";

                command.CommandText = commandString;

                connection.Open();
                NpgsqlDataReader dr = command.ExecuteReader();

                if (dr.HasRows)
                {
                    FillDisciplineList(dr, listDisciplines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
            return listDisciplines;
        }

        public List<Student> SelectStudent()
        {
            List<Student> listStudents = new List<Student>();
            try
            {
                string commandString = $"select * from student;";

                command.CommandText = commandString;

                connection.Open();
                NpgsqlDataReader dr = command.ExecuteReader();

                if (dr.HasRows)
                {
                    FillStudentList(dr, listStudents);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
            return listStudents;
        }

        public List<Discipline> SelectDisciplineByStudent(Student student)
        {
            List<Discipline> listDisciplines = new List<Discipline>();
            try
            {
                string commandString = $"select d.cod, d.name, e.grade " +
                                       $"from student s inner join enrollment e on e.student_id = s.studentregistration " +
                                       $"inner join discipline d on d.cod = e.discipline_id " +
                                       $"where '{student.StudentRegistration}' = e.student_id;";

                command.CommandText = commandString;

                connection.Open();
                NpgsqlDataReader dr = command.ExecuteReader();

                if (dr.HasRows)
                {
                    FillDisciplineList(dr, listDisciplines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
            return listDisciplines;
        }

        public bool UpdateStudent(Student student)
        {
            try
            {
                string commandString = $"update student set name = '{student.Name}', email = '{student.Email}' where studentRegistration = '{student.StudentRegistration}'";
                command.CommandText = commandString;

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }
        public bool UpdateDiscipline(Discipline discipline)
        {
            try
            {
                string commandString = $"update discipline set name = '{discipline.Name}', grade = {discipline.Grade.ToString().Replace(',','.')} where cod = {discipline.Cod}";
                command.CommandText = commandString;

                connection.Open();
                int lines = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }
        public bool UpdateGrade(Discipline discipline, Student student)
        {
            try
            {
                string commandString = $"update enrollment set grade = '{discipline.Grade.ToString().Replace(',','.')}' where discipline_id = {discipline.Cod} AND student_id = '{student.StudentRegistration}'";
                command.CommandText = commandString;

                connection.Open();
                int lines = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }
        public bool UpdateGrade(Discipline discipline, Student student, int codDiscipline)
        {
            try
            {
                string commandString = $"update enrollment set grade = '{discipline.Grade.ToString().Replace(',', '.')}' where discipline_id = {codDiscipline} AND student_id = '{student.StudentRegistration}'";
                command.CommandText = commandString;

                connection.Open();
                int lines = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }

        public bool DeleteStudent(Student student)
        {
            try
            {
                string commandStringStudent = $"delete from student where studentRegistration = '{student.StudentRegistration}'";
                command.CommandText = commandStringStudent;

                connection.Open();
                int linesStudent = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }
        public bool DeleteEnrollmentByStudent(Student student)
        {
            try
            {
                string commandStringEnrollment = $"delete from enrollment where student_id = '{student.StudentRegistration}'";
                command.CommandText = commandStringEnrollment;

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }
        public bool DeleteDisciplineOfStudent(Discipline discipline, Student student)
        {
            try
            {
                string commandStringEnrollment = $"delete from enrollment where discipline_id = '{discipline.Cod}' AND student_id = '{student.StudentRegistration}' ";
                command.CommandText = commandStringEnrollment;

                connection.Open();
                int lines = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }
        
        public bool DeleteDiscipline(Discipline discipline)
        {
            try
            {
                string commandStringStudent = $"delete from discipline where cod = '{discipline.Cod}'";
                command.CommandText = commandStringStudent;

                connection.Open();
                int linesStudent = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally { connection.Close(); }
            return true;
        }

        private void FillDisciplineList(IDataReader dr, List<Discipline> listReturn)
        {
            while (dr.Read())
            {
                listReturn.Add(new Discipline(
                        dr.GetInt32(0),
                        dr.GetString(1),
                        dr.GetDouble(2)
                    )
                );
            }
        }
        private void FillStudentList(IDataReader dr, List<Student> listReturn)
        {
            while (dr.Read())
            {
                Student student = new Student(
                                        dr.GetString(1),
                                        dr.GetString(2),
                                        dr.GetGuid(0).ToString()
                                    );
                listReturn.Add(student);
            }
        }

    }
}