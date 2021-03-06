﻿using System;
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

        [HttpGet]
        public IQueryable<runopolyarea> Get(long id)
        {
            var area = connector.AreasGet(id);
            if (area == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return area;
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