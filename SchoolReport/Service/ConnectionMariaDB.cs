using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolReport.Service
{
    public class ConnectionMariaDB : IDatabse
    {
        private MySqlConnection connection;
        private MySqlCommand command;
        private readonly string user = "root";
        private readonly string password = "1234";
        private readonly string host = "localhost";
        private readonly string database = "mariadb";

        public ConnectionMariaDB()
        {
            string connectionString = $"Server={host};Database={database};Uid={user};Pwd={password};";
            connection = new MySqlConnection(connectionString);
            command = connection.CreateCommand();
            command.Connection = connection;
        }

        public bool CreateDatabase()
        {
            try
            {
                connection.Open();
                command.CommandText = "DROP TABLE IF EXISTS enrollment; DROP TABLE IF EXISTS student; DROP TABLE IF EXISTS discipline;";
                command.ExecuteNonQuery();

                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS discipline (
                        cod INT AUTO_INCREMENT PRIMARY KEY,
                        name TEXT UNIQUE
                    );

                    CREATE TABLE IF NOT EXISTS student (
                        studentRegistration CHAR(36) DEFAULT (UUID()) PRIMARY KEY,
                        name TEXT,
                        email TEXT UNIQUE
                    );

                    CREATE TABLE IF NOT EXISTS enrollment (
                        discipline_id INT REFERENCES discipline(cod),
                        student_id CHAR(36) REFERENCES student(studentRegistration) ON DELETE CASCADE,
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
            }
            finally { connection.Close(); }
            return true;
        }

        public bool InsertStudent(Student student)
        {
            try
            {
                string commandString = $"insert into student (name, email) values ('{student.Name}', '{student.Email}');";
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
                string commandStringEnrollment = $"INSERT INTO enrollment (discipline_id, student_id, grade) VALUES ({codDiscipline}, '{student.StudentRegistration}', {discipline.Grade});";
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
                string commandString = $"select * from student ORDER BY name;";
                command.CommandText = commandString;

                connection.Open();
                MySqlDataReader dr = command.ExecuteReader();

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
        public int SelectDisciplineByName(string name)
        {
            int findedCod = -1;
            try
            {
                string commandString = $"select * from discipline WHERE name = '{name}';";
                command.CommandText = commandString;

                connection.Open();
                MySqlDataReader dr = command.ExecuteReader();

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
        public List<Discipline> SelectDisciplineByStudent(Student student)
        {
            List<Discipline> listDisciplines = new List<Discipline>();
            try
            {
                string commandString = "select d.cod, d.name, e.grade " +
                                         "from student s inner join enrollment e on e.student_id = s.studentregistration " +
                                         "inner join discipline d on d.cod = e.discipline_id " +
                                         $"where '{student.StudentRegistration}' = e.student_id";

                command.CommandText = commandString;

                connection.Open();
                MySqlDataReader dr = command.ExecuteReader();

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
                string commandString = $"update student set name = '{student.Name}', email = '{student.Email}' where studentRegistration = '{student.StudentRegistration}';";
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
                string commandString = $"update discipline set name = '{discipline.Name}', grade = {discipline.Grade.ToString().Replace(',', '.')} where cod = {discipline.Cod}";
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
                string commandString = $"update enrollment set grade = '{discipline.Grade.ToString().Replace(',', '.')}' where discipline_id = {discipline.Cod} AND student_id = '{student.StudentRegistration}'";
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

        public List<Discipline> SelectDiscipline()
        {
            List<Discipline> listDisciplines = new List<Discipline>();
            try
            {
                string commandString = $"select * from discipline;";

                command.CommandText = commandString;

                connection.Open();
                MySqlDataReader dr = command.ExecuteReader();

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
                MySqlDataReader dr = command.ExecuteReader();

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
    }
}
