﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RevitScriptETM
{
    public partial class MainMenu : Window
    {
        public event EventHandler<DataTable> DataUpdated;
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

        #region Чекбоксы. Кто выполнил задание
        private void TaskCompleted_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.DataContext is DataRowView rowView)
            {
                if (rowView != null)
                {
                    rowView["TaskHandler"] = Function_1.username;
                    UpdateDatabaseForHandlers(rowView, Function_1.username, 1);
                    RefreshItems_CheckBox();
                }
                else
                {
                    MessageBox.Show("Ошибка: DataRowView не установлен.");
                }
            }
        }

        private void TaskCompleted_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.DataContext is DataRowView rowView)
            {
                rowView["TaskHandler"] = DBNull.Value;

                UpdateDatabaseForHandlers(rowView, null, 0);
                RefreshItems_CheckBox();
            }
        }

        private void TaskApproval_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.DataContext is DataRowView rowView)
            {
                rowView["WhoApproval"] = Function_1.username;

                UpdateDatabaseForApprovals(rowView, Function_1.username, 1);
                RefreshItems_CheckBox();
            }
        }

        private void TaskApproval_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.DataContext is DataRowView rowView)
            {
                rowView["WhoApproval"] = DBNull.Value;

                UpdateDatabaseForApprovals(rowView, null, 0);
                RefreshItems_CheckBox();
            }
        }

        private void UpdateDatabaseForHandlers(DataRowView rowView, string taskHandler, int taskCompleted)
        {
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}\Tasks.mdf; Integrated Security=True";
            string query = "UPDATE [Table] SET TaskHandler = @TaskHandler, TaskCompleted = @TaskCompleted WHERE TaskNumber = @TaskNumber";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TaskHandler", taskHandler ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TaskCompleted", taskCompleted);
                    cmd.Parameters.AddWithValue("@TaskNumber", rowView["TaskNumber"]);

                    cmd.ExecuteNonQuery();
                    
                }
            }
        }

        private void UpdateDatabaseForApprovals(DataRowView rowView, string whoApproval, int taskApproval)
        {
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}\Tasks.mdf; Integrated Security=True";
            string query = "UPDATE [Table] SET WhoApproval = @WhoApproval, TaskApproval = @TaskApproval WHERE TaskNumber = @TaskNumber";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WhoApproval", whoApproval ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TaskApproval", taskApproval);
                    cmd.Parameters.AddWithValue("@TaskNumber", rowView["TaskNumber"]);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void RefreshItems_CheckBox()
        {
            DataTable updatedData = GetUpdatedDataTable();
            DataUpdated?.Invoke(this, updatedData);
        }

        private DataTable GetUpdatedDataTable()
        {
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={Function_1.documentDirectory}\Tasks.mdf; Integrated Security=True";
            string query = "SELECT * FROM [Table]";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
        #endregion

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
