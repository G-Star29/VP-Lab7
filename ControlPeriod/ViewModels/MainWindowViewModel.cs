using Avalonia.Media;
using Avalonia;
using Lab7.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace Lab7.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase result;

        ObservableCollection<IBrush> AvgMarksBrush { get; set; }
        ObservableCollection<Student> StudentItemRow { get; set; }
        ObservableCollection<float?> AvgMarsk { get; set; }
        public ViewModelBase Result
        {
            get => result;
            private set => this.RaiseAndSetIfChanged(ref result, value);
        }

        public void AddStudentItem()
        {
            StudentItemRow.Insert(0, new Student("Student name"));
            CalcAvgStudentsAll();
        }

        public void RemoveStudentItem()
        {
            var neededStudents = this.StudentItemRow.Where(x => !x.isChecked).ToList();
            StudentItemRow.Clear();
            foreach (var neededStudent in neededStudents)
            {
                StudentItemRow.Add(neededStudent);
            }
            CalcAvgStudentsAll();

        }

        public MainWindowViewModel()
        {
            StudentItemRow = new ObservableCollection<Student>();
            AvgMarsk = new ObservableCollection<float?>() { 0, 0, 0 };
            AvgMarksBrush = new ObservableCollection<IBrush>() { new SolidColorBrush(Brushes.White.Color), new SolidColorBrush(Brushes.White.Color), new SolidColorBrush(Brushes.White.Color) };
            StudentItemRow.CollectionChanged += IfCollectChange;
            Result = new WindowViewModel();
            
        }

        public void WriteFile(string filePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Student>));

            using (StreamWriter wr = new StreamWriter(filePath))
            {
                xs.Serialize(wr, this.StudentItemRow);
            }
        }

        public void ReadFile(string filePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Student>));
            using (StreamReader sr = new StreamReader(filePath))
            {
                StudentItemRow.Clear();
                StudentItemRow = (ObservableCollection<Student>)xs.Deserialize(sr);
                foreach (Student s in this.StudentItemRow)
                {
                    var gradeList = new List<Marks>(3);
                    gradeList.Add(s.IntermediateÑertification[3]);
                    gradeList.Add(s.IntermediateÑertification[4]);
                    gradeList.Add(s.IntermediateÑertification[5]);
                    s.IntermediateÑertification.Clear();
                    foreach (var mark in gradeList)
                    {
                        s.IntermediateÑertification.Add(mark);
                    }
                    s.AvgMarkCalc();
                }
            }
        }
        public void OpenWindowView() => Result = new WindowViewModel();

        public void CalcAvgStudentsAll()
        {

            for (int i = 0; i < 3; i++)
            {
                AvgMarsk[i] = 0;
            }
            foreach (Student s in StudentItemRow)
            {
                if (StudentItemRow != null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        AvgMarsk[i] += s.IntermediateÑertification[i].Mark;
                    }
                }
            }
            if (StudentItemRow.Count != 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    AvgMarsk[i] /= StudentItemRow.Count;
                    if (AvgMarsk[i] != null)
                    {
                        if (AvgMarsk[i] < 1.5) AvgMarksBrush[i] = new SolidColorBrush(Brushes.Yellow.Color);
                        if (AvgMarsk[i] < 1) AvgMarksBrush[i] = new SolidColorBrush(Brushes.Red.Color);
                        if (AvgMarsk[i] >= 1.5) AvgMarksBrush[i] = new SolidColorBrush(Brushes.LightGreen.Color);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    AvgMarsk[i] = null;
                    AvgMarksBrush[i] = new SolidColorBrush(Brushes.White.Color);
                }

            }
        }
        void IfCollectChange(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Student item in e.NewItems)
                    item.PropertyChanged += IfPropertyChange;
        }

        void IfPropertyChange(object sender, PropertyChangedEventArgs e) => CalcAvgStudentsAll();
    }
}
