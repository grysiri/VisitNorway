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
    [RoutePrefix("api/ProductsByMunicipality")]
    public class ProductsByMunicipalityController : ApiController
    {
        private Logic logic;

        //Running:
        public ProductsByMunicipalityController()
        {
            logic = new Logic();
        }

        //Testing:
        public ProductsByMunicipalityController(string test)
        {
            logic = new Logic(test);
        }

        [System.Web.Mvc.HttpGet]
        [Route("{language}/{municipality}")]
        public IQueryable<Models.Product> Get(string language, string municipality)
        {
            return logic.GetByPost(language, municipality);
        }

    }
}
