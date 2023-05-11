using Npgsql;
using SchoolReport.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;


namespace SchoolReport
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public DataBaseManager dataBaseManager;
        public ObservableCollection<Student> students { get; set; }

        public ICommand AddStudent { get; private set; }
        public ICommand RemoveStudent { get; private set; }
        public ICommand EditStudent { get; private set; }
        public ICommand AddDiscipline { get; private set; }
        public ICommand RemoveDiscipline { get; private set; }
        public ICommand EditDiscipline { get; private set; }
        

        private Student _selectedStudent;

        public Discipline _selectedDiscipline;

        public MainWindowVM()
        {
            IDatabse database = new ConnectionMariaDB();
            dataBaseManager = new DataBaseManager(database);


            students = new ObservableCollection<Student>(dataBaseManager.SelectStudentsWIthDiciplines());
            StartCommands();
        }

        public void StartCommands()
        {
            AddStudent = new RelayCommand((object _) =>
            {
                Student newStudent = new Student();
                StudentScreen studentScreen = new StudentScreen
                {
                    DataContext = newStudent
                };
                bool? validation = studentScreen.ShowDialog();
                if (validation.HasValue && validation.Value)
                {
                    dataBaseManager.InsertStudent(newStudent);
                    students.Clear();
                    students = new ObservableCollection<Student>(dataBaseManager.SelectStudentsWIthDiciplines());
                    Notify(nameof(students));
                }
            });

            RemoveStudent = new RelayCommand((object _) =>
            {
                dataBaseManager.DeleteStudent(SelectedStudent);
                students.Clear();
                students = new ObservableCollection<Student>(dataBaseManager.SelectStudentsWIthDiciplines());
                Notify(nameof(students));
            });

            EditStudent = new RelayCommand((object _) =>
            {
                Student copy = SelectedStudent.ShallowCopy();
                StudentScreen studentScreen = new StudentScreen
                {
                    DataContext = copy
                };
                bool? validation = studentScreen.ShowDialog();
                if (validation.HasValue && validation.Value)
                {
                    dataBaseManager.UpdateStudent(copy);
                    students.Clear();
                    students = new ObservableCollection<Student>(dataBaseManager.SelectStudentsWIthDiciplines());
                    Notify(nameof(students));
                }
            });

            AddDiscipline = new RelayCommand((object _) =>
            {
                Discipline newDiscipline = new Discipline();
                DisciplineScreen disciplineScreen = new DisciplineScreen
                {
                    DataContext = newDiscipline
                };
                bool? validation = disciplineScreen.ShowDialog();
                if (validation.HasValue && validation.Value)
                {
                    dataBaseManager.StudentAddDiscipline(newDiscipline, SelectedStudent);
                    students.Clear();
                    students = new ObservableCollection<Student>(dataBaseManager.SelectStudentsWIthDiciplines());
                    Notify(nameof(students));
                }
            });

            RemoveDiscipline = new RelayCommand((object _) =>
            {
                dataBaseManager.DeleteDisciplineOfStudent(SelectedDiscipline, SelectedStudent);
                students.Clear();
                students = new ObservableCollection<Student>(dataBaseManager.SelectStudentsWIthDiciplines());
                Notify(nameof(students));
            });

            EditDiscipline = new RelayCommand((object _) =>
            {
                Discipline copy = SelectedDiscipline.ShallowCopy();
                DisciplineScreen disciplineScreen = new DisciplineScreen
                {
                    DataContext = copy
                };
                bool? validation = disciplineScreen.ShowDialog();
                if (validation.HasValue && validation.Value)
                {
                    dataBaseManager.UpdateGrade(copy, SelectedStudent);
                    students.Clear();
                    students = new ObservableCollection<Student>(dataBaseManager.SelectStudentsWIthDiciplines());
                    Notify(nameof(students));
                }
            });

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                if (_selectedStudent != value)
                {
                    _selectedStudent = value;
                    Notify(nameof(SelectedStudent));
                }
            }
        }

        public Discipline SelectedDiscipline
        {
            get { return _selectedDiscipline; }
            set
            {
                if (_selectedDiscipline != value)
                {
                    _selectedDiscipline = value;
                    Notify(nameof(SelectedDiscipline));
                }
            }
        }
    }


}
