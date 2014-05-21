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
    public class ProductTest
    {
        [TestMethod]
        public void GetProductsByLang()
        {
            Mapper.AddProfile<DomainToApiModel>();
            var controller = new ProductsController("test");

            var list = new List<Product>();
            var pinfo = new Product() { Name = "Hotell", Id = 1 };
            list.Add(pinfo);
            list.Add(pinfo);
            var expected = list.AsQueryable();

            var result = controller.Get("no");

            Assert.AreEqual(expected.First().Id, result.First().Id);
            Assert.AreEqual(expected.First().Name, result.First().Name);
            Assert.AreEqual(expected.Count(), result.Count());
        }

        [TestMethod]
        public void GetProductsByLangNull()
        {
            Mapper.AddProfile<DomainToApiModel>();
            var controller = new ProductsController("test");
            var result = controller.Get("unknown_language");
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void GetProductByLangId()
        {
            Mapper.AddProfile<DomainToApiModel>();
            var controller = new ProductsController("test");
            var expected = new Product() { Name = "Hotell", Id = 1 };
            var result = controller.Get("no", "1");
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
        }

        [TestMethod]
        public void GetProductsByLangIdNull()
        {
            Mapper.AddProfile<DomainToApiModel>();
            var controller = new ProductsController("test");
            Product expected = null;
            var result = controller.Get("en", "1");

            Assert.AreEqual(expected, result);
        }
    }
}
