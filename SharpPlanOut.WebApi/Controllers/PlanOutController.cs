using Newtonsoft.Json.Linq;
using SharpPlanOut.Services.Interfaces;
using System.Collections.Generic;
using System.Web.Http;

namespace SharpPlanOut.WebApi.Controllers
{
    [RoutePrefix("planout")]
    public class PlanOutController : ApiController
    {
        private readonly INamespaceManagerService _namespaceManager;

        public PlanOutController(INamespaceManagerService namespaceManager)
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