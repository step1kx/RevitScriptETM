using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Npgsql;
using Mysqlx.Crud;

namespace RevitScriptETM
{
    public partial class MainMenu : Window
    {
        public event EventHandler<DataTable> DataUpdated;
        public static MainMenu Window;
        public MainMenu()
        {
            InitializeComponent();
            tasksDataGrid.CanUserAddRows = false;
            Window = this;
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
        }

        private void MovingWin(object sender, EventArgs e)
        {
            if(Mouse.LeftButton == MouseButtonState.Pressed)
            {
                MainMenu.Window.DragMove();
            }
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
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterWindow filterWindow = new FilterWindow();
            filterWindow.FilterDone += FilterRefresh;
            filterWindow.ShowDialog();
        }

        private void FilterRefresh(object sender, DataTable e)
        {
            
            tasksDataGrid.ItemsSource = e.DefaultView;
        }

        #region Чекбоксы. Кто выполнил задание
        private void TaskCompleted_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.DataContext is DataRowView rowView)
            {
                if (rowView["TaskHandler"] == DBNull.Value || string.IsNullOrEmpty(rowView["TaskHandler"].ToString()))
                {
                    rowView["TaskHandler"] = Function_1.username;
                    UpdateDatabaseForHandlers(rowView, Function_1.username, 1);
                    RefreshItems_CheckBox();
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
                if (rowView["WhoApproval"] == DBNull.Value || string.IsNullOrEmpty(rowView["WhoApproval"].ToString()))
                {
                    rowView["WhoApproval"] = Function_1.username;
                    UpdateDatabaseForApprovals(rowView, Function_1.username, 1);
                    RefreshItems_CheckBox();
                }
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

        private void TaskTaken_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is DataRowView rowView)
            {
                if (rowView["WhoTaken"] == DBNull.Value || string.IsNullOrEmpty(rowView["WhoTaken"].ToString()))
                {
                    rowView["WhoTaken"] = Function_1.username; 
                    UpdateDatabaseForTaskTakens(rowView, Function_1.username, 1); 
                    RefreshItems_CheckBox(); 
                }
            }
        }

        private void TaskTaken_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is DataRowView rowView)
            {
                rowView["WhoTaken"] = DBNull.Value; // Обновляем значение в модели
                UpdateDatabaseForTaskTakens(rowView, null, 0); // Обновляем значение в базе данных
                RefreshItems_CheckBox();
            }
        }

        public void CheckBoxTaken_Indeterminate(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is DataRowView rowView)
            {
                rowView["WhoTaken"] = DBNull.Value;
                UpdateDatabaseForTaskTakens(rowView, null, 2);
                RefreshItems_CheckBox();
            }
        }

        private void UpdateDatabaseForHandlers(DataRowView rowView, string taskHandler, int taskCompleted)
        {
            string query = "UPDATE public.\"Table\" SET \"TaskHandler\" = @TaskHandler, \"TaskCompleted\" = @TaskCompleted WHERE \"TaskNumber\" = @TaskNumber";
            try
            {
                using (var conn = new NpgsqlConnection(dbSqlConnection.connString))// try..catch
                {

                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskHandler", taskHandler ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TaskCompleted", taskCompleted);
                        cmd.Parameters.AddWithValue("@TaskNumber", rowView["TaskNumber"]);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void UpdateDatabaseForApprovals(DataRowView rowView, string whoApproval, int taskApproval)
        {
            string query = "UPDATE public.\"Table\" SET \"WhoApproval\" = @WhoApproval, \"TaskApproval\" = @TaskApproval WHERE \"TaskNumber\" = @TaskNumber";

            try
            {
                using (var conn = new NpgsqlConnection(dbSqlConnection.connString))// try..catch
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@WhoApproval", whoApproval ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TaskApproval", taskApproval);
                        cmd.Parameters.AddWithValue("@TaskNumber", rowView["TaskNumber"]);//!!!!
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
        }

        private void UpdateDatabaseForTaskTakens(DataRowView rowView,string whoTaken, int taskTaken)
        {
            string query = "UPDATE public.\"Table\" SET \"WhoTaken\" = @WhoTaken, \"TaskTaken\" = @TaskTaken WHERE \"TaskNumber\" = @TaskNumber";

            try
            {
                using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
                {
                    conn.Open();
                    using(NpgsqlCommand cmd = new NpgsqlCommand(query,conn))
                    {
                        cmd.Parameters.AddWithValue("@WhoTaken", whoTaken ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TaskTaken", taskTaken);
                        cmd.Parameters.AddWithValue("@TaskNumber", rowView["TaskNumber"]);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            RefreshItems_CheckBox();
        }

        private void TaskExplanationTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox && textBox.DataContext is DataRowView rowView)
            {
                // Обновляем значение в модели
                rowView["TaskExplanation"] = textBox.Text;

                // Обновляем значение в базе данных
                UpdateDatabaseForTaskExplanation(rowView);
            }
        }

        private void UpdateDatabaseForTaskExplanation(DataRowView rowView)
        {
            string query = "UPDATE public.\"Table\" SET \"TaskExplanation\" = @TaskExplanation WHERE \"TaskNumber\" = @TaskNumber";

            try
            {
                using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskExplanation", rowView["TaskExplanation"] ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TaskNumber", rowView["TaskNumber"]);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}");
            }
            // Обновляем элементы DataGrid
            RefreshItems();
        }

        private void RefreshItems_CheckBox()
        {
            DataTable updatedData = GetUpdatedDataTable();
            DataUpdated?.Invoke(this, updatedData);
        }

        private DataTable GetUpdatedDataTable()
        {
            string query = "SELECT * FROM public.\"Table\"";
            try
            {
                using (var conn = new NpgsqlConnection(dbSqlConnection.connString))// try..catch
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion


        public void RefreshItems()
        {
            int projectNumber;
            using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
            {
                conn.Open();
                string projectQuery = $"SELECT \"ProjectNumber\" FROM public.\"Projects\" WHERE \"ProjectName\" = @ProjectName";
                using (var projectCmd = new NpgsqlCommand(projectQuery, conn))
                {
                    projectCmd.Parameters.AddWithValue("@ProjectName", Function_1.filename);
                    projectNumber = Convert.ToInt32(projectCmd.ExecuteScalar());
                }
                conn.Close();
            }

            // Запрос для загрузки всех задач только для текущего проекта
            string query = $"SELECT t.* " +
                           $"FROM public.\"Table\" t " +
                           $"JOIN public.\"Projects\" p ON t.\"PK_ProjectNumber\" = p.\"ProjectNumber\" " +
                           $"WHERE t.\"PK_ProjectNumber\" = @ProjectNumber";

            try
            {
                using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProjectNumber", projectNumber);

                        NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Обновляем DataGrid
                        tasksDataGrid.ItemsSource = dataTable.DefaultView;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Открытие картинки в полном размере
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                var imageSource = image.Source as BitmapImage;
                if (imageSource != null)
                {
                    ShowImageInNewWindow(imageSource);
                }
            }
        }
        private void ShowImageInNewWindow(BitmapImage imageSource)
        {
            var imageWindow = new ImageFullSize();
            imageWindow.SetImageSource(imageSource);
            imageWindow.ShowDialog();
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;//!!!
            Close();
        }

        
    }
}
