using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Qalsql.Models
{
    public class SqlStatus
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class SqlResult
    {
        public SqlStatus Status { get; set; }
        public string Query { get; set; }
        public List<string> Header { get; set; }
        public List<List<string>> Data { get; set; }
    }

    public static class CreatorSqlResult
    {
        public static SqlResult Ok(string sql)
        {
            return new SqlResult { Query = sql, Status = new SqlStatus { Success = true } };
        }

        public static SqlResult Fail(string sql, string message)
        {
            return new SqlResult { Query = sql, Status = new SqlStatus { Success = false, Message = message } };
        }
    }

    public static class SqlSender
    {
        static readonly string ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static async Task<DataTable> ExecAsync(string query)
        {
            using (var connection = new SqlConnection(ConStr))
            {
                await connection.OpenAsync();

                var sqlCmd = new SqlCommand(query, connection) {CommandType = CommandType.Text};
                var reader = await sqlCmd.ExecuteReaderAsync();

                var tbl = new DataTable();

                if (reader.HasRows)
                {
                    tbl.Load(reader);
                }

                return tbl;
            }
        }
        public static async Task<object> ExecScalarAsync(string query)
        {
            using (var connection = new SqlConnection(ConStr))
            {
                await connection.OpenAsync();

                var sqlCmd = new SqlCommand(query, connection) { CommandType = CommandType.Text };
                return await sqlCmd.ExecuteScalarAsync();
            }
        }

    }

    public static class SqlExecutor
    {
        public static async Task<SqlResult> SendQueryAsync(string sql)
        {
            SqlResult res = CreatorSqlResult.Ok(sql);

            if (!SqlValidate(sql))
            {
                return CreatorSqlResult.Fail(sql, "A query should start from SELECT. There are no INSERT, UPDATE, DELETE.");
            }

            try
            {
                var tbl = await SqlSender.ExecAsync(sql);

                res.Header =
                tbl.Columns
                    .Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();

                res.Data =
                tbl.Rows
                    .Cast<DataRow>()
                    .Select(r => r.ItemArray.Select(c =>
                    {
                        var f = c;
                        return f.ToString();
                    }).ToList())
                    .ToList();
            }
            catch (Exception ex)
            {
                return CreatorSqlResult.Fail(sql, ex.Message);
            }

            return res;
        }

        public static async Task<SqlResult> CheckQuery(string tobechk, string etalon)
        {
            if (!SqlValidate(tobechk))
            {
                return CreatorSqlResult.Fail(tobechk, "A query should start from SELECT. There are no INSERT, UPDATE, DELETE.");
            }

            string query =
                $"select top(1) 1 Result from (({etalon} except {tobechk})union all({tobechk} except {etalon}))z";

            try
            {
                dynamic scalar = await SqlSender.ExecScalarAsync(query);
                if (scalar > 0)
                {
                    return CreatorSqlResult.Fail(tobechk, "Wrong query: try again!");
                }
                return CreatorSqlResult.Ok(tobechk);
            }
            catch (Exception ex)
            {
                return CreatorSqlResult.Fail(tobechk, ex.Message);
            }
        }

        static bool SqlValidate(string sql)
        {
            var chk = sql.Trim().ToLower();
            bool res = chk.StartsWith("select") &&
                       !chk.Contains("delete") &&
                       !chk.Contains("update") &&
                       !chk.Contains("insert");
            return res;
        }
    }
}