using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Identity.Client;
using Npgsql;

namespace RevitScriptETM
{
    /// <summary>
    /// Логика взаимодействия для FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public event EventHandler<DataTable> FilterDone;
        public static FilterWindow Window;

        public FilterWindow()
        {
            Window = this;
            InitializeComponent();
            LoadComboBoxData();  // Передаем параметры projectNumber и projectName
            LoadFilterSettings();
        }

        private void MovingWin(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                FilterWindow.Window.DragMove();
            }
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

        //private List<string> GetTaskIssuersFromDatabase()
        //{
        //    List<string> issuers = new List<string>();

        //    using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
        //    {
        //        conn.Open();
        //        string query = "SELECT DISTINCT TaskIssuer FROM public.\"Table\" WHERE TaskIssuer IS NOT NULL";
        //        using (var cmd = new NpgsqlCommand(query, conn))
        //        {
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    issuers.Add(reader["TaskIssuer"].ToString());
        //                }
        //            }
        //        }
        //    }

        //    return issuers;
        //}

        //private List<string> GetTaskHandlersFromDatabase()
        //{
        //    List<string> handlers = new List<string>();

        //    using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
        //    {
        //        conn.Open();
        //        string query = "SELECT DISTINCT TaskHandler FROM public.\"Table\" WHERE TaskHandler IS NOT NULL";
        //        using (var cmd = new NpgsqlCommand(query, conn))
        //        {
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    handlers.Add(reader["TaskHandler"].ToString());
        //                }
        //            }
        //        }
        //    }

        //    return handlers;
        //}

        //private List<string> GetWhoApprovalsFromDatabase()
        //{
        //    List<string> whoApprovals = new List<string>();

        //    using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
        //    {
        //        conn.Open();
        //        string query = "SELECT DISTINCT WhoApproval FROM public.\"Table\" WHERE WhoApproval IS NOT NULL";
        //        using (var cmd = new NpgsqlCommand(query, conn))
        //        {
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    whoApprovals.Add(reader["WhoApproval"].ToString());
        //                }
        //            }
        //        }
        //    }

        //    return whoApprovals;
        //}

        private List<string> GetTaskIssuersFromDatabase()
        {
            List<string> issuers = new List<string>();

            using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
            {
                conn.Open();
                string query = "SELECT DISTINCT \"TaskIssuer\" " +
                               "FROM public.\"Table\" t " +
                               "JOIN public.\"Projects\" p ON t.\"PK_ProjectNumber\" = p.\"ProjectNumber\" " +
                               "WHERE \"TaskIssuer\" IS NOT NULL ";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        issuers.Add(row["TaskIssuer"].ToString());
                    }
                }
                conn.Close();
            }

            return issuers;
        }

        private List<string> GetTaskHandlersFromDatabase()
        {
            List<string> handlers = new List<string>();

            using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
            {
                conn.Open();
                string query = "SELECT DISTINCT \"TaskHandler\" " +
                               "FROM public.\"Table\" t " +
                               "JOIN public.\"Projects\" p ON t.\"PK_ProjectNumber\" = p.\"ProjectNumber\" " +
                               "WHERE \"TaskHandler\" IS NOT NULL ";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        handlers.Add(row["TaskHandler"].ToString());
                    }
                }
                conn.Close();
            }

            return handlers;
        }

        private List<string> GetWhoApprovalsFromDatabase()
        {
            List<string> whoApprovals = new List<string>();

            using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
            {
                conn.Open();
                string query = "SELECT DISTINCT \"WhoApproval\" " +
                               "FROM public.\"Table\" t " +
                               "JOIN public.\"Projects\" p ON t.\"PK_ProjectNumber\" = p.\"ProjectNumber\" " +
                               "WHERE \"WhoApproval\" IS NOT NULL ";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        whoApprovals.Add(row["WhoApproval"].ToString());
                    }
                }
                conn.Close();
            }

            return whoApprovals;
        }

        private void ApplyButton_ClickFilter(object sender, RoutedEventArgs e)
        {
            string query = "SELECT t.* FROM public.\"Table\" t " +
                           "JOIN public.\"Projects\" p ON t.\"PK_ProjectNumber\" = p.\"ProjectNumber\" " +
                          $"WHERE p.\"ProjectName\" = {Function_1.filename}";

            if (FromSectionCheckBox.IsChecked == true && !string.IsNullOrEmpty(FromSectionTextBox.Text))
            {
                query += $" AND \"FromSection\" = @FromSection";
            }
            if (ToSectionCheckBox.IsChecked == true && !string.IsNullOrEmpty(ToSectionTextBox.Text))
            {
                query += $" AND \"ToSection\" = @ToSection";
            }
            if (TaskIssuerCheckBox.IsChecked == true && TaskIssuerComboBox.SelectedItem != null)
            {
                query += $" AND \"TaskIssuer\" = @TaskIssuer";
            }
            if (TaskHandlerCheckBox.IsChecked == true && TaskHandlerComboBox.SelectedItem != null)
            {
                query += $" AND \"TaskHandler\" = @TaskHandler";
            }
            if (TaskCompletedCheckBox.IsChecked == true)
            {
                string taskCompletedValue = (TaskCompletedComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                int completed = taskCompletedValue == "Выполнил" ? 1 : 0;
                query += $" AND \"TaskCompleted\" = {completed}";
            }
            if (TaskApprovalCheckBox.IsChecked == true)
            {
                string taskApprovalValue = (TaskApprovalComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                int approved = taskApprovalValue == "Согласовал" ? 1 : 0;
                query += $" AND \"TaskApproval\" = {approved}";
            }
            if (WhoApprovalCheckBox.IsChecked == true && WhoApprovalComboBox.SelectedItem != null)
            {
                query += $" AND \"WhoApproval\" = @WhoApproval";
            }

            using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    // Добавляем параметры
                    

                    if (FromSectionCheckBox.IsChecked == true && !string.IsNullOrEmpty(FromSectionTextBox.Text))
                    {
                        cmd.Parameters.AddWithValue("@FromSection", FromSectionTextBox.Text);
                    }
                    if (ToSectionCheckBox.IsChecked == true && !string.IsNullOrEmpty(ToSectionTextBox.Text))
                    {
                        cmd.Parameters.AddWithValue("@ToSection", ToSectionTextBox.Text);
                    }
                    if (TaskIssuerCheckBox.IsChecked == true && TaskIssuerComboBox.SelectedItem != null)
                    {
                        cmd.Parameters.AddWithValue("@TaskIssuer", TaskIssuerComboBox.SelectedItem.ToString());
                    }
                    if (TaskHandlerCheckBox.IsChecked == true && TaskHandlerComboBox.SelectedItem != null)
                    {
                        cmd.Parameters.AddWithValue("@TaskHandler", TaskHandlerComboBox.SelectedItem.ToString());
                    }
                    if (WhoApprovalCheckBox.IsChecked == true && WhoApprovalComboBox.SelectedItem != null)
                    {
                        cmd.Parameters.AddWithValue("@WhoApproval", WhoApprovalComboBox.SelectedItem.ToString());
                    }

                    using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable resultTable = new DataTable();
                        dataAdapter.Fill(resultTable);

                        SaveFilterSettings();

                        FilterDone?.Invoke(this, resultTable);
                        DialogResult = true;

                        Close();
                    }
                }
            }
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
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
