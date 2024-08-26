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

            string document = doc.PathName;

            string connectionString = $"Data Source={document}";



            MessageBox.Show(document);

            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    // Создание таблицы, если она не существует
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

                    // Вставка данных
                    string insertQuery = @"
                    INSERT INTO TaskTable (TaskNumber, FromSection, ToSection, TaskIssuer, TaskCompleted, TaskHandler, TaskApproval, WhoApproval, ScreenShot, TaskDescription)
                    VALUES ($TaskNumber, $FromSection, $ToSection, $TaskIssuer, $TaskCompleted, $TaskHandler, $TaskApproval, $WhoApproval, $ScreenShot, $TaskDescription)";

                    using (SqliteCommand insertCommand = new SqliteCommand(insertQuery, connection))
                    {
                        // Пример данных для вставки
                        insertCommand.Parameters.AddWithValue("$TaskNumber", 1);
                        insertCommand.Parameters.AddWithValue("$FromSection", "Раздел А");
                        insertCommand.Parameters.AddWithValue("$ToSection", "Раздел Б");
                        insertCommand.Parameters.AddWithValue("$TaskIssuer", "Иванов");
                        insertCommand.Parameters.AddWithValue("$TaskCompleted", 1); // 1 для true
                        insertCommand.Parameters.AddWithValue("$TaskHandler", "Петров");
                        insertCommand.Parameters.AddWithValue("$TaskApproval", 1); // 1 для true
                        insertCommand.Parameters.AddWithValue("$WhoApproval", "Кузнецов");
                        insertCommand.Parameters.AddWithValue("$ScreenShot", "Screen1.png");
                        insertCommand.Parameters.AddWithValue("$TaskDescription", "Описание задания 1");

                        insertCommand.ExecuteNonQuery();
                    }
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }




        }
    }
}
