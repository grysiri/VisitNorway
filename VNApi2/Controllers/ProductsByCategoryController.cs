using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using VNApi2.BLL;
using VNApi2.Models;

namespace VNApi2.Controllers
{
    [RoutePrefix("api/ProductsByCategory")]
    public class ProductsByCategoryController : ApiController
    {
        private Logic logic;

        //Running
        public ProductsByCategoryController()
        {
            logic = new Logic();
        }

        //Testing
        public ProductsByCategoryController(string test)
        {
            logic = new Logic(test);
        }

        [Route("{language}/{category}")]
        public IQueryable<Product> Get(string language, string category)
        {
            return logic.GetByCategory(language, category);
        }

    }
}
