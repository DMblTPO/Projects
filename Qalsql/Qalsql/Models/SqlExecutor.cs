using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Qalsql.Models
{
    public class SqlStatus
    {
        public bool IsOk { get; set; }
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
            return new SqlResult { Query = sql, Status = new SqlStatus { IsOk = true, Message = String.Empty } };
        }

        public static SqlResult Fail(string sql, string message)
        {
            return new SqlResult { Query = sql, Status = new SqlStatus { IsOk = false, Message = message } };
        }
    }

    public static class SqlExecutor
    {
        static readonly string _conStr = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=qalight;Integrated Security=true";

        public static SqlResult SendQuery(string sql)
        {
            SqlResult res = CreatorSqlResult.Ok(sql);

            if (!SqlChecker(sql))
            {
                return CreatorSqlResult.Fail(sql, "A query should start from SELECT. There are no INSERT, UPDATE, DELETE.");
            }

            using (var connection = new SqlConnection(_conStr))
            {
                var sqlCmd = new SqlCommand(sql, connection) { CommandType = CommandType.Text };

                try
                {
                    connection.Open();
                    var reader = sqlCmd.ExecuteReader(/*CommandBehavior.KeyInfo*/);

                    var tbl = new DataTable();
                    tbl.Load(reader);

                    reader.Close();

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
            }

            return res;
        }

        static bool SqlChecker(string sql)
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