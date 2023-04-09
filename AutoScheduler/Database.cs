using System;
using System.Data;
using System.Net.Security;
using Npgsql;

namespace Cockroach
{
  class MainClass
  {
    static void Main(string[] args)
    {
      var connStringBuilder = new NpgsqlConnectionStringBuilder();
      connStringBuilder.Host = "warmer-merman-6826.6wr.cockroachlabs.cloud";
      connStringBuilder.Port = 26257;
      connStringBuilder.SslMode = SslMode.VerifyFull;
      connStringBuilder.Username = "raven";
      connStringBuilder.Password = "JLAuy7G4qSFTI4Vh7VjLYg";
      connStringBuilder.Database = "backup";
      connStringBuilder.ApplicationName = "docs_simplecrud_csharp";
      Simple(connStringBuilder.ConnectionString);
    }

    static void Backup(string connString)
    {
      using (var conn = new NpgsqlConnection(connString))
      {
        conn.Open();

        // Create the "accounts" table.
        new NpgsqlCommand("CREATE TABLE IF NOT EXISTS tasks (id INT PRIMARY KEY, tasks STRING)", conn).ExecuteNonQuery();

        // Insert two rows into the "accounts" table.
        using (var cmd = new NpgsqlCommand())
        {
          cmd.Connection = conn;
          cmd.CommandText = "UPSERT INTO tasks(id, tasks) VALUES(@id1, @val1)";
          cmd.Parameters.AddWithValue("id1", 1);
          cmd.Parameters.AddWithValue("val1", "");
          cmd.ExecuteNonQuery();
        }
	  }
	}  
    static string getBackup(string connString)
    {
      using (var conn = new NpgsqlConnection(connString))
      {
        conn.Open();
	    // Print out the balances.
        System.Console.WriteLine("Initial balances:");
        using (var cmd = new NpgsqlCommand("SELECT tasks FROM tasks", conn))
        using (var reader = cmd.ExecuteReader())
          while (reader.Read())
            Console.Write("\taccount {0}: {1}\n", reader.GetValue(0), reader.GetValue(1));
      }
    }
  }
}
