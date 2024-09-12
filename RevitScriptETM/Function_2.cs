using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RevitScriptETM
{
    [Transaction(TransactionMode.Manual)]
    public class Function_2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            string pathName = doc.PathName;
            int lastIndexSlash = pathName.LastIndexOf('\\');
            int lastIndexDot = pathName.LastIndexOf(".");
            string projectName = pathName.Substring(lastIndexSlash+1, lastIndexDot - lastIndexSlash-1);
            string dbName = projectName.Substring(0, projectName.IndexOf("-"));

            MessageBox.Show(dbName);

            return Result.Succeeded; 
        }
    }
}
