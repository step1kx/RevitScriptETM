  using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using Microsoft.Win32;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Npgsql;
using System.Windows.Input;
using System.Linq.Expressions;
using System.Windows.Controls;


namespace RevitScriptETM
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class TasksCreator : Window
    {
        public event EventHandler<DataTable> TaskCreated;
        public string ImagePath { get; private set; }
        public string ImageName => ImagePath != null ? System.IO.Path.GetFileName(ImagePath) : string.Empty;
        public string ImageExtension => ImagePath != null ? System.IO.Path.GetExtension(ImagePath) : string.Empty;

        private static TasksCreator Window;



        public TasksCreator()
        {
            Window = this;
            InitializeComponent();
            foreach (View view in Function_1.views)
            {
                TaskViewComboBox.Items.Add(view.Name);
            }
        }

        private void MovingWin(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                TasksCreator.Window.DragMove();
            }
        }

        private void PasteImageFromClipboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Clipboard.ContainsImage())
                {
                    // Извлечение изображения из буфера обмена
                    var image = Clipboard.GetImage();
                    if (image != null)
                    {
                        // Сохранение изображения и получение пути
                        ImagePath = SaveImageToTempFile(image);
                        ImageInfoTextBlock.Text = $"{System.IO.Path.GetFileName(ImagePath)} ({System.IO.Path.GetExtension(ImagePath)})";
                    }
                }
                else
                {
                    MessageBox.Show("Буфер обмена не содержит изображения.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вставке изображения: {ex.Message}");
            }
        }

        private string SaveImageToTempFile(System.Windows.Media.Imaging.BitmapSource image)
        {
            string tempFilePath = System.IO.Path.GetTempFileName();
            tempFilePath = System.IO.Path.ChangeExtension(tempFilePath, ".png");

            using (var fileStream = new System.IO.FileStream(tempFilePath, System.IO.FileMode.Create))
            {
                var pngEncoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
                pngEncoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image));
                pngEncoder.Save(fileStream);
            }

            return tempFilePath;
        }

        private byte[] ConvertImageToBytes(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            return System.IO.File.ReadAllBytes(imagePath);
        }

        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if(ImagePath == null)
            {
                ImageInfoTextBlock.Text = null;
                MessageBox.Show("Изображение еще не добавлено");
            }
            else{
                ImagePath = null;
                ImageInfoTextBlock.Text = "Изображение удалено";
            }
            
        }

       

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FromSectionComboBox.Text) && TaskViewComboBox.SelectedItem != null)
            {
                try
                {
                    using (var conn = new NpgsqlConnection(dbSqlConnection.connString))
                    {
                        conn.Open();
                        byte[] imageBytes = ImagePath != null ? ConvertImageToBytes(ImagePath) : null;

                        NpgsqlCommand keyCommand = new NpgsqlCommand($"SELECT \"ProjectNumber\" FROM public.\"Projects\" WHERE \"ProjectName\" = '{Function_1.filename}'", conn);
                        int key = Convert.ToInt32(keyCommand.ExecuteScalar());

                        List<CheckBox> checkBoxes = new List<CheckBox> { CheckBoxAR, CheckBoxVK, CheckBoxOV, CheckBoxSS, CheckBoxES };
                        
                        foreach (var checkBox in checkBoxes)
                        {
                            if (checkBox.IsChecked == true)
                            {
                                NpgsqlCommand createCommand = new NpgsqlCommand(
                                    "INSERT INTO public.\"Table\" (\"FromSection\", \"ToSection\", \"TaskIssuer\", \"ScreenShot\", \"TaskDescription\", \"TaskView\", \"TaskCompleted\", \"TaskApproval\", \"TaskHandler\", \"WhoApproval\", \"TaskDate\", \"PK_ProjectNumber\") " +
                                    $"VALUES (@FromSection, @ToSection, @TaskIssuer, @ScreenShot, @TaskDescription, @TaskView, 0, 0, NULL, NULL, @TaskDate, {key} )", conn);

                                createCommand.Parameters.AddWithValue("@FromSection", FromSectionComboBox.SelectedItem.ToString());
                                createCommand.Parameters.AddWithValue("@ToSection", checkBox.Content.ToString());
                                createCommand.Parameters.AddWithValue("@TaskIssuer", Function_1.username);
                                createCommand.Parameters.AddWithValue("@ScreenShot", imageBytes ?? (object)DBNull.Value);
                                createCommand.Parameters.AddWithValue("@TaskDescription", DescriptionTextBox.Text);
                                createCommand.Parameters.AddWithValue("@TaskView", TaskViewComboBox.SelectedItem.ToString());
                                createCommand.Parameters.AddWithValue("@TaskDate", DateTime.Now.ToString("yyyy-MM-dd"));

                                createCommand.ExecuteNonQuery();
                            }
                        }

                        DataTable dt = new DataTable("public.\"Table\"");
                        TaskCreated?.Invoke(this, dt);
                        DialogResult = true;

                        conn.Close();
                        Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Вы не заполнили одно/несколько следующих полей:\nРаздел от кого\nРаздел кому\nИзображение\nВид");
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
