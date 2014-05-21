using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using VNApi2.BLL;
using VNApi2.Models;

namespace VNApi2.Controllers
{
    [RoutePrefix("api/LocalOrganization")]
    public class LocalOrganizationController : ApiController
    {
        private Logic logic;
        
        //Running
        public LocalOrganizationController()
        {
            logic = new Logic();
        }

        //Testing
        public LocalOrganizationController(string test)
        {
            logic = new Logic(test);
        }

        [Route("")]
        public IQueryable<LocalOrganization> Get()
        {
            return logic.GetAllLocalOrgsOrganizations();
        }
    }
}
