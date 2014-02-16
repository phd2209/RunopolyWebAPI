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
        [HttpGet]
        public runopolyuser Get(long id)
        {
            var user = connector.UserGet(id);
            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return user;
        }
        // POST profile
        [HttpPost]
        public runopolyuser Post(runopolyuser user)
        {
            user.lastlogindate = DateTime.Now;
            long id = connector.UserAdd(user);
            var returnuser = connector.UserGet(id);
            if (returnuser == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return returnuser;
        }
        // PUT profile/5
        [HttpPut]
        public runopolyuser Put(runopolyuser _user)
        {
            var user = connector.UserGet(_user.id);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            connector.UserUpdate(_user);
            return user;
        }
        // DELETE /api/user/5
        [HttpDelete]
        public void Delete(long id)
        {
            var user = connector.UserGet(id);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            connector.UserDelete(user.id);
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
