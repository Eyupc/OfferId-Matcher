using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OfferID_Matcher
{
    class DatabaseConnection
    {
        private String server;
        private String user;
        private String database;
        private int port;
        private String password;

        private MySqlConnection connection;

        public DatabaseConnection(string server, string user, int port, string database, string password)
        {
            this.server = server;
            this.user = user;
            this.port = port;
            this.database = database;
            this.password = password;
        }

        public void DBConnect()
        {
            String sql = "server =" + this.server + ";user =" + this.user + "; database = " + this.database + "; port =+" + this.port + "; password =" + this.password + ";";
            this.connection = new MySqlConnection(sql);
            try
            {
                connection.Open();
                Console.WriteLine("Successfully connected with your database!");
            }catch(MySqlException e)
            {
                Console.WriteLine("Problem with your configuration! Something went wrong. Take a look at your config.ini.");
                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.Out.WriteLine(e);

                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.Write("Press a key to exit!");
                Console.ReadLine();
                System.Environment.Exit(0);

            }

        }

        public MySqlConnection getConnection()
        {
            return this.connection;
        }
       
        public void CloseConnection()
        {

            
            connection.Close();
            //Console.WriteLine("Connection closed!");
        }


    }
}
