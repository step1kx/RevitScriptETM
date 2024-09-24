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
using System.Collections;
using System.Windows.Forms.VisualStyles;

namespace RevitScriptETM
{
    [Transaction(TransactionMode.Manual)]
    public class Function_1 : IExternalCommand
    {
        public static string username;
        public static List<View> views;
        public static int key;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            username = uidoc.Application.Application.Username;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            views = collector.OfClass(typeof(View)).Cast<View>().ToList();

            string filename = Path.GetFileName(doc.PathName).Substring(0, Path.GetFileName(doc.PathName).IndexOf("-"));

            
            

            MessageBox.Show($"Имя: {filename}");

            MainMenu myWindow = new MainMenu();

            
            
            using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
            {
                conn.Open();  // Открываем соединение
                
                string selectKey = "SELECT \"ProjectNumber\" FROM public.\"Projects\"";
                string queryForDB = $"SELECT \"ProjectName\" FROM public.\"Projects\" WHERE \"ProjectName\"='{filename}'";
                NpgsqlCommand cmd = new NpgsqlCommand(queryForDB, conn);
                int UserCount = Convert.ToString(cmd.ExecuteScalar()).Length;
                MessageBox.Show($"{UserCount}");
                if (UserCount == 0)
                {
                    queryForDB = $"INSERT INTO public.\"Projects\" (\"ProjectName\") VALUES ('{filename}')";
                    NpgsqlCommand cmd1 = new NpgsqlCommand(queryForDB,conn);
                    cmd1.ExecuteNonQuery();
                }
                NpgsqlCommand cmd2 = new NpgsqlCommand(selectKey,conn);
                key = Convert.ToInt32(cmd2.ExecuteScalar());

                string query = $"SELECT * FROM public.\"Table\" WHERE \"PK_ProjectNumber\" = {key}";

                NpgsqlCommand cmd3 = new NpgsqlCommand(query, conn);
                NpgsqlDataAdapter dataAdp = new NpgsqlDataAdapter(cmd3);
                DataTable dt = new DataTable("Project");
                //dataAdp.Fill(dt);
                //myWindow.tasksDataGrid.ItemsSource = dt.DefaultView;
                conn.Close();
            }
            

            //try
            //{
            //    using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
            //    {
            //        conn.Open();  // Открываем соединение
            //        string query = "SELECT * FROM public.\"Table\"";
            //        NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            //        // Получаем данные    
            //        NpgsqlDataAdapter dataAdp = new NpgsqlDataAdapter(cmd);
            //        DataTable dt = new DataTable("Table");
            //        dataAdp.Fill(dt);
            //        myWindow.tasksDataGrid.ItemsSource = dt.DefaultView;
            //        conn.Close();
            //    }
            //}
            //catch (Exception)
            //{

                
            //}

            
            myWindow.ShowDialog();
            myWindow.RefreshItems();
            return Result.Succeeded;
        }

        
    }
}
