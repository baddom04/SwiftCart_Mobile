﻿namespace ShoppingList.Model
{
    public class Household
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Identifier { get; init; }
        public int UserId { get; init; }
    }
}
