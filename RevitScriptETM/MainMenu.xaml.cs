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
                rowView["TaskHandler"] = Function_1.username;
                UpdateDatabaseForHandlers(rowView, Function_1.username, 1);
                RefreshItems_CheckBox();
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
            

            string query = "UPDATE public.\"Table\" SET \"TaskHandler\" = @TaskHandler, \"TaskCompleted\" = @TaskCompleted WHERE \"TaskNumber\" = @TaskNumber";

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

        private void UpdateDatabaseForApprovals(DataRowView rowView, string whoApproval, int taskApproval)
        {
            string query = "UPDATE public.\"Table\" SET \"WhoApproval\" = @WhoApproval, \"TaskApproval\" = @TaskApproval WHERE \"TaskNumber\" = @TaskNumber";

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

        private void RefreshItems_CheckBox()
        {
            DataTable updatedData = GetUpdatedDataTable();
            DataUpdated?.Invoke(this, updatedData);
        }

        private DataTable GetUpdatedDataTable()
        {
            string query = "SELECT * FROM public.\"Table\"";

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
        #endregion

        public void RefreshItems()
        {
            string query = $"SELECT t.* " +
                    $"FROM public.\"Table\" t " +
                    $"JOIN public.\"Projects\" p ON t.\"PK_ProjectNumber\" = p.\"ProjectNumber\""; // Загрузка всех данных из таблицы

            using (var conn = new NpgsqlConnection(dbSqlConnection.connString))// try..catch
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    // Обновляем DataGrid
                    tasksDataGrid.ItemsSource = dataTable.DefaultView;
                }
                conn.Close();
            }
        }

        // Открытие картинки в полном размере
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
