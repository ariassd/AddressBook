using System;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace AddressBook.Models
{
    public class MongoHelper
    {
        public static IMongoCollection<T> GetCollection<T>(string collectionName) 
        {
            string conectionString = ConfigurationManager.AppSettings["connectionString"];
            MongoClient client = new MongoClient(conectionString);
            IMongoDatabase database = client.GetDatabase("mvcaddbook", null);
            IMongoCollection<T> collection = database.GetCollection<T>(collectionName);
            return collection;
        }
    }
}
