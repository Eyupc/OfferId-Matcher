using System;
using PeanutButter.INI;
using NugetJObject;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using System.Threading;

namespace OfferID_Matcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OfferID Matcher");
            Console.WriteLine("------------------");


            Configuration.ConnectionInfo();
            DatabaseConnection connection = new DatabaseConnection(Configuration.getHost(), Configuration.getUser(), Configuration.getPort(),Configuration.getDatabase(), Configuration.getPassword());
            connection.DBConnect();

            if (!File.Exists(Configuration.furnidataPath())){
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("Your furnidata path is wrong!");
                Console.ReadLine();
                return;
            }else if (Path.GetExtension(Configuration.furnidataPath()) != ".json")
            {
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("Sorry, we only support JSON format!");
                Console.ReadLine();
                return;
            }

            JObject obj = JObject.Parse(File.ReadAllText(Configuration.furnidataPath()));
            JObject roomitemtypes = (JObject)obj.GetValue("roomitemtypes"); // Roomitemtypes
            JObject wallitemtypes = (JObject)obj.GetValue("wallitemtypes"); //Wallitemtypes

            JArray flooritems = (JArray)roomitemtypes.GetValue("furnitype");
            JArray wallitems = (JArray)wallitemtypes.GetValue("furnitype");

            int problems = 0;
            Console.WriteLine("The program will start in 3 seconds..");
            Thread.Sleep(3000);
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection.getConnection();
            command.CommandText = "UPDATE catalog_items SET offer_id=@offerid WHERE catalog_name =@catalog_name";
            for (int i= 0; i <= flooritems.Count - 1; i++)
            {
                try
                {
                    int offerId = (int)flooritems[i].SelectToken("offerid");
                    string classname = (string)flooritems[i].SelectToken("classname");

                    if(offerId == null || classname == null)
                    {
                        Console.WriteLine("Problem with your furnidata!");
                        problems++;
                        continue;
                    }
                    command.Parameters.AddWithValue("@offerid", offerId);
                    command.Parameters.AddWithValue("@catalog_name", classname);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    Console.WriteLine("[FLOORITEM]  OfferId of  |" + classname + "|  is changed!");

                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e);
                    problems++;
                    continue;
                }

            }

            for (int i = 0; i <= wallitems.Count - 1; i++)
            {
                try
                {
                    int offerId = (int)wallitems[i].SelectToken("offerid");
                    string classname = (string)wallitems[i].SelectToken("classname");

                    if (offerId == null || classname == null)
                    {
                        Console.WriteLine("Problem with your furnidata!");
                        problems++;
                        continue;
                    }

                    command.Parameters.AddWithValue("@offerid", offerId);
                    command.Parameters.AddWithValue("@catalog_name", classname);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    Console.WriteLine("[WALLITEM]  OfferId of  |" + classname + "|  is changed!");

                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e);
                    problems++;
                    continue;
                }

            }
            connection.CloseConnection();
            Console.WriteLine("----------------------------------------------------------------------");
            Console.Write("Successfully changed the offer ids! (Caught: " + problems+" problem(s)!)");
 
            Console.ReadLine();
            


        }


    }
}
