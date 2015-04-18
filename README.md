# SQLiteDB
Basics operations for SQLite-C# Framework 4.5

Contains basics operations:

void Insert(string tablename, params object[] values);
void Delete(string tablename, string whereclause = null);
void Update(string tablename, string whereclause, object[] values);
void DataTable GetTable(string tablename, string whereclause, params object[] fields);
object GetValue(string tablename, string field, string whereclause = null);

and generical:

DataTable GetTableFromQuery(string statment);
public void ExecuteQuery(string statment);
