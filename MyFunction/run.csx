#r "Newtonsoft.Json"
#r "System.Configuration"

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Data;
using System.Net;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    string response;

    try
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.Process);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = $"SELECT * FROM [MyLocalDB].[dbo].[Table1] WHERE [Column1] = 1;";
            var command = new SqlCommand(query, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                var dataTable = new DataTable();
                dataTable.Load(reader);
                var values = new List<string>();

                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Console.WriteLine(row[column]);
                    }
                }

                response = string.Join(",", values);
            }

            connection.Close();
        }
    }
    catch (Exception exception)
    {
        return new OkObjectResult(exception.Message);
    }

    return new OkObjectResult(response);
}