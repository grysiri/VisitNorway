﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VNApi2.BLL;
using VNApi2.Controllers;
using VNApi2.Models;

namespace VNApi2Test
{
    /// <summary>
    /// Summary description for ProductByMunicipalityTest
    /// </summary>
    [TestClass]
    public class ProductByMunicipalityTest
    {
        [TestMethod]
        public void GetProductByMunicipalityTest()
        {
            Mapper.AddProfile<DomainToApiModel>();
            var controller = new ProductsByMunicipalityController("test");
            var list = new List<Product>();
            var pinfo = new Product() { Name = "Hotell", Id = 1 };
            list.Add(pinfo);
            list.Add(pinfo);
            var expected = list.AsQueryable();

            var result = controller.Get("no", "Oslo");

            Assert.AreEqual(expected.First().Id, result.First().Id);
            Assert.AreEqual(expected.First().Name, result.First().Name);
            Assert.AreEqual(expected.Count(), result.Count());
        }

        [TestMethod]
        public void GetNullProductByMunicipalityTest()
        {
            Mapper.AddProfile<DomainToApiModel>();
            var controller = new ProductsByMunicipalityController("test");

            var result = controller.Get("no", "null");

            Assert.IsNull(result);
        }

    }
}