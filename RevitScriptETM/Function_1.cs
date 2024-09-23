using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;
using System.Data.SqlClient;
using System.Windows;
using System.Reflection;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System;

namespace RevitScriptETM
{
    [Transaction(TransactionMode.Manual)]
    public class Function_1 : IExternalCommand
    {
        public static string username;
        public static List<View> views;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            username = uidoc.Application.Application.Username;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            views = collector.OfClass(typeof(View)).Cast<View>().ToList();


            MainMenu myWindow = new MainMenu();
            // Используем NpgsqlConnection для подключения к PostgreSQL
            //using (var conn = dbSqlConnection.connString)
            //{

            //    conn.Open();  // Открываем соединение



            //    // Выполняем SQL-запрос
            //    string query = "SELECT * FROM public.\"Table\"";
            //    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            //    // Получаем данные
            //    NpgsqlDataAdapter dataAdp = new NpgsqlDataAdapter(cmd);
            //    DataTable dt = new DataTable("public.\"Table\""); // Указываем название таблицы
            //    dataAdp.Fill(dt);

            //    // Отображаем данные в DataGrid
            //    myWindow.tasksDataGrid.ItemsSource = dt.DefaultView;
            //}

            using (var conn = dbSqlConnection.connString)
            {
                try
                {
                    conn.Open();  // Открываем соединение

                    // Выполняем SQL-запрос
                    string query = "SELECT * FROM public.\"Table\"";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                    // Получаем данные
                    NpgsqlDataAdapter dataAdp = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable("public.\"Table\""); // Указываем название таблицы
                    dataAdp.Fill(dt);

                    // Отображаем данные в DataGrid
                    myWindow.tasksDataGrid.ItemsSource = dt.DefaultView;
                }
                catch (NpgsqlException ex)
                {
                    // Обработка ошибок, связанных с PostgreSQL
                    MessageBox.Show($"Ошибка подключения или выполнения запроса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    // Обработка других ошибок
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }



            myWindow.ShowDialog();
            myWindow.RefreshItems();

            return Result.Succeeded;
        }
    }
}
