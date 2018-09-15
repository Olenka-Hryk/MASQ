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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MetricAnalyzerSoftwareQuality
{
    /// <summary>
    /// Логика взаимодействия для WindowInfoAboutAuthor.xaml
    /// </summary>
    public partial class WindowInfoAboutAuthor : Window
    {
        public WindowInfoAboutAuthor()
        {
            InitializeComponent();
            titleBar.MouseLeftButtonDown += (o, e) => DragMove();
        }

        // < CloseThisWindowInfoAboutAuthor_Button_Click > - закриття вікна, функція-обробник кнопки "ЗАКРИТИ"
        private void CloseThisWindowInfoAboutAuthor_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
