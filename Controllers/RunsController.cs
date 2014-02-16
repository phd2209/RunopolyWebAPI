using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RunopolyWebAPI.Models;

namespace RunopolyWebAPI.Controllers
{
    public class RunsController : ApiController
    {
        private readonly RunopolyConnector connector;
        public RunsController()
            : this(new RunopolyConnector())
        {
        }
        public RunsController(RunopolyConnector _connector)
        {
            connector = _connector;
        }
        
        // GET api/runs
        //public IEnumerable<string> Get()
        //{
            //return new string[] { "value1", "value2" };
        //}

        // GET api/runs/5
        public IQueryable<runopolyrun> Get(long id)
        {
            var runs = connector.MyRunsGet(id);
            if (runs == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return runs;
        }
        // POST api/runs
        public HttpResponseMessage Post(runopolyrun run)
        {
            run.creationdate = DateTime.Now;
            connector.RunAdd(run);

            var response = Request.CreateResponse(HttpStatusCode.Created, run);
            response.Headers.Location = new Uri(Request.RequestUri,
                Url.Route(null, new { id = run.id }));
            return response;
        }

        // PUT api/runs/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/runs/5
        public void Delete(int id)
        {
            var run = connector.MyRunGet(id);

            if (run == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            connector.RunDelete(run.id);
        }

        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
