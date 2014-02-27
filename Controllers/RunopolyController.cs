using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RunopolyWebAPI.Models;

namespace RunopolyWebAPI.Controllers
{
    public class RunopolyController : ApiController
    {
        private readonly RunopolyConnector connector;
        public RunopolyController()
            : this(new RunopolyConnector())
        {
        }
        public RunopolyController(RunopolyConnector _connector)
        {
            connector = _connector;
        }

        // User functions
        [HttpGet]
        public runopolyuser user(long id)
        {
            var user = connector.UserGet(id);
            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return user;
        }

        [HttpPost]
        public runopolyuser saveuser(runopolyuser user)
        {
            user.lastlogindate = DateTime.Now;
            long id = connector.UserAdd(user);
            var returnuser = connector.UserGet(id);
            if (returnuser == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return returnuser;
        }

        [HttpPost]
        public runopolyuser updateuser(long id, runopolyuser _user)
        {
            var user = connector.UserGet(id);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            connector.UserUpdate(_user);
            return user;
        }

        [HttpPost]
        public void deleteuser(long id)
        {
            var user = connector.UserGet(id);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            connector.UserDelete(user.id);
        }

        // Area functions
        [HttpGet]
        public IQueryable<runopolyarea> areas(long id)
        {
            var area = connector.AreasGet(id);
            if (area == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return area;
        }

        // Run functions
        [HttpGet]
        public IQueryable<runopolyrun> runs(long id)
        {
            var runs = connector.MyRunsGet(id);
            if (runs == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return runs;
        }
        
        [HttpPost]
        public HttpResponseMessage createrun(runopolyrun run)
        {
            run.creationdate = DateTime.Now;
            connector.RunAdd(run);

            var response = Request.CreateResponse(HttpStatusCode.Created, run);
            response.Headers.Location = new Uri(Request.RequestUri,
                Url.Route(null, new { id = run.id }));
            return response;
        }
        
        [HttpPost]
        public void deleterun(int id)
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
