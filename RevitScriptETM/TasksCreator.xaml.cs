using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using Microsoft.Win32;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;


namespace RevitScriptETM
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class TasksCreator : Window
    {
        public string FromSection => FromSectionTextBox.Text;
        public string ToSection => ToSectionTextBox.Text;
        public string ImagePath { get; private set; }
        public string ImageName => ImagePath != null ? System.IO.Path.GetFileName(ImagePath) : string.Empty;
        public string ImageExtension => ImagePath != null ? System.IO.Path.GetExtension(ImagePath) : string.Empty;
        public string Description => DescriptionTextBox.Text;

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

        private string InsertNameFromRevit()
        {
            return string.Empty;
        }

        

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection($@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {Function_1.documentDirectory}\Tasks.mdf; Integrated Security = True", null);
            using (conn)
            {
                conn.Open();
                SqlCommand createCommand = new SqlCommand($"INSERT INTO [Table] (FromSection, ToSection, TaskIssuer, Screenshot, TaskDescription, TaskView, TaskCompleted, TaskApproval) " +
                    $"VALUES ('{FromSectionTextBox.Text}', '{ToSectionTextBox.Text}', '{Function_1.username}', NULL, '{DescriptionTextBox.Text}', N'{TaskViewComboBox.SelectedItem}', 0, 0)", conn);
                createCommand.ExecuteNonQuery();
                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("Table"); // В скобках указываем название таблицы
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
