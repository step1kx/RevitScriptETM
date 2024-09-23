﻿using System.Collections.Generic;
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



        public TasksCreator()
        {
            InitializeComponent();
            foreach (View view in Function_1.views)
            {
                TaskViewComboBox.Items.Add(view.Name);
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
            if (FromSectionTextBox.Text != "" && ToSectionTextBox.Text != "" && TaskViewComboBox.SelectedItem != null)
            {
                using (var conn = dbSqlConnection.connString)
                {
                    dbSqlConnection.connString.Open();
                    byte[] imageBytes = ImagePath != null ? ConvertImageToBytes(ImagePath) : null;

                    // Используем NpgsqlCommand для выполнения запроса
                    NpgsqlCommand createCommand = new NpgsqlCommand(
                        "INSERT INTO \"Table\" (\"FromSection\", \"ToSection\", \"TaskIssuer\", \"Screenshot\", \"TaskDescription\", \"TaskView\", \"TaskCompleted\", \"TaskApproval\", \"TaskHandler\", \"WhoApproval\", \"TaskDate\") " +
                        "VALUES (@FromSection, @ToSection, @TaskIssuer, @Screenshot, @TaskDescription, @TaskView, 0, 0, NULL, NULL, @TaskDate)", dbSqlConnection.connString);

                    // Добавляем параметры
                    createCommand.Parameters.AddWithValue("@FromSection", FromSectionTextBox.Text);
                    createCommand.Parameters.AddWithValue("@ToSection", ToSectionTextBox.Text);
                    createCommand.Parameters.AddWithValue("@TaskIssuer", Function_1.username);
                    createCommand.Parameters.AddWithValue("@Screenshot", imageBytes ?? (object)DBNull.Value); // Передаем байты изображения или NULL
                    createCommand.Parameters.AddWithValue("@TaskDescription", DescriptionTextBox.Text);
                    createCommand.Parameters.AddWithValue("@TaskView", TaskViewComboBox.SelectedItem.ToString());
                    createCommand.Parameters.AddWithValue("@TaskDate", DateTime.Now.ToString("yyyy-MM-dd")); // Формат для PostgreSQL

                    // Выполняем команду
                    createCommand.ExecuteNonQuery();

                    // Оповещаем, что задание создано
                    DataTable dt = new DataTable("Table");
                    TaskCreated?.Invoke(this, dt);
                    DialogResult = true;

                    dbSqlConnection.connString.Close();
                    Close();
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
