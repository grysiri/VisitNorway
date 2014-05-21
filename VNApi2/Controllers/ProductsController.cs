using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using VNApi2.BLL;
using VNApi2.Models;

namespace VNApi2.Controllers
{
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        private Logic logic;
        
        //Running:
        public ProductsController()
        {
            logic = new Logic();
        }

        //Testing:
        public ProductsController(string test)
        {
            logic = new Logic(test);
        }

        [HttpGet]
        [Route("{language}")]
        public IQueryable<Product> Get(string language)
        {
            return logic.GetAll(language);
        }

        [HttpGet]
        [Route("{language}/{id}")]
        public Product Get(string language, string id)
        {
            return logic.GetSingle(language, id);
        }

    }
}