using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RunopolyWebAPI.Models;

namespace RunopolyWebAPI.Controllers
{
    public class OwnersController : ApiController
    {
        private readonly RunopolyConnector connector;
        public OwnersController()
            : this(new RunopolyConnector())
        {
        }
        public OwnersController(RunopolyConnector _connector)
        {
            connector = _connector;
        }

        [HttpGet]
        public IQueryable<runopolyowners> Get(long id)
        {
            var owners = connector.OwnersGet(id);
            if (owners == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return owners;
        }

        [HttpOptions]
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
