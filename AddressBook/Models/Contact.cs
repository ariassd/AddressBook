using System;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq;

namespace AddressBook.Models
{

    public class Contact 
    {
        private static string COLLECTION = "contact";
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<string> Phone { get; set; }
        public List<string> Email { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }

        public static List<Contact> GetList()
        {
            List<Contact> result = new List<Contact>();
            FilterDefinition<Contact> filter = FilterDefinition<Contact>.Empty;
            MongoHelper.GetCollection<Contact>(COLLECTION).FindSync(filter).ForEachAsync(doc => result.Add(doc));
            return result;
        }

        public static Contact FindById(string id)
        {
            Contact result = new Contact();
            var filter = Builders<Contact>.Filter.Eq(s => s.Id, new ObjectId(id));
            result = MongoHelper.GetCollection<Contact>(COLLECTION).
                       FindSync(filter).First();
            return result;
        }

        public static List<Contact> FindByName(string name)
        {
            List<Contact> result = new List<Contact>();
            var filter = Builders<Contact>.Filter.Regex(i => i.Name, new BsonRegularExpression($"/{name}/gi"));
            result = MongoHelper.GetCollection<Contact>(COLLECTION).
                                FindSync(filter).ToList();
            return result;
        }

        public static bool Delete(string id, out string message) 
        {
            bool result = false;
            message = "";
            try
            {
                DeleteResult deleteResult = MongoHelper.GetCollection<Contact>(COLLECTION).DeleteOne(Builders<Contact>.Filter.Eq(r => r.Id, new ObjectId(id)));

                if (deleteResult.DeletedCount == 1) {
                    result = true;
                }
                else
                {
                    result = false;
                    message = "The id wasn't found";
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = $"An exception ocurred while deleting contact. {ex.Message}";
            }
            return result;
        }

        public static bool AddNew(Contact newPerson, out string message) 
        {
            bool result = false;
            message = "";
            try 
            {
                cleanData(newPerson);
                MongoHelper.GetCollection<Contact>(COLLECTION).InsertOne(newPerson);
                result = true;
            }
            catch(Exception ex) 
            {
                result = false;
                message = $"An exception ocurred while creating contact. {ex.Message}" ;
            }
            return result;
        }

        public static bool Add(List<Contact> contacts, out string message)
        {
            bool result = false;
            message = "";
            try
            {
                foreach (var con in contacts)
                {
                    cleanData(con);
                }

                MongoHelper.GetCollection<Contact>(COLLECTION).InsertMany(contacts);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                message = $"An exception ocurred while creating contact. {ex.Message}";
            }
            return result;
        }

        public static bool Edit(Contact person, out string message)
        {
            bool result = false;
            message = "";
            try
            {
                person = cleanData(person);
                var filter = Builders<Contact>.Filter.Eq(s => s.Id, person.Id);
                var rmongo = MongoHelper.GetCollection<Contact>(COLLECTION).ReplaceOne(filter, person);

                if (rmongo.ModifiedCount >= 1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    message = "The id wasn't found";
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                message = $"An exception ocurred while updating contact. {ex.Message}";
            }
            return result;
        }

        private static Contact cleanData(Contact contact)
        {
            contact.Email = contact.Email
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .Distinct().ToList();
            contact.Phone = contact.Phone
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .Distinct().ToList();
            return contact;
        }
    }
}
