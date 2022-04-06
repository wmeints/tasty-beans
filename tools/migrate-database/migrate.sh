MIRATION_FILES=/sql/*.sql

echo "Making sure database is available"
/opt/mssql-tools/bin/sqlcmd -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -Q "IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '$DB_NAME') BEGIN CREATE DATABASE [$DB_NAME]; END"

for migration_file in $MIRATION_FILES
do
    echo "Running migration file: $migration_file"
    /opt/mssql-tools/bin/sqlcmd -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -i $migration_file
done