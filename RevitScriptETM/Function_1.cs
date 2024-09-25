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
        public static string filename;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            username = uidoc.Application.Application.Username;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            views = collector.OfClass(typeof(View)).Cast<View>().ToList();

            MainMenu myWindow = new MainMenu();

            try
            {
                filename = Path.GetFileName(doc.PathName).Substring(0, Path.GetFileName(doc.PathName).IndexOf("-"));

                using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
                {
                    conn.Open();  // Открываем соединение

                    string queryForDB = $"SELECT \"ProjectName\" FROM public.\"Projects\" WHERE \"ProjectName\"='{filename}'";
                    NpgsqlCommand cmd = new NpgsqlCommand(queryForDB, conn);
                    int UserCount = Convert.ToString(cmd.ExecuteScalar()).Length;
                    if (UserCount == 0)
                    {
                        queryForDB = $"INSERT INTO public.\"Projects\" (\"ProjectName\") VALUES ('{filename}')";
                        NpgsqlCommand cmd1 = new NpgsqlCommand(queryForDB, conn);
                        cmd1.ExecuteNonQuery();
                    }

                    string query = $"SELECT t.* " +
                        $"FROM public.\"Table\" t " +
                        $"JOIN public.\"Projects\" p ON t.\"PK_ProjectNumber\" = p.\"ProjectNumber\" " +
                        $"WHERE p.\"ProjectName\" = '{filename}'";

                    NpgsqlCommand cmd2 = new NpgsqlCommand(query, conn);
                    NpgsqlDataAdapter dataAdp = new NpgsqlDataAdapter(cmd2);
                    DataTable dt = new DataTable("Project");
                    dataAdp.Fill(dt);
                    myWindow.tasksDataGrid.ItemsSource = dt.DefaultView;
                }

                myWindow.ShowDialog();
                myWindow.RefreshItems();
            }
            catch (Exception e)
            {
                MessageBox.Show("Не верное наименование модели!" + $"\n{e.Message}");
            }

            return Result.Succeeded;
        }
    }
}
