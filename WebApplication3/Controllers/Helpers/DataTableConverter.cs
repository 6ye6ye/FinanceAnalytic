﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FinanceAnalytic.Controllers.Helpers
{
    public static class DataTableConverter
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            try
            {
                DataTable dt = new DataTable();
                Type t = typeof(T);
                PropertyInfo[] pia = t.GetProperties();

                //Inspect the properties and create the columns in the DataTable
                foreach (PropertyInfo pi in pia)
                {
                    Type ColumnType = pi.PropertyType;
                    if ((ColumnType.IsGenericType))
                    {
                        ColumnType = ColumnType.GetGenericArguments()[0];
                    }
                    dt.Columns.Add(pi.Name, ColumnType);
                }
                //Populate the data table
                foreach (T item in collection)
                {
                    DataRow dr = dt.NewRow();
                    dr.BeginEdit();
                    foreach (PropertyInfo pi in pia)
                    {
                        if (pi.GetValue(item, null) != null)
                        {
                            dr[pi.Name] = pi.GetValue(item, null);
                        }
                    }
                    dr.EndEdit();
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch { return null; }
            
        }
    }
}
