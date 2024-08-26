using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RevitScriptETM
{
    public partial class MainMenu : Window
    {
        public ObservableCollection<TaskItems> TaskItems { get; set; }

        public MainMenu()
        {
            InitializeComponent();

            TaskItems = new ObservableCollection<TaskItems>();

            DataContext = this;


        }

        private void TasksCreator_Click(object sender, RoutedEventArgs e)
        {
            TasksCreator inputWindow = new TasksCreator();
            if (inputWindow.ShowDialog() == true)
            {
                // Можно обработать введенные данные из InputWindow
                // Пример: MessageBox.Show($"Раздел от кого: {inputWindow.FromSection}");
            }
        }

        // Обработчик нажатия кнопки фильтра
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем и отображаем окно фильтра
            FilterWindow filterWindow = new FilterWindow();
            if (filterWindow.ShowDialog() == true)
            {
                // Применяем фильтр, если пользователь нажал "Применить"
                ApplyFilter(
                    filterWindow.FromSection,
                    filterWindow.ToSection,
                    filterWindow.TaskIssuer,
                    filterWindow.TaskHandler,
                    filterWindow.TaskCompleted,
                    filterWindow.TaskApproval,
                    filterWindow.WhoApproval,
                    filterWindow.FromSectionCheckBox.IsChecked ?? false,
                    filterWindow.ToSectionCheckBox.IsChecked ?? false,
                    filterWindow.TaskIssuerCheckBox.IsChecked ?? false,
                    filterWindow.TaskCompletedCheckBox.IsChecked ?? false,
                    filterWindow.TaskHandlerCheckBox.IsChecked ?? false,
                    filterWindow.TaskApprovalCheckBox.IsChecked ?? false,
                    filterWindow.WhoApprovalCheckBox.IsChecked ?? false
                );
            }
        }

        // Метод для применения фильтра к данным DataGrid
        private void ApplyFilter(
            string fromSection,
            string toSection,
            string taskIssuer,
            string taskHandler,
            bool? taskCompleted,
            bool? taskApproval,
            string whoApproval,
            bool filterByFromSection,
            bool filterByToSection,
            bool filterByTaskIssuer,
            bool filterByTaskCompleted,
            bool filterByTaskHandler,
            bool filterByTaskApproval,
            bool filterByWhoApproval)
        {
            var filteredItems = TaskItems
                .Where(item => (!filterByFromSection || string.IsNullOrEmpty(fromSection) || item.FromSection.Contains(fromSection)) &&
                               (!filterByToSection || string.IsNullOrEmpty(toSection) || item.ToSection.Contains(toSection)) &&
                               (!filterByTaskIssuer || string.IsNullOrEmpty(taskIssuer) || item.TaskIssuer == taskIssuer) &&
                               (!filterByTaskCompleted || !taskCompleted.HasValue || item.TaskCompleted == taskCompleted.Value) &&
                               (!filterByTaskHandler || string.IsNullOrEmpty(taskHandler) || item.TaskHandler == taskHandler) &&
                               (!filterByTaskApproval || !taskApproval.HasValue || item.TaskApproval == taskApproval.Value) &&
                               (!filterByWhoApproval || string.IsNullOrEmpty(whoApproval) || item.WhoApproval == whoApproval))
                .ToList();

            tasksDataGrid.ItemsSource = filteredItems;
        }
    }
}
