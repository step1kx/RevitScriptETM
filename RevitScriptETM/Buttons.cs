using Autodesk.Revit.UI;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;


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

            if (application.GetRibbonPanels(tabName).Count > 0)
            {
                panel = application.GetRibbonPanels(tabName).First();
            }
            else application.CreateRibbonTab(tabName);

            panel = application.CreateRibbonPanel(tabName, "Задания");

            panel.AddItem(new PushButtonData(nameof(Tasks), "Задания", assemblyLocation, typeof(Tasks).FullName)
            {
                //LargeImage = GetBitmapImage(Properties.Resources.CheckArhitects),
                LongDescription = "Выдача заданий для смежных разделов"
            });

            return Result.Succeeded;
        }
        BitmapImage GetBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
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
    }
}
