using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RunopolyWebAPI.Models;

namespace RunopolyWebAPI.Controllers
{
    public class AreasController : ApiController
    {
        private readonly RunopolyConnector connector;
        public AreasController()
            : this(new RunopolyConnector())
        {
        }
        public AreasController(RunopolyConnector _connector)
        {
            connector = _connector;
        }
        // GET api/values
        public IQueryable<runopolyarea> Get()
        {
            var areas = connector.AreasGet();

            if (areas == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return areas;
        }
        // GET api/values/5
        public runopolyarea Get(int id)
        {
            var area = connector.AreaGet(id);
            if (area == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return area;
        }


        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}