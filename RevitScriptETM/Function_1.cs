using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace RevitScriptETM
{
    [Transaction(TransactionMode.Manual)]
    public class Function_1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            MainMenu myWindow = new MainMenu();
            myWindow.ShowDialog();

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            string document = doc.ActiveProjectLocation.Location.ToString();

            string connectionString = $"Data Source={document};Version=3;";

            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();


                    string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS TaskTable (
                        TaskNumber INTEGER PRIMARY KEY,
                        FromSection TEXT,
                        ToSection TEXT,
                        TaskIssuer TEXT,
                        TaskCompleted INTEGER,
                        TaskHandler TEXT,
                        TaskApproval INTEGER,
                        WhoApproval TEXT,
                        ScreenShot TEXT,
                        TaskDescription TEXT
                    )";

                    using (SqliteCommand createTableCommand = new SqliteCommand(createTableQuery, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }


                    
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Result.Succeeded;
        }
    }
}
