﻿namespace EFCoreInMemoryDemo.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public float Price { get; set; }
    }
}
