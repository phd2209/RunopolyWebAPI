using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RunopolyWebAPI.Models;

namespace RunopolyWebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private readonly RunopolyConnector connector;
        public UsersController()
            : this(new RunopolyConnector())
        {
        }
        public UsersController(RunopolyConnector _connector)
        {
            connector = _connector;
        }
        // GET api/user/5
        public runopolyuser Get(long id)
        {
            var user = connector.UserGet(id);
            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return user;
        }
        // POST profile
        public HttpResponseMessage Post(runopolyuser user)
        {
            user.lastlogindate = DateTime.Now;
            connector.UserAdd(user);

            var response = Request.CreateResponse(HttpStatusCode.Created, user);
            response.Headers.Location = new Uri(Request.RequestUri,
                Url.Route(null, new { id = user.id }));
            return response;
        }
        // PUT profile/5
        public runopolyuser Put(runopolyuser _user)
        {
            var user = connector.UserGet(_user.id);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            connector.UserUpdate(_user);
            return user;
        }
        // DELETE /api/user/5
        public void Delete(long id)
        {
            var user = connector.UserGet(id);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            connector.UserDelete(user.id);
        }  
    }
}
