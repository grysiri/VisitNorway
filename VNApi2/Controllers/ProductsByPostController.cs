using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using VNApi2.BLL;

namespace VNApi2.Controllers
{
    [RoutePrefix("api/ProductsByPost")]
    public class ProductsByPostController : ApiController
    {
        private Logic logic;
        
        //Running:
        public ProductsByPostController()
        {
            logic = new Logic();
        }

        //Testing:
        public ProductsByPostController(string test)
        {
            logic = new Logic(test);
        }

        [HttpGet]
        [Route("{language}/{postarea}")]
        public IQueryable<Models.Product> Get(string language, string postarea)
        {
            return logic.GetByPost(language, postarea);
        }

    }
}
