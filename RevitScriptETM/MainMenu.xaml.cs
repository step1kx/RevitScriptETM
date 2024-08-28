using Autodesk.Revit.UI;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace RevitScriptETM
{
    public partial class MainMenu : Window
    {
        public ObservableCollection<TaskItems> TaskItems { get; set; }

        public MainMenu()
        {
            InitializeComponent();
            tasksDataGrid.CanUserAddRows = false;
        }

        private void TasksCreator_Click(object sender, RoutedEventArgs e)
        {
            TasksCreator inputWindow = new TasksCreator();
            inputWindow.ShowDialog();
        }

        // Обработчик нажатия кнопки фильтра
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterWindow filterWindow = new FilterWindow();
            filterWindow.ShowDialog();
        }
        
    }
}
