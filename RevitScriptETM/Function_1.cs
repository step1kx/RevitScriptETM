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

            string BDTamplatePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);



            MainMenu myWindow = new MainMenu();

            using (dbSqlConnection.conn)
            {
                dbSqlConnection.conn.Open();
                SqlCommand createCommand = new SqlCommand("SELECT * FROM [Table]", dbSqlConnection.conn);
                createCommand.ExecuteNonQuery();
                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("Table"); // В скобках указываем название таблицы
                dataAdp.Fill(dt);
                myWindow.tasksDataGrid.ItemsSource = dt.DefaultView; // Сам вывод 

            }
            dbSqlConnection.conn.Close();
            myWindow.ShowDialog();
            myWindow.RefreshItems();

            return Result.Succeeded; 
        }
        
    }
}
