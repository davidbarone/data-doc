using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using data_doc_api.Lib;
using System.Linq;
using System.Text;
using data_doc_api.Models;
using System.Reflection;
using System;
using Dapper;

namespace data_doc_api
{
    /// <summary>
    /// Extention methods for Data-Doc
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Checks a collection of ParentChild objects, and returns whether there is a parent-child relationship between 2 values.
        /// </summary>
        /// <typeparam name="T">Type of each parent / child element</typeparam>
        /// <param name="rules">List of parent / child rules</param>
        /// <param name="ancestor">The ancestor value</param>
        /// <param name="current">The current value</param>
        /// <returns>Returns true of the ancestor is an ancestor of current</returns>
        public static bool HasAncestorRelationship<T>(this IEnumerable<ParentChild<T>> rules, T ancestor, T current)
        {
            if (rules.Any(r => r.Parent.Equals(ancestor) && r.Child.Equals(current)))
                return true;
            else
            {
                // recursively check parents
                var parents = rules.Where(r => r.Child.Equals(current)).Select(s => s.Parent);
                foreach (var parent in parents)
                {
                    if (rules.HasAncestorRelationship(ancestor, parent))
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the SQL DDL string for an attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string DataTypeDescEx(this AttributeInfo attribute)
        {
            List<string> charTypes = new List<string>() {
                    "char", "varchar", "nchar", "nvarchar", "varbinary", "binary"
                };
            List<string> decimalTypes = new List<string>() {
                    "decimal", "numeric"
                };
            var type = attribute.DataType.ToLower();
            if (charTypes.Contains(type))
            {
                return $"{attribute.DataType}({attribute.DataLength})";
            }
            else if (decimalTypes.Contains(type))
            {
                return $"{attribute.DataType}({attribute.Precision}, {attribute.Scale})";
            }
            else
            {
                return $"{attribute.DataType}";
            }
        }

        /// <summary>
        /// Pretty-prints a list of tree node objects that contain parent-child relationships
        /// </summary>
        /// <typeparam name="T">The type of the node</typeparam>
        /// <param name="node">The TreeNode list containing all the parent-child relationships</param>
        /// <param name="indent">The current indentation</param>
        /// <param name="isLastChild">Set to true if the last child in a set</param>
        /// <returns>A pretty-printed hierarchy</returns>
        public static string PrettyPrint<T>(this TreeNode<T> node, string indent = "", bool isLastChild = true)
        {
            var sb = new StringBuilder();
            sb.Append(indent + "+- " + node.Current + System.Environment.NewLine);
            indent += isLastChild ? "   " : "|  ";

            // children
            for (var i = 0; i < node.Children.Count(); i++)
            {
                var isLast = i == node.Children.Count() - 1;
                sb.Append(node.Children.ElementAt(i).PrettyPrint(indent, isLast));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a POCO dataset to a data table
        /// </summary>
        /// <typeparam name="T">The type of the IEnumerable</typeparam>
        /// <param name="entities">The set of POCO entity objects</param>
        /// <returns>A data table</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> entities)
        {
            var type = typeof(T);
            var props = type.GetProperties();
            DataTable dt = new DataTable();
            foreach (var prop in props)
            {
                var nullableUnderlyingType = System.Nullable.GetUnderlyingType(prop.PropertyType);
                if (nullableUnderlyingType != null)
                {
                    // Nullable type
                    dt.Columns.Add(prop.Name, nullableUnderlyingType);
                }
                else
                {
                    // Normal type
                    dt.Columns.Add(prop.Name, prop.PropertyType);
                }
            }

            foreach (var row in entities)
            {
                DataRow dr = dt.NewRow();
                foreach (var prop in props)
                {
                    dr[prop.Name] = prop.GetValue(row) ?? System.DBNull.Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Bulk copies a data table
        /// </summary>
        /// <param name="cn">The connection string</param>
        /// <param name="dt">The data table</param>
        /// <param name="destination">The destination table</param>
        /// <param name="timeout">The optional timeout, defaulting to 1 hour</param>
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


        /// <summary>
        /// This extension converts an enumerable set to a Dapper TVP
        /// </summary>
        /// <typeparam name="T">type of enumerbale</typeparam>
        /// <param name="enumerable">list of values</param>
        /// <param name="typeName">database type name</param>
        /// <param name="orderedColumnNames">if more than one column in a TVP, 
        /// columns order must mtach order of columns in TVP</param>
        /// <returns>a custom query parameter</returns>
        public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>(this IEnumerable<T> enumerable, string typeName, IEnumerable<string> orderedColumnNames = null)
        {
            var dataTable = new DataTable();
            if (typeof(T).IsValueType || typeof(T).FullName.Equals("System.String"))
            {
                dataTable.Columns.Add(orderedColumnNames == null ?
                    "NONAME" : orderedColumnNames.First(), typeof(T));
                foreach (T obj in enumerable)
                {
                    dataTable.Rows.Add(obj);
                }
            }
            else
            {
                PropertyInfo[] properties = typeof(T).GetProperties
                    (BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo[] readableProperties = properties.Where
                    (w => w.CanRead).ToArray();
                if (readableProperties.Length > 1 && orderedColumnNames == null)
                {
                    throw new Exception("Ordered list of column names must be provided when TVP contains more than one column");
                }

                var columnNames = (orderedColumnNames ??
                    readableProperties.Select(s => s.Name)).ToArray();
                foreach (string name in columnNames)
                {
                    dataTable.Columns.Add(name, readableProperties.Single
                        (s => s.Name.Equals(name)).PropertyType);
                }

                foreach (T obj in enumerable)
                {
                    dataTable.Rows.Add(
                        columnNames.Select(s => readableProperties.Single
                            (s2 => s2.Name.Equals(s)).GetValue(obj))
                            .ToArray());
                }
            }
            return dataTable.AsTableValuedParameter(typeName);
        }
    }
}