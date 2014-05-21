using System;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using VNApi2.BLL;

namespace VNApi2.Controllers
{
    [RoutePrefix("api/ProductsByLocalOrganization")]
    public class ProductsByLocalOrganizationController : ApiController
    {
        private Logic logic;
        
        //Running
        public ProductsByLocalOrganizationController()
        {
            logic = new Logic();
        }

        //Testing
        public ProductsByLocalOrganizationController(string test)
        {
            logic = new Logic(test);
        }

        [HttpGet]
        [Route("{language}/{id}")]
        public IQueryable<Models.Product> Get(String language, string id)
        {
            return logic.GetByLocalOrgId(int.Parse(id), language);
        }
    }
}
