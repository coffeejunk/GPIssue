using System;
using static System.Diagnostics.Debug;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;

namespace GPIssue
{
    internal class Button1 : Button
    {
        protected override async void OnClick()
        {
            // Toolbox adds a row to the table
            var tbxPath = Project.Current.HomeFolderPath + @"\Toolbox.pyt\Tool";
            var gpResult = await QueuedTask.Run(() => Geoprocessing.ExecuteToolAsync(tbxPath, Geoprocessing.MakeValueArray()));

            // GP succeeds
            Geoprocessing.ShowMessageBox(gpResult.Messages, "GP Messages", gpResult.IsFailed ? GPMessageBoxStyle.Error : GPMessageBoxStyle.Default);

            // has no effect
            await Project.Current.SaveEditsAsync();
            // has also no effect
            await Project.Current.SaveAsync();

            // write to table
            var createRow = new EditOperation { Name = "Create Row in Table" };
            bool writeSucceeded = false;
            await QueuedTask.Run(() =>
            {
                using (var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(Project.Current.DefaultGeodatabasePath))))
                using (var table = geodatabase.OpenDataset<Table>("TestTable"))
                {
                    var attributes = new Dictionary<string, object> { { "Something", "Something from Pro" } };

                    createRow.Create(table, attributes);

                    // will fail
                    writeSucceeded = createRow.Execute();
                }
            });

            MessageBox.Show("EditOperation() was " + (writeSucceeded ? "successful" : "has failed") + " \n" + createRow.ErrorMessage);
        }
    }
}
