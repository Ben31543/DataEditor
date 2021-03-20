using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Data;
using Repositories.Interfaces;
using Repositories.Repositories;
using System.Data;
using System.Text;

namespace Repositories.Injections
{
    public static class ExtensionMethods
    {
        public static void AddRepositoryInjections(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(new DataContext(new DbContextOptions<DataContext>(), connectionString));
            services.AddTransient<IDataRepository, DataRepository>();
        }

        public static string TurnIntoSqlColumns(this List<string> columns)
        {
            StringBuilder returnableResult = new StringBuilder();

            for (int i = 1; i < columns.Count; i++)
            {
                string res = columns[i];

                if (i == columns.Count - 1)
                {
                    returnableResult.Append(res);
                    break;
                }

                returnableResult.Append(res + ", ");
            }
            return returnableResult.ToString();
        }

        public static string TurnIntoSqlValuesRow(List<object> values)
        {
            StringBuilder returnableResult = new StringBuilder();

            for (int i = 1; i < values.Count; i++)
            {
                #region Other variant
                //Type dataType = values[i].GetType();
                //string res;

                //switch (dataType.ToString())
                //{
                //    case "System.Int32":
                //        res = values[i].ToString();
                //        break;
                //    case "System.String":
                //        res = $"'{values[i]}'";
                //        break;
                //    case "System.Char":
                //        res = $"'{values[i]}'";
                //        break;
                //    case "System.DateTime":
                //        res = $"'{values[i]}'";
                //        break;
                //    default:
                //        res = values[i].ToString();
                //        break;
                //}
                #endregion

                if (i == values.Count - 1)
                {
                    returnableResult.Append($"'{values[i]}'");
                    break;
                }

                returnableResult.Append($"'{values[i]}'" + ", ");
            }
            return returnableResult.ToString();
        }

        public static string TurnIntoSqlColumnValuePair(List<string> columns, List<object> values)
        {
            StringBuilder returnableResult = new StringBuilder();

            for (int i = 0; i < columns.Count; i++)
            {
                if (i == columns.Count - 1 || i == values.Count - 1)
                {
                    returnableResult.Append($"{columns[i]} = '{values[i]}';");
                    break;
                }

                returnableResult.Append($"{columns[i]} = '{values[i]}', ");
            }

            return returnableResult.ToString();
        }
    }
}
