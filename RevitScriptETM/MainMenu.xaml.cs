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

            DataContext = this; // Устанавливаем DataContext
            TaskItems = new ObservableCollection<TaskItems>();
            tasksDataGrid.CanUserAddRows = false;
            tasksDataGrid.ItemsSource = Function_1.collview.View;

            // tasksDataGrid.ItemsSource = 



            // DataContext = this;


        }

        private void TasksCreator_Click(object sender, RoutedEventArgs e)
        {
            TasksCreator inputWindow = new TasksCreator();
            DataFromRevit_Event eventHandler = new DataFromRevit_Event();
            ExternalEvent tskview = ExternalEvent.Create(eventHandler);
            tskview.Raise();
            TasksCreator.elem = eventHandler.elements;
            inputWindow.ShowDialog();
                //MessageBox.Show(elem.Count.ToString() + "это elem");
            
        }

        // Обработчик нажатия кнопки фильтра
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем и отображаем окно фильтра
            FilterWindow filterWindow = new FilterWindow();
            filterWindow.ShowDialog();
            
        }

        // Метод для применения фильтра к данным DataGrid
        
    }
}
