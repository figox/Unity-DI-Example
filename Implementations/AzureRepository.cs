using System;
using System.Collections.Generic;
using Domain;
using Interfaces;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace Implementations
{
    public class AzureRepository : IRepository<Customer>
    {
        private DocumentClient _client;
        private static string _endpoint = "{ENDPOINT_HERE}";
        private static string _key = "{MASTER_KEY_HERE}";
        private static string _databaseId = "{DATABASE_NAME_HERE}";
        private static string _collectionId = "{COLLECTION_NAME_HERE}";

        public async Task<bool> Add(Customer obj)
        {
            bool added = false;

            using (_client = new DocumentClient(new Uri(_endpoint), _key))
            {
                //Get the database - LinQ query syntax
                Database db = (from d in _client.CreateDatabaseQuery()
                               where d.Id == _databaseId
                               select d).ToArray().FirstOrDefault();

                //Get the collection - LinQ method syntax
                DocumentCollection collection =
                    _client.CreateDocumentCollectionQuery(UriFactory.CreateDatabaseUri(db.Id))
                            .Where(c => c.Id == _collectionId)
                            .ToArray()
                            .SingleOrDefault();

                var response = await _client.CreateDocumentAsync(collection.SelfLink, obj);
                added = response.StatusCode == System.Net.HttpStatusCode.Created ? true : false;
            }

            return added;
        }

        public async Task<bool> Delete(string id)
        {
            bool deleted = false;

            using (_client = new DocumentClient(new Uri(_endpoint), _key))
            {
                //Get the database - LinQ query syntax
                Database db = (from d in _client.CreateDatabaseQuery()
                               where d.Id == _databaseId
                               select d).ToArray().FirstOrDefault();

                //Get the collection - LinQ method syntax
                DocumentCollection collection =
                    _client.CreateDocumentCollectionQuery(UriFactory.CreateDatabaseUri(db.Id))
                            .Where(c => c.Id == _collectionId)
                            .ToArray()
                            .SingleOrDefault();

                //Get the document from collection
                Document doc = _client.CreateDocumentQuery(collection.SelfLink,
                    "SELECT * FROM CastMembers c WHERE c.id = '" + id + "'")
                    .ToList().SingleOrDefault();

                if (doc != null)
                {
                    var response = await _client.DeleteDocumentAsync(doc.SelfLink);
                    deleted = response.StatusCode == System.Net.HttpStatusCode.NoContent ? true : false;
                }
            }

            return deleted;
        }

        public List<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();

            using (_client = new DocumentClient(new Uri(_endpoint), _key))
            {
                //Get the database - LinQ query syntax
                Database db = (from d in _client.CreateDatabaseQuery()
                               where d.Id == _databaseId
                               select d).ToArray().FirstOrDefault();

                //Get the collection - LinQ method syntax
                DocumentCollection collection =
                    _client.CreateDocumentCollectionQuery(UriFactory.CreateDatabaseUri(db.Id))
                            .Where(c => c.Id == _collectionId)
                            .ToArray()
                            .SingleOrDefault();

                //Get list of documents - SQL syntax
                customers = _client.CreateDocumentQuery<Customer>(collection.SelfLink,
                    "SELECT * FROM Customers")
                    .ToList();
            }

            return customers;
        }

        public Customer GetById(string id)
        {
            Customer customer = new Customer();

            using (_client = new DocumentClient(new Uri(_endpoint), _key))
            {
                //Get the database - LinQ query syntax
                Database db = (from d in _client.CreateDatabaseQuery()
                               where d.Id == _databaseId
                               select d).ToArray().FirstOrDefault();

                //Get the collection - LinQ method syntax
                DocumentCollection collection =
                    _client.CreateDocumentCollectionQuery(UriFactory.CreateDatabaseUri(db.Id))
                            .Where(c => c.Id == _collectionId)
                            .ToArray()
                            .SingleOrDefault();

                //Get list of documents - SQL syntax
                customer = _client.CreateDocumentQuery<Customer>(collection.SelfLink,
                    "SELECT * FROM Customers WHERE id = '" + id + "'")
                    .FirstOrDefault();
            }

            return customer;
        }
    }
}
