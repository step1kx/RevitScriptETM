using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using System.IO;

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




            string documentDirectory = Path.GetDirectoryName(doc.PathName);
            string dbPath = Path.Combine(documentDirectory, "MyDatabase.db");

            // Проверяем, существует ли файл базы данных
            if (!File.Exists(dbPath))
            {
                // Создание пустого файла базы данных, если его нет
                File.Create(dbPath).Close();
            }



            MessageBox.Show($"{dbPath}");

            string connectionString = "Server=localhost;Database=TaskDatabase;User ID=root;Password=yourpassword;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Создание таблицы, если она не существует
                    string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS TaskTable (
                        TaskNumber INT PRIMARY KEY,
                        FromSection VARCHAR(255),
                        ToSection VARCHAR(255),
                        TaskIssuer VARCHAR(255),
                        TaskCompleted BOOLEAN,
                        TaskHandler VARCHAR(255),
                        TaskApproval BOOLEAN,
                        WhoApproval VARCHAR(255),
                        ScreenShot VARCHAR(255),
                        TaskDescription TEXT
                    )";

                    using (MySqlCommand createTableCommand = new MySqlCommand(createTableQuery, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }

                    // Вставка данных
                    string insertQuery = @"
                    INSERT INTO TaskTable (TaskNumber, FromSection, ToSection, TaskIssuer, TaskCompleted, TaskHandler, TaskApproval, WhoApproval, ScreenShot, TaskDescription)
                    VALUES (@TaskNumber, @FromSection, @ToSection, @TaskIssuer, @TaskCompleted, @TaskHandler, @TaskApproval, @WhoApproval, @ScreenShot, @TaskDescription)";

                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        // Пример данных для вставки
                        insertCommand.Parameters.AddWithValue("@TaskNumber", 1);
                        insertCommand.Parameters.AddWithValue("@FromSection", "Раздел А");
                        insertCommand.Parameters.AddWithValue("@ToSection", "Раздел Б");
                        insertCommand.Parameters.AddWithValue("@TaskIssuer", "Иванов");
                        insertCommand.Parameters.AddWithValue("@TaskCompleted", true);
                        insertCommand.Parameters.AddWithValue("@TaskHandler", "Петров");
                        insertCommand.Parameters.AddWithValue("@TaskApproval", true);
                        insertCommand.Parameters.AddWithValue("@WhoApproval", "Кузнецов");
                        insertCommand.Parameters.AddWithValue("@ScreenShot", "Screen1.png");
                        insertCommand.Parameters.AddWithValue("@TaskDescription", "Описание задания 1");

                        insertCommand.ExecuteNonQuery();
                    }

                    return Result.Succeeded;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return Result.Failed;
            }




        }
    }
}
