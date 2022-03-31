using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab7.Models
{
    public class Student : INotifyPropertyChanged
    {
        public string Name { set; get; }

        ObservableCollection<Marks> шntermediateСertification;
        public ObservableCollection<Marks> IntermediateСertification
        {
            get => шntermediateСertification;
            set
            {
                this.шntermediateСertification = value;
                RaisePropertyChangedEvent("IntermediateСertification");
            }
        }

        float? avg;
        [XmlIgnore]
        Avalonia.Media.SolidColorBrush avgBrush;
        [XmlIgnore]
        public Avalonia.Media.SolidColorBrush AvgBrush
        {
            get => avgBrush;
            private set
            {
                this.avgBrush = value;
                RaisePropertyChangedEvent("AvgBrush");
            }
        }
        [XmlIgnore]
        public bool isChecked { get; set; }
        [XmlIgnore]
        public float? Avg
        {
            get => avg;
            private set
            {
                if (value is not null)
                {
                    if (value < 1)
                    {
                        AvgBrush = new SolidColorBrush(Brushes.Red.Color);
                        avg = value;
                    }

                    if (value < 1.5)
                    {
                        AvgBrush = new SolidColorBrush(Brushes.Yellow.Color);
                        avg = value;
                    }
            
                    if (value >= 1.5)
                    {
                        AvgBrush = new SolidColorBrush(Brushes.LightGreen.Color);
                        avg = value;
                    }
                }
                else
                {
                    avg = null;
                    AvgBrush = new SolidColorBrush(Brushes.White.Color);
                }
                RaisePropertyChangedEvent("Avg");
            }
        }

        public void AvgMarkCalc()
        {
            if (IntermediateСertification.Any(mark => mark.Mark is null))
            {
                Avg = null;
            }
            else
            {
                float sum = 0;
                foreach (var mark in IntermediateСertification) sum += (float)mark.Mark;
                Avg = sum / 3;
            }
        }
        public Student(string name)
        {
            Name = name;
            IntermediateСertification = new ObservableCollection<Marks>();
            IntermediateСertification.CollectionChanged += IfCollectChange_1;
            IntermediateСertification.Clear();
            IntermediateСertification.Add(new Marks(0));
            IntermediateСertification.Add(new Marks(0));
            IntermediateСertification.Add(new Marks(0));
            isChecked = false;
            AvgMarkCalc();
        }

        public Student()
        {
            Name = "NULL";
            IntermediateСertification = new ObservableCollection<Marks>();
            IntermediateСertification.CollectionChanged += IfCollectChange_1;
            IntermediateСertification.Clear();
            IntermediateСertification.Add(new Marks(0));
            IntermediateСertification.Add(new Marks(0));
            IntermediateСertification.Add(new Marks(0));
            isChecked = false;
            AvgMarkCalc();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChangedEvent(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        void IfCollectChange_1(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Marks item in e.NewItems)
                    item.PropertyChanged += IfPropertyChange_1;

            if (e.OldItems != null)
                foreach (Marks item in e.OldItems)
                    item.PropertyChanged -= IfPropertyChange_1;
        }

        void IfPropertyChange_1(object sender, PropertyChangedEventArgs e) => AvgMarkCalc();
     

    }
}
