﻿using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System;
using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;


namespace RevitScriptETM
{
    internal class Buttons : IExternalApplication
    {
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        string tabName = "ETM";
       

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel panel;
            try
            {
                panel = application.GetRibbonPanels(tabName).First();
            }
            catch (Exception)
            {
                application.CreateRibbonTab(tabName);
            }

            panel = application.CreateRibbonPanel(tabName, "Задания");

            panel.AddItem(new PushButtonData(nameof(Function_1), "Задания для\n смежных разделов", assemblyLocation, typeof(Function_1).FullName)
            {
                LargeImage = GetBitmapImage(Properties.Resources.ETMLogo, 32, 32),
                LongDescription = "Программа для создания задания смежным разделам\n" +
                                  "Она позволяет просматривать, создавать и фильтровать задания в определенном проекте"
            });

            return Result.Succeeded;
        }

        BitmapImage GetBitmapImage(Bitmap bitmap, int width, int height)
        {
            // Создание нового битмапа с заданными размерами
            var resizedBitmap = new Bitmap(bitmap, new Size(width, height));

            using (var memory = new MemoryStream())
            {
                resizedBitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
        //BitmapImage GetBitmapImage(Bitmap bitmap)
        //{
        //    using (var memory = new MemoryStream())
        //    {
        //        bitmap.Save(memory, ImageFormat.Png);
        //        memory.Position = 0;

        //        var bitmapImage = new BitmapImage();
        //        bitmapImage.BeginInit();
        //        bitmapImage.StreamSource = memory;
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmapImage.EndInit();
        //        bitmapImage.Freeze();

        //        return bitmapImage;
        //    }
        //}
    }
}
