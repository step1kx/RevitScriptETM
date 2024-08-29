using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using Microsoft.Win32;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Linq;


namespace RevitScriptETM
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class TasksCreator : Window
    {
        public string ImagePath { get; private set; }
        public string ImageName => ImagePath != null ? System.IO.Path.GetFileName(ImagePath) : string.Empty;
        public string ImageExtension => ImagePath != null ? System.IO.Path.GetExtension(ImagePath) : string.Empty;
        

        public TasksCreator()
        {
            InitializeComponent();
            foreach (View view in Function_1.views)
            {
                TaskViewComboBox.Items.Add(view.Name);
            }
        }

        private void ImportImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
                ImageInfoTextBlock.Text = $"{System.IO.Path.GetFileName(ImagePath)} ({System.IO.Path.GetExtension(ImagePath)})";
            }
        }

        //private string InsertNameFromRevit()
        //{
        //    return string.Empty;
        //}

        

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            //if (FromSectionTextBox.Text)
            //{
                SqlConnection conn = new SqlConnection($@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {Function_1.documentDirectory}\Tasks.mdf; Integrated Security = True", null);
                using (conn)
                {
                    conn.Open();
                    SqlCommand createCommand = new SqlCommand($"INSERT INTO [Table] (FromSection, ToSection, TaskIssuer, Screenshot, TaskDescription, TaskView, TaskCompleted, TaskApproval) " +
                        $"VALUES (N'{FromSectionTextBox.Text}', N'{ToSectionTextBox.Text}', N'{Function_1.username}', NULL, N'{DescriptionTextBox.Text}', N'{TaskViewComboBox.SelectedItem}', 0, 0)", conn);
                    createCommand.ExecuteNonQuery();
                    SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("Table"); // В скобках указываем название таблицы
                }
                DialogResult = true;
            //}
            //else MessageBox.Show("Зполните все обязательные поля");
            
 
            foreach (Window window in Application.Current.Windows)
            {
                MainMenu win;
                if (window is MainMenu)
                {
                    win = window as MainMenu;
                    win.RefreshItems();
                }
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
