Tested on ArcGIS Pro 2.1.3

Clicking the Button from the Addin ribbon will:

1. execute a Geoprocessing tool which uses the (legacy) arcpy InsertCursor to
   add a row to a table. (`/GPIssue/Button1.cs:25 ` ->
   `/Data/GPIssue/Toolbox.pyt` see `execute()`)
2. then try to insert a row into the same table using the arcpro .net sdk,
   which will fail. (`/GPIssue/Button1.cs:48`)
