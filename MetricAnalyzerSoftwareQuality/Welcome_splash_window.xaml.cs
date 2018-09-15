using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;

namespace MetricAnalyzerSoftwareQuality
{
    /// <summary>
    /// Логика взаимодействия для Welcome_splash_window.xaml
    /// </summary>
    public partial class Welcome_splash_window : Window, INotifyPropertyChanged
    {
        private BackgroundWorker _bgWorker = new BackgroundWorker();
        public int _workerState;

        public event PropertyChangedEventHandler PropertyChanged;

        public int WorkerState
        {
            get { return _workerState; }
            set
            {
                _workerState = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("WorkerState"));
            }
        }

        public Welcome_splash_window()
        {
            InitializeComponent();
            DataContext = this;
            _bgWorker.DoWork += (s, e) =>
             {
                 for (int i = 0; i <= 50; i++)
                 {
                     System.Threading.Thread.Sleep(50);
                     WorkerState = i;
                 }
                 Application.Current.Dispatcher.Invoke((Action)delegate
                 {
                     MainWindow mainWindow = new MainWindow(); // _prop -- private members of class)
                     App.MainWin = mainWindow; 
                     mainWindow.Show();
                     this.Hide();
                 });
             };

            _bgWorker.RunWorkerAsync();
        }
    }
}
