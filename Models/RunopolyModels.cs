using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel; 
using System.Web.Mvc;
using System.Globalization;

namespace RunopolyWebAPI.Models
{
    public class runopolyuser
    {
      public long id { get; set; }
      public string first_name { get; set; }
      public string last_name { get; set; }
      public string gender { get; set; }
      public string email { get; set; }
    }

    public class runopolyarea
    {
        public int id { get; set; }
        public string name { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public int radius1 { get; set; }
        public int radius2 { get; set; }
        public int rotation { get; set; }
        public string owner { get; set; }
        public int level { get; set; }
    }

    public class runopolyrun
    {
        public int id { get; set; }
        public long userid { get; set; }
        public int areaid { get; set; }
        public double totalkm { get; set; }
        public double areakm { get; set; }
        public TimeSpan time { get; set; }
        public DateTime creationdate { get; set; }
    }
}