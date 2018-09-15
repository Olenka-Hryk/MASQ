using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public partial class WindowOfSettings : Window
    {
        public WindowOfSettings()
        {
            InitializeComponent();
            titleBar.MouseLeftButtonDown += (o, e) => DragMove();
            cmbColors.ItemsSource = typeof(Colors).GetProperties();
        }

        // < CloseWindowSettings_Button_Click > - закриття вікна, функція-обробник кнопки "СКАСУВАТИ"
        private void CloseWindowSettings_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // < SaveWindowSettings_Button_Click > - закриття вікна + збереження налаштувань, функція-обробник кнопки "ЗБЕРЕГТИ"
        private void SaveWindowSettings_Button_Click(object sender, RoutedEventArgs e)
        {
            if(ShowOrHideToollPanel.IsChecked == true)
            {
                ((MainWindow)App.MainWin).ToolBar.Visibility = Visibility.Hidden;
                ((MainWindow)App.MainWin).grid_Main.RowDefinitions[0].Height = new GridLength(80);
            }
            if (ShowOrHideToollPanel.IsChecked == false)
            {
                ((MainWindow)App.MainWin).ToolBar.Visibility = Visibility;
                ((MainWindow)App.MainWin).grid_Main.RowDefinitions[0].Height = new GridLength(111);
            }
            Color selectedColor = (Color)(cmbColors.SelectedItem as PropertyInfo).GetValue(null, null);
            ((MainWindow)App.MainWin).grid_Main.Background = new SolidColorBrush(selectedColor);

            if(MessageBox.Show("Нові налаштування програми успішно збережено! \n ", "Інформація:", MessageBoxButton.OKCancel, MessageBoxImage.Information)== MessageBoxResult.OK)
            {
                this.Close();
            }
            else { }
        }
    }
}
