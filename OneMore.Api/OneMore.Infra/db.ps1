echo ""
echo ""
echo ""
echo ------------------------------------------------------
echo "Initializing the migration and creating SQL script ..."
$apiFolder = "../OneMore.Api/"
$now = Get-Date -UFormat "%d%m%Y_%Hh%Mm%Ss"
$sqlFilename = "OneMore" + $now + ".sql"
del dummy?.sql
echo ------------------------------------------------------
echo $now
echo $sqlFilename
echo ------------------------------------------------------
dotnet ef migrations add -s $apiFolder $now
echo ------------------------------------------------------
dotnet ef database update -s $apiFolder
echo ------------------------------------------------------
dotnet ef migrations script -s $apiFolder -o dummy2.sql
echo ------------------------------------------------------
echo "Creating $sqlFilename ..."
echo "USE onemore;" > dummy1.sql
Get-Content dummy1.sql, dummy2.sql | Set-Content $sqlFilename
del dummy1.sql
del dummy2.sql
echo ------------------------------------------------------
echo Done.