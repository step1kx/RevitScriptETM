using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

using System.Windows.Forms;

namespace RevitScriptETM
{
    [Transaction(TransactionMode.Manual)]
    internal class Tasks : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MessageBox.Show("Ok");
            return Result.Succeeded;
        }
    }
}
