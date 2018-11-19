using System;
using System.Data.SQLite;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace JSONParsingPractice2
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://jsonplaceholder.typicode.com/");
            
            var request = new RestRequest("/todos");

            IRestResponse response = client.Execute(request);

            var content = response.Content;
            
            JArray dataContent = JArray.Parse(content);

            SQLiteConnection.CreateFile("todos.db");
            
            SQLiteConnection conn = new SQLiteConnection("Data Source=todos.db;Version=3;");
            
            conn.Open();

            var sql = "CREATE TABLE IF NOT EXISTS todo_data (userId INT, id INT, title VARCHAR(255), completed VARCHAR(30))";
            
            SQLiteCommand command = new SQLiteCommand(sql, conn);

            command.ExecuteNonQuery();

            foreach (var VARIABLE in dataContent)
            {
                var userId = int.Parse(VARIABLE["userId"].ToString());
                var id = int.Parse(VARIABLE["id"].ToString());
                var title = VARIABLE["title"].ToString();
                var completed = VARIABLE["completed"].ToString();

                sql = $"INSERT INTO todo_data (userId, id, title, completed) VALUES ('{userId}', '{id}', '{title}', '{completed}')";
                
                command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
            }
           
            conn.Close();
        }
    }
}