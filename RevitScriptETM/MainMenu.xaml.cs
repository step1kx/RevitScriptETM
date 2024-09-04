using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace RevitScriptETM
{
    public partial class MainMenu : Window
    {

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
                RefreshItems(); 
            };
            inputWindow.ShowDialog();
        }
        // Обработчик нажатия кнопки фильтра
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterWindow filterWindow = new FilterWindow();
            filterWindow.FilterDone += FilterRefresh;
            filterWindow.ShowDialog();

        }

        private void FilterRefresh(object sender, DataTable e)
        {
            // Обновите DataGrid или другой элемент управления данными
            tasksDataGrid.ItemsSource = e.DefaultView;
        }

        private void TaskCompletedCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateTaskField("TaskCompleted", "TaskHandler");
        }

        private void TaskApprovalCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateTaskField("TaskApproval", "WhoApproval");
        }

        private void UpdateTaskField(string checkBoxField, string userField)
        {
            // Получаем выбранную строку из DataGrid
            if (tasksDataGrid.SelectedItem is DataRowView row)
            {
                int taskNumber = Convert.ToInt32(row["TaskNumber"]);
                bool isChecked = Convert.ToBoolean(row[checkBoxField]);

                // Определяем имя пользователя
                string userName = Environment.UserName;

                // Обновляем данные в базе данных
                string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}\Tasks.mdf; Integrated Security=True";
                string query = isChecked
                    ? $"UPDATE [Table] SET {userField} = @UserName WHERE TaskNumber = @TaskNumber"
                    : $"UPDATE [Table] SET {userField} = NULL WHERE TaskNumber = @TaskNumber";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", isChecked ? userName : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TaskNumber", taskNumber);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Обновляем DataGrid
                RefreshItems();
            }
        }

        public void RefreshItems()
        {
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}\Tasks.mdf; Integrated Security=True";
            string query = "SELECT * FROM [Table]"; // Загрузка всех данных из таблицы

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Обновляем DataGrid
                    tasksDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
        }
    }
}
