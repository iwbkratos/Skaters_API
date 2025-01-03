﻿using System.Data.SqlTypes;

namespace Skaters.Domain.Model
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; }
        public float Price { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public Guid StoreId { get; set; }
        public required string UserId { get; set; }

    }
}
