﻿using BookLibrary.Data.Entities;

namespace BookLibrary.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public string? Genre { get; set; }

        public int? Year { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public string Publisher { get; set; } = "";
        public string Location { get; set; } = "";

        public List<string> Authors { get; set; } = new List<string>();
    }
}
