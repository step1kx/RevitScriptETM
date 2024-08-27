﻿using Autodesk.Revit.Attributes;
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
using System.Linq;
using System.Windows.Data;

namespace RevitScriptETM
{
    [Transaction(TransactionMode.Manual)]
    public class Function_1 : IExternalCommand
    {
        public static CollectionViewSource collview = new CollectionViewSource();
        public static List<TaskItems> taskItems = new List<TaskItems>();
        public static SqlConnection conn = null;
       

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
            //conn.Open();

            //SqlCommand cmd = new SqlCommand("SELECT * FROM [Table]", conn);
            //SqlDataReader reader = cmd.ExecuteReader();

            //SqlDataAdapter dataAdapter = new SqlDataAdapter(
            //    "SELECT * FROM [Table]", conn);
            //DataSet dataSet = new DataSet();
            //dataAdapter.Fill(dataSet);
            MainMenu myWindow = new MainMenu();

            
            using (conn)
            {
                conn.Open();
                SqlCommand createCommand = new SqlCommand("SELECT * FROM [Table]", conn);
                createCommand.ExecuteNonQuery();
                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("Table"); // В скобках указываем название таблицы
                dataAdp.Fill(dt);
                // Вывод на грид
                myWindow.tasksDataGrid.ItemsSource = dt.DefaultView; // Сам вывод 
                
            }




            //taskItems.Add(new TaskItems(
            //         1,"fdgdg","gdfgdfg","f234",0,"sdgs",1,"Я","","gfdgdgdfgdfg" 
            //     ));

            collview.Source = taskItems;
            collview.View.Refresh();


            
            myWindow.ShowDialog();


            myWindow.tasksDataGrid.ItemsSource = collview.View;
            myWindow.tasksDataGrid.Items.Refresh();

            
            return Result.Succeeded;
        }
    }
}
