using System;

namespace VNApi2.Models
{
    public class Category
    {
        public String CategoryName { get; set; }
        public Category ParentCategory { get; set; }
    }
}