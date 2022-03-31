using Avalonia.Media;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Lab7.Models
{
    public class Marks : INotifyPropertyChanged
    {
        float? mark;

        [XmlIgnore]
        public Avalonia.Media.IBrush Brush { get; private set; }

        public float? Mark
        {
            set
            {
                switch (value)
                {
                    case 0:
                        Brush = Brushes.Red;
                        mark = value;
                        break;
                    case 1:
                        Brush = Brushes.Yellow;
                        mark = value;
                        break;
                    case 2:
                        Brush = Brushes.LightGreen;
                        mark = value;
                        break;
                    default:
                        Brush = Brushes.White;
                        mark = null;
                        break;
                }
                RaisePropertyChangedEvent("Mark");

            }
            get => mark;
        }
        public Marks(float mark)
        {
            Mark = mark;
        }

        public Marks()
        {
            Mark = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }
    }
}
