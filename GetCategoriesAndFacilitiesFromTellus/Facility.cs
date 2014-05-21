using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace GetCategoriesAndFacilitiesFromTellus
{
    public class Facility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Comments = new List<string>(); 

        public new string ToString()
        {
            string s = Id + " " + Name;           
            return s;
        }
    }

    public class FacilityCategory : Facility
    {
        public List<Facility> List = new List<Facility>();

        public new string ToString()
        {
            string s = base.ToString() + "\n";
            foreach (var fac in List)
            {
                s +=" "+ fac.ToString()+ "\n";
            }
            return s;
        }
    }

    public class GetAllFacilities
    {
        public List<Facility> Facilities = new List<Facility>();

        public void Get(string[] alle)
        {
            Console.WriteLine("Henter alle fasiliteter");
            foreach (var v in alle)
                Iterate(XDocument.Parse(v));

            var file = new StreamWriter("FacilitiesTellus.txt"); //Writes to this file
            
            //Iterates through all facilitycategories and their facilities, writes and saves to file
            foreach (var facility in Facilities.Where(fac => fac is FacilityCategory))
            {
                var fac = (FacilityCategory) facility;
                Console.WriteLine(fac.ToString());
                file.WriteLine(fac.ToString());
            }
            file.Close();            
        }
        
        //Iterates through all products, gets their facilities and saves the ones who isn't in the list already
        public void Iterate(XDocument xDoc)
        {
            foreach (var product in xDoc.Descendants("product"))
            {
                foreach (
                    var facilitycategory in product.Descendants("facilityCategoryList").Descendants("facilityCategory"))
                {
                    var facat = new FacilityCategory()
                    {
                        Id = int.Parse(facilitycategory.Attribute("id").Value),
                        Name = facilitycategory.Element("name").Value
                    };
                    if (!Facilities.Contains(facat, new Comparer()))
                        Facilities.Add(facat);
                    else
                        facat = (FacilityCategory)Facilities.Single(f => f.Id == facat.Id && f.Name == facat.Name);

                    foreach (var facility in facilitycategory.Descendants("facilityList").Descendants("facility"))
                    {
                        var fac = new Facility()
                        {
                            Id = int.Parse(facility.Attribute("id").Value),
                            Name = facility.Element("name").Value
                        };
                        var hasnewcomment = false;
                        string c = "";
                        if (facility.Element("comment") != null && !facility.Element("comment").Value.Equals(""))
                        {
                            c = facility.Element("comment").Value;
                            hasnewcomment = true;
                            fac.Comments.Add(c);
                        }
                        if (!facat.List.Contains(fac, new Comparer()))
                            facat.List.Add(fac);
                        else if (hasnewcomment && !c.Equals(""))
                        {
                            var fa = Facilities.Single(f => f.Id == facat.Id && f.Name == facat.Name);
                            if (!fa.Comments.Contains(c)) fa.Comments.Add(c);
                        }
                    }
                }
            }
        }
    }

    //komparator for fasiliteter, sammenligner id og navn
    public class Comparer : IEqualityComparer<Facility>
    {
        public bool Equals(Facility x, Facility y)
        {
            return (x.Id == y.Id && x.Name.Equals(y.Name));
        }

        public int GetHashCode(Facility obj)
        {
            throw new NotImplementedException();
        }
    }

}
