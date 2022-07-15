using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace KeyboardLearner
{
    static class Database
    {
        private static readonly string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\..\\..\\..\\database\\keyboard-db.db";
        private static readonly string connection_string = $"Data Source={path}";
        private static readonly SQLiteConnection sqlite = new SQLiteConnection(connection_string);

        /// <summary>
        /// Connects to the Keyboard Learner local database file.
        /// Must be called before any database interaction.
        /// </summary>
        public static void Connect()
        {
            try
            {
                sqlite.Open();
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine("Could not connect to the database. Is the path correct?");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Closes any open connection to the database.
        /// </summary>
        public static void Disconnect()
        {
            sqlite.Close();
        }

        /// <summary>
        /// Executes an SQL query to the database.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <param name="parameters">Parameters for the query. These are passed as tuples, with the param name and value</param>
        public static void ExecuteQuery(string query, params (string,dynamic)[] parameters)
        {
            Connect();
            using var cmd = new SQLiteCommand(query, sqlite);
            foreach((string name, dynamic val) in parameters)
            {
                cmd.Parameters.AddWithValue(name, val);
            }
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            Disconnect();
        }

        /// <summary>
        /// Used similarly to ExecuteQuery, but returns data (used with select statements).
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <param name="parameters">Parameters for the query. These are passed as tuples, with the param name and value</param>
        /// <returns>A list of records returned from the query.</returns>
        public static List<Dictionary<string,dynamic>> ExecuteSelectQuery(string query, (string, dynamic)[] parameters = null)
        {
            List<Dictionary<string, dynamic>> results = new List<Dictionary<string, dynamic>>();
            Connect();
            using var cmd = new SQLiteCommand(query, sqlite);
            if(parameters != null && parameters.Length > 0)
            {
                foreach ((string name, dynamic val) in parameters)
                {
                    cmd.Parameters.AddWithValue(name, val);
                }
            }
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                Dictionary<string, dynamic> r = new Dictionary<string, dynamic>();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    r.Add(rdr.GetName(i), rdr.IsDBNull(i) ? null : rdr.GetValue(i)); // Used to avoid errors with DB null values
                }
                results.Add(r);
            }
            Disconnect();
            return results;
        }

        /// <summary>
        /// Loads the contents of a file and readies for execution. Used for SQL queries saved in the /database/sql folder.
        /// </summary>
        /// <param name="filename">The name of the file to be loaded.</param>
        /// <returns>The contents of the file as a string.</returns>
        public static string LoadQuery(string filename)
        {
            FileInfo file = new FileInfo($"..\\..\\..\\database\\sql\\{filename}");
            string query = file.OpenText().ReadToEnd();
            return query;
        }
    }
}
