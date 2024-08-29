using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
            inputWindow.TaskCreated += (s, args) =>
            {
                RefreshItems(); // Обновление данных в DataGrid после создания задачи
            };
            inputWindow.ShowDialog();
        }

        // Обработчик нажатия кнопки фильтра
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterWindow filterWindow = new FilterWindow();
            filterWindow.ShowDialog();
        }

        

        private void tasksDataGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            tasksDataGrid.Items.Refresh();
        }

        public void RefreshItems()
        {
            SqlConnection conn = new SqlConnection($@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {Function_1.documentDirectory}\Tasks.mdf; Integrated Security = True", null);
            using (conn)
            {
                conn.Open();
                SqlCommand createCommand = new SqlCommand("SELECT * FROM [Table]", conn);
                createCommand.ExecuteNonQuery();
                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("Table"); // В скобках указываем название таблицы
                dataAdp.Fill(dt);
                // Вывод на грид
                tasksDataGrid.ItemsSource = dt.DefaultView;

            }
            InitializeComponent();
            tasksDataGrid.Items.Refresh();
        }
    }
}
