using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public event EventHandler<DataTable> FilterDone;

        public FilterWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            TaskIssuerComboBox.ItemsSource = GetTaskIssuersFromDatabase();
            TaskHandlerComboBox.ItemsSource = GetTaskHandlersFromDatabase();
            WhoApprovalComboBox.ItemsSource = GetWhoApprovalsFromDatabase();
        }

        private List<string> GetTaskIssuersFromDatabase()
        {
            List<string> issuers = new List<string>();
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}\Tasks.mdf; Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT DISTINCT TaskIssuer FROM [Table] WHERE TaskIssuer IS NOT NULL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        issuers.Add(reader["TaskIssuer"].ToString());
                    }
                }
            }

            return issuers;
        }

        private List<string> GetTaskHandlersFromDatabase()
        {
            // Пример аналогичного метода для TaskHandler
            List<string> handlers = new List<string>();
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}\Tasks.mdf; Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT DISTINCT TaskHandler FROM [Table] WHERE TaskHandler IS NOT NULL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        handlers.Add(reader["TaskHandler"].ToString());
                    }
                }
            }

            return handlers;
        }

        private List<string> GetWhoApprovalsFromDatabase()
        {
            // Пример аналогичного метода для WhoApproval
            List<string> whoApprovals = new List<string>();
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}\Tasks.mdf; Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT DISTINCT TaskView FROM [Table] WHERE TaskView IS NOT NULL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        whoApprovals.Add(reader["TaskView"].ToString());
                    }
                }
            }

            return whoApprovals;
        }


        //private void ApplyButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var filteredItems = taskItems.AsQueryable();

        //    if (FromSectionCheckBox.IsChecked == true && !string.IsNullOrEmpty(FromSectionTextBox.Text))
        //    {
        //        filteredItems = filteredItems.Where(item => item.FromSection == FromSectionTextBox.Text);
        //    }

        //    if (ToSectionCheckBox.IsChecked == true && !string.IsNullOrEmpty(ToSectionTextBox.Text))
        //    {
        //        filteredItems = filteredItems.Where(item => item.ToSection == ToSectionTextBox.Text);
        //    }

        //    if (TaskIssuerCheckBox.IsChecked == true && TaskIssuerComboBox.SelectedItem != null)
        //    {
        //        filteredItems = filteredItems.Where(item => item.TaskIssuer == TaskIssuerComboBox.SelectedItem.ToString());
        //    }

        //    if (TaskHandlerCheckBox.IsChecked == true && TaskHandlerComboBox.SelectedItem != null)
        //    {
        //        filteredItems = filteredItems.Where(item => item.TaskHandler == TaskHandlerComboBox.SelectedItem.ToString());
        //    }

        //    if (TaskCompletedCheckBox.IsChecked == true)
        //    {
        //        string taskCompletedValue = (TaskCompletedComboBox.SelectedItem as ComboBoxItem).Content.ToString();
        //        bool completed = taskCompletedValue == "Выполнил";
        //        filteredItems = filteredItems.Where(item => item.TaskCompleted == 1);
        //    }

        //    if (TaskApprovalCheckBox.IsChecked == true)
        //    {
        //        string taskApprovalValue = (TaskApprovalComboBox.SelectedItem as ComboBoxItem).Content.ToString();
        //        bool approved = taskApprovalValue == "Согласовал";
        //        filteredItems = filteredItems.Where(item => item.TaskApproval == 1);
        //    }

        //    if (WhoApprovalCheckBox.IsChecked == true && WhoApprovalComboBox.SelectedItem != null)
        //    {
        //        filteredItems = filteredItems.Where(item => item.WhoApproval == WhoApprovalComboBox.SelectedItem.ToString());
        //    }

        //    // Теперь у вас есть отфильтрованный список
        //    List<TaskItems> result = filteredItems.ToList();
        //}
        private void ApplyButton_ClickFilter(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM [Table] WHERE 1=1";

            if (FromSectionCheckBox.IsChecked == true && !string.IsNullOrEmpty(FromSectionTextBox.Text))
            {
                query += $" AND FromSection = N'{FromSectionTextBox.Text}'";
            }
            // Фильтр по ToSection
            if (ToSectionCheckBox.IsChecked == true && !string.IsNullOrEmpty(ToSectionTextBox.Text))
            {
                query += $" AND ToSection = N'{ToSectionTextBox.Text}'";
            }

            // Фильтр по TaskIssuer
            if (TaskIssuerCheckBox.IsChecked == true && TaskIssuerComboBox.SelectedItem != null)
            {
                query += $" AND TaskIssuer = N'{TaskIssuerComboBox.SelectedItem}'";
            }

            // Фильтр по TaskHandler
            if (TaskHandlerCheckBox.IsChecked == true && TaskHandlerComboBox.SelectedItem != null)
            {
                query += $" AND TaskHandler = N'{TaskHandlerComboBox.SelectedItem}'";
            }

            // Фильтр по TaskCompleted
            if (TaskCompletedCheckBox.IsChecked == true)
            {
                string taskCompletedValue = (TaskCompletedComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                int completed = taskCompletedValue == "Выполнил" ? 1 : 0;
                query += $" AND TaskCompleted = {completed}";
            }

            // Фильтр по TaskApproval
            if (TaskApprovalCheckBox.IsChecked == true)
            {
                string taskApprovalValue = (TaskApprovalComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                int approved = taskApprovalValue == "Согласовал" ? 1 : 0;
                query += $" AND TaskApproval = {approved}";
            }

            // Фильтр по WhoApproval
            if (WhoApprovalCheckBox.IsChecked == true && WhoApprovalComboBox.SelectedItem != null)
            {
                query += $" AND WhoApproval = '{WhoApprovalComboBox.SelectedItem}'";
            }

            // ... добавьте другие фильтры, если необходимо ...

            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}\Tasks.mdf; Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable resultTable = new DataTable();
                    dataAdapter.Fill(resultTable);

                    // Вызовите событие с отфильтрованными данными
                    FilterDone?.Invoke(this, resultTable);
                    DialogResult = true;
                    Close();
                }
                
            }
        }




        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    
    }
}
