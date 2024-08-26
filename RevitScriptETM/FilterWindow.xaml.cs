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
        public string FromSection => FromSectionCheckBox.IsChecked == true ? FromSectionTextBox.Text : null;
        public string ToSection => ToSectionCheckBox.IsChecked == true ? ToSectionTextBox.Text : null;
        public string TaskIssuer => TaskIssuerCheckBox.IsChecked == true ? TaskIssuerComboBox.SelectedItem as string : null;
        public string TaskHandler => TaskHandlerCheckBox.IsChecked == true ? TaskHandlerComboBox.SelectedItem as string : null;
        public bool? TaskCompleted
        {
            get
            {
                if (TaskCompletedCheckBox.IsChecked == true)
                {
                    if (TaskCompletedComboBox.SelectedIndex == 1)
                    {
                        return true; // Выполнил
                    }
                    else if (TaskCompletedComboBox.SelectedIndex == 2)
                    {
                        return false; // Не выполнил
                    }
                }
                return null; // Все или если CheckBox не выбран
            }
        }

        public bool? TaskApproval
        {
            get
            {
                if (TaskApprovalCheckBox.IsChecked == true)
                {
                    if (TaskApprovalComboBox.SelectedIndex == 1)
                    {
                        return true; // Согласовал
                    }
                    else if (TaskApprovalComboBox.SelectedIndex == 2)
                    {
                        return false; // Не согласовал
                    }
                }
                return null; // Все или если CheckBox не выбран
            }
        }

        public string WhoApproval => WhoApprovalCheckBox.IsChecked == true ? WhoApprovalComboBox.SelectedItem as string : null;

        public FilterWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            // Загрузка данных в ComboBox'ы из базы данных
            // Например, TaskIssuerComboBox.ItemsSource = GetTaskIssuersFromDatabase();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    
    }
}
