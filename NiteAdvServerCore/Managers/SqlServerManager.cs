using NiteAdvServerCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace NiteAdvServerCore.Managers;


internal static class SqlServerManager
{
    private static string SqlServerConnection
    {
        get
        {
//#if DEBUG
            return $"Server=tcp:nephili.database.windows.net,1433;Initial Catalog=NiteOrchestrator;Persist Security Info=False;User ID=marco;Password=Motorol1275;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
//#else
//                return $"Server=tcp:nephili.database.windows.net,1433;Initial Catalog=NiteOrchestrator;Persist Security Info=False;User ID=marco;Password=Motorol1275;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
//#endif
        }
    }

    internal static List<Nation> GetNations()
    {
        DataTable dt1 = ExecuteCommand($"Select * from [dbo].Nation where Id>0");
        return BindList<Nation>(dt1);

    }

    internal static List<T> FreeQuery<T>(string sqlStatement) where T : class
    {
        DataTable dt1 = ExecuteCommand(sqlStatement);
        return BindList<T>(dt1);

    }

    internal static int? FreeQueryCount(string sqlStatement)
    {
        DataTable dt1 = ExecuteCommand(sqlStatement);
        return dt1.Rows[0].Field<int?>(0);

    }
    internal static List<T> GetCollection<T>(string WhereCondition = "") where T : Entity
    {
        string className = GetTableName<T>();
        string where = String.Empty;
        where = String.IsNullOrWhiteSpace(WhereCondition) ? String.Empty : String.Format(" where {0}", WhereCondition);
        string sqlString = String.Format("Select * from {0}.{1} {2}", DbSchema, className, where);
        DataTable dt1 = ExecuteCommand(sqlString);
        return BindList<T>(dt1);

    }
    internal static List<T> GetBaseCollection<T>(string WhereCondition = "") where T : BaseEntity
    {
        string className = GetTableName<T>();
        string where = String.Empty;
        where = String.IsNullOrWhiteSpace(WhereCondition) ? String.Empty : String.Format(" where {0}", WhereCondition);
        string sqlString = String.Format("Select * from {0}.{1} {2}", DbSchema, className, where);
        DataTable dt1 = ExecuteCommand(sqlString);
        return BindList<T>(dt1);

    }
    internal static int? GetCollectionCount<T>(string WhereCondition = "") where T : Entity
    {
        string className = GetTableName<T>();
        string where = String.Empty;
        where = String.IsNullOrWhiteSpace(WhereCondition) ? String.Empty : String.Format(" where {0}", WhereCondition);
        string sqlString = String.Format("Select Count(*) from {0}.{1} {2}", DbSchema, className, where);
        DataTable dt1 = ExecuteCommand(sqlString);
        return dt1.Rows[0].Field<int?>(0);

    }


    internal static T GetEntity<T>(string WhereCondition = "") where T : Entity
    {
        string className = GetTableName<T>();
        string  where = String.IsNullOrWhiteSpace(WhereCondition) ? String.Empty : String.Format(" where {0}", WhereCondition);
        string sqlString = String.Format("Select * from {0}.{1} {2}", DbSchema, className, where);
        DataTable dt1 = ExecuteCommand(sqlString);
        if (dt1 != null && dt1.Rows.Count > 0)
            return BindList<T>(dt1).FirstOrDefault();
        else
            return null;

    }
    internal static T GetBaseEntity<T>(string WhereCondition = "") where T : BaseEntity
    {
        string className = GetTableName<T>();
        string where = String.Empty;

       
            where = String.IsNullOrWhiteSpace(WhereCondition) ? String.Empty : String.Format(" where {0}", WhereCondition);

        string sqlString = String.Format("Select * from {0}.{1} {2}", DbSchema, className, where);
        DataTable dt1 = ExecuteCommand(sqlString);
        if (dt1 != null && dt1.Rows.Count > 0)
            return BindList<T>(dt1).FirstOrDefault();
        else
            return null;

    }
    internal static T Save<T>(T entity) where T : Entity
    {
        T result = null;
        int id;
        Tuple<string, int> pkReference;
        pkReference = GetPrimaryKey<T>(entity);
        if (pkReference.Item2 > 0)
        {
            var resUpdate = PerformUpdate<T>(entity);
            if (resUpdate == -1)
                throw new Exception("Errore in update del record");
            id = pkReference.Item2;
        }
        else
        {
            var resInsert = PerformInsert<T>(entity);
            if (resInsert == -1)
                throw new Exception("Errore in inserimento del record");
            id = GetPrimaryKeyValueCalculated<T>(pkReference.Item1, true);
        }
        result = GetEntity<T>(String.Format("{0}={1}", pkReference.Item1, id.ToString()));
        return result;
    }
   
    internal static bool Delete<T>(T entity) where T : Entity
    {
        try
        {
            Tuple<string, int> pkReference;
            pkReference = GetPrimaryKey<T>(entity);
            if (entity != null)
            {
                String commandString = String.Format("Delete from " + GetTableName<T>() + " where {0} = {1}", pkReference.Item1, pkReference.Item2);
                ExecuteCommand(commandString);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }



    #region "Private methods"
    private static string GetTableName<T>()
    {
        var entity = Activator.CreateInstance<T>();
        string result = typeof(T).Name;
        object[] attrs = entity.GetType().GetCustomAttributes(true);
        if (attrs != null && attrs.Any())
        {
            foreach( var attr in attrs)
            {
                if (attr.GetType() == typeof(TableNameAttribute))
                {
                    result = ((TableNameAttribute)attr).TableName;
                }
            }
        }
        return result;
    }
    private static string DbSchema
    {
        get
        {
            return "[dbo]";
        }
    }

  
    private static bool ManageAttributes<T>(Entity myEntity, PropertyInfo prop, object[] attributes, ref string resultString, ref string valueString, ref List<Tuple<string, object>> parameters) where T : Entity
    {
        try
        {
            foreach (var attr in attributes)
            {
                var propertyName = prop.Name;
                if (attr.GetType() == typeof(PrimaryKeyAttribute))
                {
                    var pkAttrType = ((PrimaryKeyAttribute)attr).TypePk;
                    if (pkAttrType == PrimaryKeyType.Autogenerated)
                    {
                        //non faccio nulla, ma lascio la chiamata perche forse va gestita
                    }
                    else if (pkAttrType == PrimaryKeyType.Calculated)
                    {
                        dynamic pk;
                        pk = GetPrimaryKeyValueCalculated<T>(propertyName, false);
                        if (pk != 0)
                        {
                            resultString += "[" + prop.Name + "],";
                            valueString += String.Format("@{0},", propertyName);
                            parameters.Add(new Tuple<string, object>(String.Format("@{0}", prop.Name), pk));
                        }

                    }
                }
                else if (attr.GetType() == typeof(SetValueAttribute))
                {

                    object val = null;
                    if (prop.PropertyType == typeof(String))
                        val = ((SetValueAttribute)attr).GetString;
                    else if (prop.PropertyType == typeof(DateTime))
                        val = ((SetValueAttribute)attr).GetDate;
                    else if (prop.PropertyType == typeof(byte[]))
                        val = ((SetValueAttribute)attr).GetBytes;
                    else if (prop.PropertyType == typeof(int))
                    {
                        var oldValue = prop.GetValue(myEntity, null);
                        val = ((SetValueAttribute)attr).GetInt((int?)oldValue);
                    }
                    resultString += "[" + prop.Name + "],";
                    valueString += String.Format("@{0},", propertyName);
                    parameters.Add(new Tuple<string, object>(String.Format("@{0}", prop.Name), val));

                }
                else if (attr.GetType() == typeof(SetValueIfNullAttribute))
                {
                    //@Marco: il valore nella property deve essere nullable
                    var valProperty = prop.GetValue(myEntity, null);
                    if (valProperty == null)
                    {
                        object val = null;
                        if (prop.PropertyType.FullName == typeof(String).FullName)
                            val = ((SetValueIfNullAttribute)attr).GetString;
                        else if (prop.PropertyType.FullName == typeof(DateTime?).FullName)
                            val = ((SetValueIfNullAttribute)attr).GetDate;
                        else if (prop.PropertyType.FullName == typeof(byte[]).FullName)
                            val = ((SetValueIfNullAttribute)attr).GetBytes;

                        resultString += "[" + prop.Name + "],";
                        valueString += String.Format("@{0},", propertyName);
                        parameters.Add(new Tuple<string, object>(String.Format("@{0}", prop.Name), val));
                    }

                }
                else if (attr.GetType() == typeof(IgnoreAttribute))
                {
                    //do nothing
                }
                else
                {
                    var val = prop.GetValue(myEntity, null);
                    if (val != null)
                    {
                        resultString += "[" + prop.Name + "],";
                        valueString += String.Format("@{0},", propertyName);
                        parameters.Add(new Tuple<string, object>(String.Format("@{0}", prop.Name), val));
                    }
                    //else
                    //    resultString += "[" + prop.Name + "]=NULL,";
                }


            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private static int GetPrimaryKeyValueCalculated<T>(string fieldName, bool lastValue)
    {
        string tableName = GetTableName<T>();
        string sqlString = String.Empty;
        DataTable dt1 = null;
        sqlString = String.Format("Select MAX({0}) from {1}.{2}", fieldName, DbSchema, tableName);
        dt1 = ExecuteCommand(sqlString);
        if (dt1 != null && dt1.Rows.Count > 0)
        {
            var valPk = dt1.Rows[0].Field<int?>(0);
            if (valPk == null)
                valPk = 0;
            return lastValue ? valPk.Value : valPk.Value + 1;
        }
        else
        {
            return lastValue ? 0 : 1;
        }

    }


    private static Tuple<string, int> GetPrimaryKey<T>(T myEntity) where T : Entity
    {
        var properties = myEntity.GetType().GetProperties().Where(p => p.GetCustomAttributes().OfType<PrimaryKeyAttribute>().Any()).ToList(); //.Select({p => p.Name})
        if (properties != null && properties.Any() && properties.Count() == 1)
        {
            var myProperty = properties[0];
            //return Tuple<string,int?>{myProperty.Name, (int?)myProperty.GetValue(myEntity, null)}
            dynamic myValue = null;
            myValue = myProperty.GetValue(myEntity, null);
            var valueToAdd = myValue == null ? 0 : (int)myValue;
            return new Tuple<string, int>(myProperty.Name, valueToAdd);
        }
        else
            throw new Exception("Non è possibilire stabilire entità con chiavi composite, SQLServerManager non lo supporta");

    }


    private static int PerformUpdate<T>(T entity) where T : Entity
    {
        String resultString = String.Empty;
        List<Tuple<String, Object>> parameters = new List<Tuple<string, object>>();
        Tuple<string, int> pkReference = GetPrimaryKey<T>(entity);
        resultString = "Update " + GetTableName<T>() + " set ";
        foreach (var prop in entity.GetType().GetProperties())
        {


            if (prop.Name != pkReference.Item1)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                if (attrs == null || !attrs.Any())
                {
                    var val = prop.GetValue(entity, null);
                    if (val != null)
                    {
                        resultString += "[" + prop.Name + "]=@" + prop.Name + ",";
                        parameters.Add(new Tuple<string, object>(String.Format("@{0}", prop.Name), val));
                    }
                    else
                        resultString += "[" + prop.Name + "]=NULL,";

                }
                else
                {
                    var valueString = string.Empty;
                    ManageAttributes<T>(entity, prop, attrs, ref resultString, ref valueString, ref parameters);
                    if (!String.IsNullOrWhiteSpace(valueString))
                    {
                        resultString = resultString.TrimEnd(',');
                        resultString += String.Format("={0}", valueString);
                    }
                }
            }


        }
        resultString = resultString.TrimEnd(',');
        resultString += String.Format(" Where {0}={1}", pkReference.Item1, pkReference.Item2.ToString());
        int resQuery = ExecuteNonQueryWithParameters(resultString, parameters);
        if (resQuery != null && resQuery > 0)
            return resQuery;
        else
            throw new Exception("Errore nella modifica dei dati"); // migliorare

    }



    private static int PerformInsert<T>(T entity) where T : Entity
    {
        String resultString = String.Empty;
        String valueString = "(";
        List<Tuple<String, Object>> parameters = new List<Tuple<string, object>>();
        resultString = "Insert into " + GetTableName<T>() + " (";
        var propList = entity.GetType().GetProperties();
        int counter = 0;
        foreach (var prop in propList)
        {

            object[] attrs = prop.GetCustomAttributes(true);
            if (attrs == null || !attrs.Any())
            {
                var val = prop.GetValue(entity, null);
                if (val != null)
                {
                    resultString += "[" + prop.Name + "],";
                    valueString += String.Format("@{0},", prop.Name);
                    parameters.Add(new Tuple<string, object>(String.Format("@{0}", prop.Name), prop.GetValue(entity, null)));

                }

                else
                {
                    resultString += "[" + prop.Name + "],";
                    valueString += "NULL,";
                }
            }
            else
                ManageAttributes<T>(entity, prop, attrs, ref resultString, ref valueString, ref parameters);

            counter += 1;
        }
        resultString = String.Format("{0})", resultString.TrimEnd(','));
        valueString = String.Format("{0})", valueString.TrimEnd(','));
        resultString = String.Format("{0} values {1}", resultString, valueString);
        var resQuery = ExecuteNonQueryWithParameters(resultString, parameters);
        if (resQuery > 0)
            return resQuery;
        else
            throw new Exception("Errore nell'inserimento dei dati"); // migliorare

    }

    
    private static List<T> BindList<T>(DataTable dt)
    {

        try
        {
            // Example 1:
            // Get private fields + non properties
            //var fields = typeof(T).GetFields(BindingFlags.Noninternal | BindingFlags.Instance);

            // Example 2: Your case
            // Get all internal fields
            var fields = typeof(T).GetProperties();

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            object value = dr[dc.ColumnName];
                            if (!(value is System.DBNull))
                                   fieldInfo.SetValue(ob, value);

                            break;
                        }
                    }
                }

                lst.Add(ob);
            }

            return lst;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    private static int ExecuteNonQuery(string sqlstring, List<Tuple<String, Object>> parameters)
    {
        try
        {
           
            SqlTransaction trans;
            int result = -1;
            using (var conn = new SqlConnection(SqlServerConnection))
            {
                SqlCommand command;
                conn.Open();
                trans = conn.BeginTransaction();
                using (command = new SqlCommand(sqlstring, conn))
                {
                    command.Transaction = trans;
                    if (parameters != null && parameters.Any())
                    {
                        foreach (Tuple<String, object> param in parameters)
                            command.Parameters.AddWithValue(param.Item1, param.Item2);
                    }


                    result = command.ExecuteNonQuery();
                }
                trans.Commit();
                conn.Close();
            }
            return result;
        }
        catch (Exception ex)
        {
            return -1;
        }

    }
   
    private static DataTable ExecuteCommand(string sqlstring)
    {
        try
        {
            var rex = new DataTable();
            using (var conn = new SqlConnection(SqlServerConnection))
            {
                SqlCommand command;
                SqlDataReader reader;
                conn.Open();

                command = new SqlCommand(sqlstring, conn);
                reader = command.ExecuteReader();
                rex.Load(reader);
                reader.Close();
                conn.Close();


            }
            return rex;
        }
        catch (Exception ex)
        {
            return null;
        }

    }


    private static object ExecuteScalar(string sqlstring)
    {
        try
        {
            object result = null;
            using (var conn = new SqlConnection(SqlServerConnection))
            {
                SqlCommand command;
                conn.Open();

                using (command = new SqlCommand(sqlstring, conn))
                {
                    result = command.ExecuteScalar();
                }
                conn.Close();
            }
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    public static int ExecuteNonQueryWithParameters(string sqlstring, List<Tuple<String, Object>> parameters)
    {
        //try @Marco: commentato per test per portare in UI le Exception
        SqlTransaction trans;
        int result = -1;
        using (var conn = new SqlConnection(SqlServerConnection))
        {

            SqlCommand command;
            conn.Open();
            trans = conn.BeginTransaction();
            using (command = new SqlCommand(sqlstring, conn))
            {
                command.Transaction = trans;
                if (parameters != null && parameters.Any())
                {
                    foreach (Tuple<String, object> param in parameters)
                    {
                        // command.Parameters.AddWithValue(param.Item1, param.Item2);
                        //il bug del russo
                        command.Parameters.Add(param.Item1, GetRightDbType(param.Item2)).Value = param.Item2;
                    }
                }
                result = command.ExecuteNonQuery();
            }
            trans.Commit();
            conn.Close();
        }
        return result;
    }
    private static SqlDbType GetRightDbType(object value)
    {
        if (value is Int32 || value is Int64 || value is Int32)
            return SqlDbType.Int;
        else if (value is String || value is string)
            return SqlDbType.NVarChar;
        else if (value is float || value is decimal)
            return SqlDbType.Decimal;
        else if (value is bool || value is Boolean)
            return SqlDbType.Bit;
        else if (value is byte[])
            return SqlDbType.VarBinary;
        else if (value is DateTime)
            return SqlDbType.DateTime;
        else
            return SqlDbType.NVarChar;
    }


    #endregion
}
