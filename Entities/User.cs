using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Drink4Burpee.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        private int _level = 1;
        public int Level 
        { 
            get
            {
                return _level;
            } 
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(Level));
                }

                _level = value;
            } 
        }

        public List<Drink> Drinks { get; set; } = new List<Drink>();
    }
}