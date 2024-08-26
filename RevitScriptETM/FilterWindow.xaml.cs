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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RevitScriptETM
{
    /// <summary>
    /// Логика взаимодействия для FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public string FromSection { get; private set; }
        public string ToSection { get; private set; }
        public string TaskHandler { get; private set; }

        public FilterWindow()
        {
            InitializeComponent();
        }

        //Обработчик для кнопки "Применить"
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // Сохраняем введенные значения
            //FromSection = FromSectionTextBox.Text;
            //ToSection = ToSectionTextBox.Text;
            //TaskHandler = TaskHandlerTextBox.Text;
            //DialogResult = true;
        }

        // Обработчик для кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
