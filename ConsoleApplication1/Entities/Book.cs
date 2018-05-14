using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace ConsoleApplication1.Entities
{
    public class Book
    {
        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }

        [BsonElement("genre")]
        public string Genre { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }
    }
}
