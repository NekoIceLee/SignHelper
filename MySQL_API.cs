using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;

namespace SignHelper;

public class MySQLAPI
{
    static MySqlConnection _connection = new MySqlConnection();
    static void Init()
    {
        var conbuilder = new MySqlConnectionStringBuilder()
        {
            Server = "localhost",
            Port = 3306,
            UserID = "root",
            Password = "licanxi",
            Database = "signdata",
        };
        _connection.ConnectionString = conbuilder.ConnectionString;
        _connection.Open();
    }

    public static object AddSignData(string username)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"insert into signlog (uid, time) values ('{username}', '{DateTime.Now}')";
        var result = command.ExecuteScalar();
        return result;
    }
    public static IEnumerable<DateTime> GetTodaySignData(string username)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"select time from signlog where uid='{username}' and DATE(time) = '{DateTime.Now:d}'";
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return (DateTime)reader["time"];
        }
        yield break;
    }
}
