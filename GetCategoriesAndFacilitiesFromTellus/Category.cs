using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace GetCategoriesAndFacilitiesFromTellus
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Id + " " + Name;
        }
    }

    public class SubCategory : Category
    {
        public Category ParentCat { get; set; }

        public override string ToString()
        {
            return "   " + base.ToString();
        }
    }

    public class GetAllCategories
    {
        public int Counter = 0;
        public List<Category> AllCategories = new List<Category>(); 

        public void Get(string[] alle)
        {
            Console.WriteLine("Collecting all categories");
                foreach (var v in alle)
                {
                    Iterate(XDocument.Parse(v));
                }
                IComparer<Category> c = new ValueComparer();
                AllCategories.Sort(c);
 
                var file = new StreamWriter("CategoriesTellus.txt");

                foreach (var cat in AllCategories.Where(cat => !(cat is SubCategory)))
                {
                    Console.WriteLine(cat.ToString());
                    file.WriteLine(cat.ToString());
                    foreach (
                        var subcat in
                            AllCategories.Where(
                                subcat => subcat is SubCategory && ((SubCategory) subcat).ParentCat == cat))
                    {
                        Console.WriteLine(subcat.ToString());
                        file.WriteLine(subcat.ToString());
                        foreach (
                            var subcat2 in
                                AllCategories.Where(
                                    subcat2 => subcat2 is SubCategory && ((SubCategory) subcat2).ParentCat == subcat))
                        {
                            Console.WriteLine("  " + subcat2.ToString());
                            file.WriteLine("  " + subcat2.ToString());
                        }
                    }
                }
                file.Close();

                Console.WriteLine("Total categories and sub: " + Counter);
                Console.WriteLine("Categories in list: " + AllCategories.Count);                
            }

        public void Iterate(XDocument xDoc)
        {
            foreach (var product in xDoc.Descendants("product"))
            {
                foreach (var category in product.Descendants("categoryList").Descendants("category"))
                {                    
                    var cat = SaveCategory(category.Attribute("id").Value, category.Element("name").Value);
                    Counter++;

                    foreach (
                        var subcategory1 in category.Descendants("categorySubType1List").Descendants("categorySubType1"))
                    {                       
                        var subcat = SaveCategory(subcategory1.Attribute("id").Value, subcategory1.Element("name").Value, category.Attribute("id").Value);
                        subcat.ParentCat = cat;
                        Counter++;

                        foreach (
                            var subcategory2 in
                                subcategory1.Descendants("categorySubType2List").Descendants("categorySubType2"))
                        {                           
                           var subcat2 = SaveCategory(subcategory2.Attribute("id").Value, subcategory2.Element("name").Value, subcategory1.Attribute("id").Value);
                            subcat2.ParentCat = subcat;
                           Counter++;
                        }
                    }

                }
            }
        }

        public Category SaveCategory(string idstring, string name)
        {
            var id = int.Parse(idstring);
            var newcat = new Category() {Id = id, Name = name};
            if (!AllCategories.Contains(newcat, new IdAndNameComparer()))
            {
                AllCategories.Add(newcat);
                return newcat;
            }
            return AllCategories.Single(c=>c.Id == newcat.Id && c.Name == newcat.Name);
        }

        public SubCategory SaveCategory(string idstring, string name, string parentidstring)
        {
            var id = int.Parse(idstring);
            var parentid = int.Parse(parentidstring);
            var newcat = new SubCategory() {Id = id, Name = name};
            if (!AllCategories.Contains(newcat, new IdAndNameComparer()))
            {
                AllCategories.Add(newcat);
                return newcat;
            }
            return (SubCategory) AllCategories.Single(c => c.Id == newcat.Id && c.Name == newcat.Name);
        }
    }

    public class IdComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category x, Category y)
        {
            return x.Id == y.Id;            
        }

        public int GetHashCode(Category obj)
        {
            throw new NotImplementedException();
        }
    }

    public class IdAndNameComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category x, Category y)
        {            
            return (x.Id == y.Id && x.Name.Equals(y.Name));
        }

        public int GetHashCode(Category obj)
        {
            throw new NotImplementedException();
        }
    }

    public class ValueComparer : IComparer<Category>
    {
        public int Compare(object x, object y)
        {
            var val = 0;
            if (x is Category && y is Category)
            {
                var catx = (Category) x;
                var caty = (Category) y;
                if (catx.Id < caty.Id) val = 1;
                if (catx.Id > caty.Id) val = -1;
                return val;
            }
            throw new InvalidCastException("Wrong class in comparator");
        }

        public int Compare(Category x, Category y)
        {
            var val = 0;
              if (x.Id < y.Id) val = -1;
              if (x.Id > y.Id) val = 1;  
            
            return val;
        }
    }
}

