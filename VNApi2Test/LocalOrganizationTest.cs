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
    public class LocalOrganizationTest
    {
        [TestMethod]
        public void GetLocalOrgs()
        {
            Mapper.AddProfile<DomainToApiModel>();
            var controller = new LocalOrganizationController("test");
            var list = new List<LocalOrganization>();
            var lorg = new LocalOrganization() { Name = "VisitOslo", Id = 2 };
            list.Add(lorg);
            list.Add(lorg);
            var expected = list.AsQueryable();

            var result = controller.Get();

            Assert.AreEqual(expected.First().Id, result.First().Id);
            Assert.AreEqual(expected.First().Name, result.First().Name);
            Assert.AreEqual(expected.Count(), result.Count());

        }

    }
}
