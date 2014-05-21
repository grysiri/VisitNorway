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
    public class ProductsNearbyTest
    {
        private const string test = "test";

        [TestMethod]
        public void GetProductsNearby()
        {
            Mapper.AddProfile<DomainToApiModel>();
            var controller = new ProductsNearbyController(test);

            var list = new List<Product>();
            var pinfo = new Product() {Name = "Hotell", Id = 1};
            list.Add(pinfo);
            list.Add(pinfo);
            var expected = list.AsQueryable();

            var result = controller.Get("no", "71.111", "99.999");

            Assert.AreEqual(expected.First().Id, result.First().Id);
            Assert.AreEqual(expected.First().Name, result.First().Name);
            Assert.AreEqual(expected.Count(), result.Count());
        }

        [TestMethod]
        public void GetNullProductsNearby()
        {
            var controller = new ProductsNearbyController(test);
            var result = controller.Get("no", "15", "19.4");
            Assert.AreEqual(0, result.Count());
        }


    }
}
