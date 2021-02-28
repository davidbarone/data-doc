using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace DataDoc
{

    public static class Extensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> entities)
        {
            var type = typeof(T);
            var props = type.GetProperties();
            DataTable dt = new DataTable();
            foreach (var prop in props)
            {
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var row in entities)
            {
                DataRow dr = dt.NewRow();
                foreach (var prop in props)
                {
                    dr[prop.Name] = prop.GetValue(row);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static void BulkCopy(this SqlConnection cn, DataTable dt, string destination, int timeout = 60 * 60)
        {
            SqlBulkCopy bcp = new SqlBulkCopy(cn);
            bcp.DestinationTableName = destination;
            bcp.BulkCopyTimeout = timeout;

            // Add in column Mappings to ensure load occurs by column name, not ordinal position
            foreach (System.Data.DataColumn column in dt.Columns)
            {
                bcp.ColumnMappings.Add(column.ColumnName, column.ColumnName);
            }
            bcp.WriteToServer(dt);
        }
    }
}