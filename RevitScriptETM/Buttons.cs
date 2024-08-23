using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System;
using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;

namespace RevitScriptETM
{
    internal class Buttons : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location,
                   iconDirectoryPath = Path.GetDirectoryName(assemblyLocation) + @"\icons\",
                   tabName = "ETM";

            application.CreateRibbonTab(tabName);
           
            // Создаем панель на вкладке
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Проверка таблицы");

            // Создаем кнопку для панели
            PushButtonData buttonData = new PushButtonData("MyButton", "Click Me", assemblyLocation, "RevitScriptETM.Command")
            {
                LargeImage = new BitmapImage(new Uri(iconDirectoryPath + "green.png"))
            };

            panel.AddItem(buttonData);

         
           

            return Result.Succeeded;
        }
    }
}
