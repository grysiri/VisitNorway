using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VNApi2.BLL;
using VNApi2.Controllers;
using VNApi2.Models;

namespace VNApi2Test
{
    [TestClass]
    public class ProductByLocalOrganizationTest
    {
        [TestMethod]
        public void GetProductByLocalOrganization()
        {
            Mapper.AddProfile<DomainToApiModel>();
            var controller = new ProductsByLocalOrganizationController("test");

            var list = new List<Product>();
            var pinfo = new Product() { Name = "Hotell", Id = 1 };
            list.Add(pinfo);
            list.Add(pinfo);
            var expected = list.AsQueryable();

            var result = controller.Get("no", "1");

            Assert.AreEqual(expected.First().Id, result.First().Id);
            Assert.AreEqual(expected.First().Name, result.First().Name);
            Assert.AreEqual(expected.Count(), result.Count());
        }
    }
}
