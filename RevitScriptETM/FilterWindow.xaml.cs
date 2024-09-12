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
using System.Configuration;

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
            LoadFilterSettings();
        }

        private void SaveFilterSettings()
        {
            Properties.Settings.Default.FromSectionCheckBox = FromSectionCheckBox.IsChecked ?? false;
            Properties.Settings.Default.FromSectionTextBox = FromSectionTextBox.Text;

            Properties.Settings.Default.ToSectionCheckBox = ToSectionCheckBox.IsChecked ?? false;
            Properties.Settings.Default.ToSectionTextBox = ToSectionTextBox.Text;

            Properties.Settings.Default.TaskIssuerCheckBox = TaskIssuerCheckBox.IsChecked ?? false;
            Properties.Settings.Default.TaskIssuerComboBox = TaskIssuerComboBox.SelectedItem?.ToString();

            Properties.Settings.Default.TaskCompletedCheckBox = TaskCompletedCheckBox.IsChecked ?? false;
            Properties.Settings.Default.TaskCompletedComboBox = TaskCompletedComboBox.SelectedItem?.ToString();

            Properties.Settings.Default.TaskApprovalCheckBox = TaskApprovalCheckBox.IsChecked ?? false;
            Properties.Settings.Default.TaskApprovalComboBox = TaskApprovalComboBox.SelectedItem?.ToString();

            Properties.Settings.Default.WhoApprovalCheckBox = WhoApprovalCheckBox.IsChecked ?? false;
            Properties.Settings.Default.WhoApprovalComboBox = WhoApprovalComboBox.SelectedItem?.ToString();

            Properties.Settings.Default.TaskHandlerCheckBox = TaskHandlerCheckBox.IsChecked ?? false;
            Properties.Settings.Default.TaskHandlerComboBox = TaskHandlerComboBox.SelectedItem?.ToString();

            Properties.Settings.Default.Save();
        }

        private void LoadFilterSettings()
        {
            // Проверяем наличие значения перед установкой
            FromSectionCheckBox.IsChecked = Properties.Settings.Default.FromSectionCheckBox;
            FromSectionTextBox.Text = Properties.Settings.Default.FromSectionTextBox;

            ToSectionCheckBox.IsChecked = Properties.Settings.Default.ToSectionCheckBox;
            ToSectionTextBox.Text = Properties.Settings.Default.ToSectionTextBox;

            TaskIssuerCheckBox.IsChecked = Properties.Settings.Default.TaskIssuerCheckBox;
            TaskIssuerComboBox.SelectedItem = Properties.Settings.Default.TaskIssuerComboBox;

            TaskCompletedCheckBox.IsChecked = Properties.Settings.Default.TaskCompletedCheckBox;
            if (Properties.Settings.Default.TaskCompletedComboBox != null)
            {
                TaskCompletedComboBox.SelectedItem = Properties.Settings.Default.TaskCompletedComboBox;
            }

            TaskApprovalCheckBox.IsChecked = Properties.Settings.Default.TaskApprovalCheckBox;
            if (Properties.Settings.Default.TaskApprovalComboBox != null)
            {
                TaskApprovalComboBox.SelectedItem = Properties.Settings.Default.TaskApprovalComboBox;
            }

            WhoApprovalCheckBox.IsChecked = Properties.Settings.Default.WhoApprovalCheckBox;
            if (Properties.Settings.Default.WhoApprovalComboBox != null)
            {
                WhoApprovalComboBox.SelectedItem = Properties.Settings.Default.WhoApprovalComboBox;
            }

            TaskHandlerCheckBox.IsChecked = Properties.Settings.Default.TaskHandlerCheckBox;
            if (Properties.Settings.Default.TaskHandlerComboBox != null)
            {
                TaskHandlerComboBox.SelectedItem = Properties.Settings.Default.TaskHandlerComboBox;
            }
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
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}{Function_1.dbName}.mdf; Integrated Security=True";

            using (dbSqlConnection conn = new SqlConnection(connectionString))
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
                conn.Close();
            }
            
            return issuers;
        }

        private List<string> GetTaskHandlersFromDatabase()
        {
            // Пример аналогичного метода для TaskHandler
            List<string> handlers = new List<string>();
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}{Function_1.dbName}.mdf; Integrated Security=True";

            using (dbSqlConnection conn = new SqlConnection(connectionString))
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
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}{Function_1.dbName}.mdf; Integrated Security=True";

            using (dbSqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT DISTINCT WhoApproval FROM [Table] WHERE WhoApproval IS NOT NULL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        whoApprovals.Add(reader["WhoApproval"].ToString());
                    }
                }
                conn.Close();
            }

            return whoApprovals;
        }

        private void ApplyButton_ClickFilter(object sender, RoutedEventArgs e)
        {
            // Сохранение состояния фильтров
            

            // Построение запроса с учетом фильтров
            string query = "SELECT * FROM [Table] WHERE 1=1";

            if (FromSectionCheckBox.IsChecked == true && !string.IsNullOrEmpty(FromSectionTextBox.Text))
            {
                query += $" AND FromSection = N'{FromSectionTextBox.Text}'";
            }
            if (ToSectionCheckBox.IsChecked == true && !string.IsNullOrEmpty(ToSectionTextBox.Text))
            {
                query += $" AND ToSection = N'{ToSectionTextBox.Text}'";
            }
            if (TaskIssuerCheckBox.IsChecked == true && TaskIssuerComboBox.SelectedItem != null)
            {
                query += $" AND TaskIssuer = N'{TaskIssuerComboBox.SelectedItem}'";
            }
            if (TaskHandlerCheckBox.IsChecked == true && TaskHandlerComboBox.SelectedItem != null)
            {
                query += $" AND TaskHandler = N'{TaskHandlerComboBox.SelectedItem}'";
            }
            if (TaskCompletedCheckBox.IsChecked == true)
            {
                string taskCompletedValue = (TaskCompletedComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                int completed = taskCompletedValue == "Выполнил" ? 1 : 0;
                query += $" AND TaskCompleted = {completed}";
            }
            if (TaskApprovalCheckBox.IsChecked == true)
            {
                string taskApprovalValue = (TaskApprovalComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                int approved = taskApprovalValue == "Согласовал" ? 1 : 0;
                query += $" AND TaskApproval = {approved}";
            }
            if (WhoApprovalCheckBox.IsChecked == true && WhoApprovalComboBox.SelectedItem != null)
            {
                query += $" AND WhoApproval = '{WhoApprovalComboBox.SelectedItem}'";
            }

            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}{Function_1.dbName}.mdf; Integrated Security=True";

            using (dbSqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable resultTable = new DataTable();
                    dataAdapter.Fill(resultTable);

                    SaveFilterSettings();

                    // Вызовите событие с отфильтрованными данными
                    FilterDone?.Invoke(this, resultTable);
                    DialogResult = true;

                    Close();
                }
                conn.Close();
            }
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            // Сброс состояния всех фильтров
            FromSectionCheckBox.IsChecked = false;
            FromSectionTextBox.Text = string.Empty;

            ToSectionCheckBox.IsChecked = false;
            ToSectionTextBox.Text = string.Empty;

            TaskIssuerCheckBox.IsChecked = false;
            TaskIssuerComboBox.SelectedItem = null;

            TaskHandlerCheckBox.IsChecked = false;
            TaskHandlerComboBox.SelectedItem = null;

            TaskCompletedCheckBox.IsChecked = false;
            TaskCompletedComboBox.SelectedItem = null;

            TaskApprovalCheckBox.IsChecked = false;
            TaskApprovalComboBox.SelectedItem = null;

            WhoApprovalCheckBox.IsChecked = false;
            WhoApprovalComboBox.SelectedItem = null;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            SaveFilterSettings();
            Close();
        }

    }
}
