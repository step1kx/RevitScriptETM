using System.Collections.ObjectModel;
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

            TaskItems = new ObservableCollection<TaskItems>();
            tasksDataGrid.CanUserAddRows = false;
            
            

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
            
        }

        // Метод для применения фильтра к данным DataGrid
        
    }
}
