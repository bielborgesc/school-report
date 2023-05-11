using Moq;
using SchoolReport.Service;
using System.Collections.ObjectModel;

namespace TestSchoolReport
{
    public class Tests
    {
        Mock<IDatabse> mock;
        Student student;
        Discipline discipline;
        List<Student> listStudent;

        [SetUp]
        public void Setup()
        {
            mock = new Mock<IDatabse>();
            student = new Student("Gabriel", "gabriel@aluno.com", "2a3d7c79-ef4a-11ed-9f83-0242ac110002");
            discipline = new Discipline(1, "Matematica", 10);
            listStudent = new List<Student>
            {
                new Student("Tiago", "tiago@email.com", "2a3d7c79-ef4a-11ed-9f83-0242ac110035")
            };
            student.Disciplines.Add(discipline);
            listStudent.Add(student);
        }

        [Test]
        public void TestInsertStudent()
        {
            
            mock.Setup(x => x.InsertStudent(student)).Returns(true);

            DataBaseManager server = new DataBaseManager(mock.Object);
            bool result = server.InsertStudent(student);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestInsertDiscipline()
        {
            mock.Setup(x => x.InsertDiscipline(discipline)).Returns(true);

            DataBaseManager server = new DataBaseManager(mock.Object);
            bool result = server.InsertDiscipline(discipline);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestUpdateStudent()
        {
            student.Name = "Borges";
            mock.Setup(x => x.UpdateStudent(student)).Returns(true);

            DataBaseManager server = new DataBaseManager(mock.Object);
            bool result = server.UpdateStudent(student);

            Assert.IsTrue(result);
        }

        [Test]
       public void TestUpdateDiscipline()
        {
            mock.Setup(db => db.UpdateDiscipline(discipline)).Returns(true);

            DataBaseManager dbService = new DataBaseManager(mock.Object);

            var result = dbService.UpdateDiscipline(discipline);
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestDeleteStudent()
        {
            mock.Setup(x => x.DeleteStudent(student)).Returns(true);

            DataBaseManager server = new DataBaseManager(mock.Object);
            bool result = server.DeleteStudent(student);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestDeleteDiscipline()
        {
            mock.Setup(x => x.DeleteDiscipline(discipline)).Returns(true);

            DataBaseManager server = new DataBaseManager(mock.Object);
            bool result = server.DeleteDiscipline(discipline);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestListStudent()
        {
            mock.Setup(x => x.SelectStudentsWIthDiciplines()).Returns(listStudent);

            DataBaseManager server = new DataBaseManager(mock.Object);
            List<Student> result = server.SelectStudentsWIthDiciplines();

            Assert.That(result, Is.EqualTo(listStudent));
        }

    }
}