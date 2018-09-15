using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricAnalyzerSoftwareQuality.Class.Table
{
   public class MyTableInfo_allResultsMetrics : INotifyPropertyChanged
    {
        private int _number;
        private double _value;
        private string _id;
        private string _name;

        public MyTableInfo_allResultsMetrics() { }
        public MyTableInfo_allResultsMetrics(int number, double value, string id, string name)
        {
            this.Number = number;
            this.Value = value;
            this.ID = id;
            this.Name = name;
        }
        public int Number {
            get
            {
                return _number;
            }

            set
            {
                _number = value;
                NotifyPropertyChanged("Number");
            }
        }

        public double Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                NotifyPropertyChanged("Value");
            }
        }

        public string ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                NotifyPropertyChanged("ID");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

// var mainWindow = new MainWindow(); --> DataGrid --> USERS
// var viewModel = new ViewModel(); --> UsersCollection
// mainWindow.DataContext = viewModel;
// property { get; set; }
  // row -> USer (name, age///)

