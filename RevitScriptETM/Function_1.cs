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
        public static string documentDirectory = @"\\192.168.53.190\bim\01-Объекты\DataBaseTasks\";
        public static string dbName;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            username = uidoc.Application.Application.Username;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            views = collector.OfClass(typeof(View)).Cast<View>().ToList();

            string BDTamplatePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dbName = DBName(doc);

            try
            {
                if (!File.Exists(documentDirectory + dbName))
                {
                    File.Copy(Path.Combine(BDTamplatePath, "Tasks.mdf"), Path.Combine(documentDirectory, $"{dbName}.mdf"), false);
                    File.Copy(Path.Combine(BDTamplatePath, "Tasks_log.ldf"), Path.Combine(documentDirectory, $"{dbName}_log.ldf"), false);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SqlConnection conn = new SqlConnection($@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {documentDirectory}\{DBName(doc)}.mdf; Integrated Security = True", null);
            MainMenu myWindow = new MainMenu();

            using (conn)
            {
                conn.Open();
                SqlCommand createCommand = new SqlCommand("SELECT * FROM [Table]", conn);
                createCommand.ExecuteNonQuery();
                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("Table"); // В скобках указываем название таблицы
                dataAdp.Fill(dt);
                myWindow.tasksDataGrid.ItemsSource = dt.DefaultView; // Сам вывод 

            }
            conn.Close();
            myWindow.ShowDialog();
            myWindow.RefreshItems();

            return Result.Succeeded; 
        }
        string DBName(Document doc)
        {
            string pathName = doc.PathName;
            int lastIndexSlash = pathName.LastIndexOf('\\');
            int lastIndexDot = pathName.LastIndexOf(".");
            string projectName = pathName.Substring(lastIndexSlash + 1, lastIndexDot - lastIndexSlash - 1);
            string dbName = projectName.Substring(0, projectName.IndexOf("-"));
            return dbName;
        }
    }
}
