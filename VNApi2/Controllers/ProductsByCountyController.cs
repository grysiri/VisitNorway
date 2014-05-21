using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using VNApi2.BLL;

namespace VNApi2.Controllers
{
    [RoutePrefix("api/ProductsByCounty")]
    public class ProductsByCountyController : ApiController
    {
        private Logic logic;

        //Running:
        public ProductsByCountyController()
        {
            logic = new Logic();
        }

        //Testing:
        public ProductsByCountyController(string test)
        {
            logic = new Logic(test);
        }

        [System.Web.Mvc.HttpGet]
        [Route("{language}/{county}")]
        public IQueryable<Models.Product> Get(string language, string county)
        {
            return logic.GetByCounty(language, county);
        }

    }
}
