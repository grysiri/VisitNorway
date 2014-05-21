using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using VNApi2.BLL;

namespace VNApi2.Controllers
{
    [RoutePrefix("api/ProductsNearby")]
    public class ProductsNearbyController : ApiController
    {
        private Logic logic;
        
        //Running:
        public ProductsNearbyController()
        {
            logic = new Logic();
        }

        //Testing:
        public ProductsNearbyController(string test)
        {
            logic = new Logic(test);
        }


        [HttpGet]
        [Route("{language}/{latitude}/{longitude}")]
        public IQueryable<Models.Product> Get(string language, string latitude, string longitude)
        {
            return logic.GetNearby(language, latitude, longitude);
        }
    }
}
