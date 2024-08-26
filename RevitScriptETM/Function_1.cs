using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;
using System.Data.SqlClient;
using System.Windows;
using System.Reflection;

namespace RevitScriptETM
{
    [Transaction(TransactionMode.Manual)]
    public class Function_1 : IExternalCommand
    {
        private SqlConnection conn = null;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            string documentDirectory = Path.GetDirectoryName(doc.PathName);
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

            conn = new SqlConnection($@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {documentDirectory}\Tasks.mdf; Integrated Security = True", null);
            conn.Open();

            MessageBox.Show(conn.State.ToString());

            MainMenu myWindow = new MainMenu();
            myWindow.ShowDialog();


            return Result.Succeeded;
        }
    }
}
