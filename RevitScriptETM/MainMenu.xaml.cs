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
using DataGrid = System.Windows.Controls.DataGrid;

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

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем и отображаем окно фильтра
            FilterWindow filterWindow = new FilterWindow();
            if (filterWindow.ShowDialog() == true)
            {
                // Применяем фильтр, если пользователь нажал "Применить"
                ApplyFilter(filterWindow.FromSection,
                            filterWindow.ToSection,
                            filterWindow.TaskIssuer,
                            filterWindow.TaskHandler,
                            filterWindow.TaskCompleted,
                            filterWindow.TaskApproval,
                            filterWindow.WhoApproval
                            );
            }
        }

        private void ApplyFilter(string fromSection, string toSection, string taskIssuer, string taskHandler, int? taskCompleted, int? taskApproval, string whoApproval)
        {
            throw new NotImplementedException();
        }

        // Метод для применения фильтра к данным DataGrid
        private void ApplyFilter(string fromSection, string toSection, string taskIssuer, int? taskCompleted, string taskHandler, int? taskApproval, string whoApproval)
        {
            var filteredItems = TaskItems
                .Where(item => (string.IsNullOrEmpty(fromSection) || item.FromSection.Contains(fromSection)) &&
                               (string.IsNullOrEmpty(toSection) || item.ToSection.Contains(toSection)) &&
                               (string.IsNullOrEmpty(taskIssuer) || item.TaskIssuer == taskIssuer) &&
                               (!taskCompleted.HasValue || item.TaskCompleted == (taskCompleted.Value == 1)) && // Преобразование int в bool
                               (string.IsNullOrEmpty(taskHandler) || item.TaskHandler == taskHandler) &&
                               (!taskApproval.HasValue || item.TaskApproval == (taskApproval.Value == 1)) && // Преобразование int в bool
                               (string.IsNullOrEmpty(whoApproval) || item.WhoApproval == whoApproval))
                .ToList();

            tasksDataGrid.ItemsSource = filteredItems;
        }

    }
}
