using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;
using System.Data.SqlClient;
using System.Windows;
using System.Reflection;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System;
using Org.BouncyCastle.Crypto;

namespace RevitScriptETM
{
    [Transaction(TransactionMode.Manual)]
    public class Function_1 : IExternalCommand
    {
        public static List<TaskItems> taskItems = new List<TaskItems>();
        private SqlConnection conn = null;
        string output = "";

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

            SqlCommand cmd = new SqlCommand("SELECT * FROM [Table]", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            //SqlDataAdapter dataAdapter = new SqlDataAdapter(
            //    "SELECT * FROM [Table]", conn);
            //DataSet dataSet = new DataSet();
            //dataAdapter.Fill(dataSet);
            //while (reader.Read()) // Перемещение к первой строке и далее
            //{
            //   output = String.Format("TaskNumber: {0}, FromSection {1}, ToSection {2}, " +
            //       "TaskIssure {3}, TaskComplited: {4}, TaskHandler {5}, TaskApproval {6}, WhoApproval {7}, Screenshot {7}, TaskDescription {7}",
            //        reader.GetInt32(reader.GetOrdinal("TaskNumber")),
            //        reader.GetString(reader.GetOrdinal("FromSection")),
            //        reader.GetString(reader.GetOrdinal("ToSection")),
            //        reader.GetString(reader.GetOrdinal("TaskIssure")),
            //        reader.GetInt32(reader.GetOrdinal("TaskComplited")),
            //        reader.GetString(reader.GetOrdinal("TaskHandler")),
            //        reader.GetInt32(reader.GetOrdinal("TaskApproval")),
            //        reader.GetString(reader.GetOrdinal("WhoApproval")),
            //        "", // ScreenShot
            //        reader.GetString(reader.GetOrdinal("TaskDescription"))
            //    );

            //}

            





            MainMenu myWindow = new MainMenu();
            myWindow.ShowDialog();

            myWindow.tasksDataGrid.ItemsSource = taskItems;

            MessageBox.Show(output);
            return Result.Succeeded;
        }
    }
}
