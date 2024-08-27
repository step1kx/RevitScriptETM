using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace RevitScriptETM
{
    internal class DataFromRevit_Event : IExternalEventHandler
    {
        public string username;
        public List<View> elements;
        void IExternalEventHandler.Execute(UIApplication app)
        {
            username = app.Application.Username;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            elements = collector.OfClass(typeof(View)).Cast<View>().ToList();
            MessageBox.Show(elements.Count.ToString()+"это event");
            TasksCreator.elem = elements;
        }

        public string GetUserName(string Username)
        {
            string username = Username;
            return username; 

        }

        string IExternalEventHandler.GetName()
        {
            return "DataFromRevit_Event";
        }
    }
}
