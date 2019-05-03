using Microsoft.Azure.Documents.Client;
using System;
using System.Reflection;

namespace Demo2
{
    internal class Program
    {
        private static readonly string cosmosURI = "https://localhost:8081";
        private static readonly string cosmosPrimaryKey = "<your key goes here>";
        private static readonly string cosmosDatabaseId = "MyDatabase";
        private static readonly string cosmosCollectionId = "MyCollection";

        private static void Main(string[] args)
        {
            using (DocumentClient cosmosClient = new DocumentClient(new Uri(cosmosURI), cosmosPrimaryKey))
            {
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(cosmosDatabaseId, cosmosCollectionId);
                string docPartition = Assembly.GetExecutingAssembly().GetName().Name;

                var timestamp = new DateTime(2019, 3, 27, 18, 15, 0, DateTimeKind.Unspecified);

                User[] users = new User[]
                {
                    new User() { id = "1", partition = docPartition, UserName = "User1", LastLogin = timestamp },
                    new User() { id = "2", partition = docPartition, UserName = "User2", LastLogin = timestamp.ToLocalTime() },
                    new User() { id = "3", partition = docPartition, UserName = "User3", LastLogin = timestamp.ToUniversalTime() }
                };

                foreach (User user in users)
                    cosmosClient.CreateDocumentAsync(collectionUri, user).Wait();
            };
        }

        private class User
        {
            public string id;
            public string partition;
            public string UserName;
            public DateTimeOffset LastLogin;
        }
    }
}