using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        public static List<View> elem;

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

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection($@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {Function_1.documentDirectory}\Tasks.mdf; Integrated Security = True", null);
            using (conn)
            {
                
                conn.Open();
                //SqlCommand createCommand = new SqlCommand($"INSERT INTO [Table] (FromSection, ToSection, TaskIssuer, TaskCompleted, TaskHandler, TaskApproval, Screenshot, TaskDescription, TaskView, ) " +
                //    $"VALUES ('{FromSectionTextBox.Text}', '{ToSectionTextBox.Text}', '{Function_1.username}', 'DBNull.Value', '{DescriptionTextBox.Text}', '{Function_1.views}', 0, 0)", conn);
                SqlCommand createCommand = new SqlCommand($"INSERT INTO [Table] " +
    $"(FromSection, ToSection, TaskIssuer, TaskCompleted, TaskHandler, TaskApproval, WhoApproval, Screenshot, TaskDescription, TaskView) " +
    $"VALUES ('{FromSection}', '{ToSection}', '{Function_1.username}', 0, null, 0, null, null, '{Description}', '{Function_1.views}')", conn);
                MessageBox.Show(Function_1.username.ToString());
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
