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
        public static string documentDirectory;


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            username = uidoc.Application.Application.Username;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            views = collector.OfClass(typeof(View)).Cast<View>().ToList();

            documentDirectory = Path.GetDirectoryName(doc.PathName);
            string BDTamplatePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            try
            {
                if (!File.Exists(documentDirectory + @"\Tasks.mdf"))
                {
                    File.Copy(Path.Combine(BDTamplatePath, "Tasks.mdf"), Path.Combine(documentDirectory, "Tasks.mdf"), false);
                    File.Copy(Path.Combine(BDTamplatePath, "Tasks_log.ldf"), Path.Combine(documentDirectory, "Tasks_log.ldf"), false);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SqlConnection conn = new SqlConnection($@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {documentDirectory}\Tasks.mdf; Integrated Security = True", null);
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
            myWindow.ShowDialog();
            
            return Result.Succeeded;
        }
    }
}
