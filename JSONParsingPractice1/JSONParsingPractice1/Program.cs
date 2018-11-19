using System;
using System.Data.SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace JSONParsingPractice1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://jsonplaceholder.typicode.com/");

            var request = new RestRequest("/users");

            IRestResponse response = client.Execute(request);

            var contentText = response.Content;

//            Console.WriteLine(contentText);
            
            JArray data = JArray.Parse(contentText);

//            Console.WriteLine(data["name"]);

            try
            {
                SQLiteConnection.CreateFile("test.db");
            }
            catch (Exception)
            {
                Console.WriteLine("db already exists");
            }
            
            SQLiteConnection connection = new SQLiteConnection("Data Source=test.db;Version=3");
            
            connection.Open();

            string sql = "CREATE TABLE IF NOT EXISTS userData (id INT, name VARCHAR(50), username VARCHAR(50), email VARCHAR(50), street_address VARCHAR(50), suite VARCHAR(50), city VARCHAR(50), zipcode VARCHAR(50))";
            
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();

            foreach (var item in data)
            {
                var id = int.Parse(item["id"].ToString());
                var name = item["name"].ToString();
                var username = item["username"].ToString();
                var email = item["email"].ToString();
                var street_address = item["address"]["street"].ToString();
                var suite = item["address"]["suite"].ToString();
                var city = item["address"]["city"].ToString();
                var zipcode = item["address"]["zipcode"].ToString();
                
                sql = $"INSERT INTO userData (id, name, username, email, street_address, suite, city, zipcode) VALUES ('{id}', '{name}', '{username}', '{email}', '{street_address}', '{suite}', '{city}', '{zipcode}')";
                
                command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
//            
            connection.Close();
        }
    }
}