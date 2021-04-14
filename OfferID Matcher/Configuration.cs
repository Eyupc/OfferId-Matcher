using System;
using PeanutButter.INI;

namespace OfferID_Matcher
{
    class Configuration
    {
        private static string host;
        private static string user;
        private static int port;
        private static string database;
        private static string password;
        private static string furnitureData;

        public static void ConnectionInfo()
        {
            INIFile file = new INIFile("config.ini");

            host = file.GetValue("connection", "host");
            user = file.GetValue("connection", "user");
            port = Int32.Parse(file.GetValue("connection", "port"));
            database = file.GetValue("connection", "database");
            password = file.GetValue("connection", "password");

            furnitureData = file.GetValue("furnidata", "furnidata-path");
        }

        public static string getHost()
        {
            return host;
        }
        public static string getUser()
        {
            return user;
        }
        public static int getPort()
        {
            return port;
        }
        public static string getDatabase()
        {
            return database;
        }
        public static string getPassword()
        {
            return password;
        }
        public static string furnidataPath()
        {
            return furnitureData;
        }
    }
}
