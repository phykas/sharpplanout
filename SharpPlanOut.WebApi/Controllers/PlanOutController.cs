using Newtonsoft.Json.Linq;
using SharpPlanOut.Services.Interfaces;
using System.Collections.Generic;
using System.Web.Http;

namespace SharpPlanOut.WebApi.Controllers
{
    [RoutePrefix("planout")]
    public class PlanOutController : ApiController
    {
        private readonly INamespaceManager _namespaceManager;

        public PlanOutController(INamespaceManager namespaceManager)
        {
            _namespaceManager = namespaceManager;
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Get(JObject inputs)
        {
            return Ok(_namespaceManager.GetParams(inputs.ToObject<Dictionary<string, object>>()));
        }
    }
}