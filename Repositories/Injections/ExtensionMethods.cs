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

        public static string TurnIntoSqlColumns(this Dictionary<string, string> values)
        {
            StringBuilder returnableResult = new StringBuilder();
            int index = 0;

            foreach (var item in values)
            {
                if (index == values.Values.Count - 1)
                {
                    returnableResult.Append($"{item.Key}");
                    break;
                }

                returnableResult.Append($"{item.Key}, ");
                index++;
            }

            return returnableResult.ToString();
        }

        public static string TurnIntoSqlValuesRow(this Dictionary<string, string> values)
        {
            StringBuilder returnableResult = new StringBuilder();
            int index = 0;

            foreach (var item in values)
            {
                if (index == values.Values.Count - 1)
                {
                    returnableResult.Append($"{item.Value}");
                    break;
                }

                returnableResult.Append($"{item.Value}, ");
                index++;
            }

            return returnableResult.ToString();
        }

        public static string TurnIntoSqlColumnValuePair(this Dictionary<string, string> tableValues)
        {
            StringBuilder returnableResult = new StringBuilder();
            int index = 0;

            foreach (var item in tableValues)
            {
                if (index == tableValues.Count - 1)
                {
                    returnableResult.Append($"{item.Key} = '{item.Value}'");
                    break;
                }

                returnableResult.Append($"{item.Key} = '{item.Value}', ");
                index++;
            }

            return returnableResult.ToString();
        }
    }
}
