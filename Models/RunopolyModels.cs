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
      public string nick_name { get; set; }
      public string first_name { get; set; }
      public string last_name { get; set; }
      public string gender { get; set; }
      public string email { get; set; }
      public DateTime lastlogindate { get; set; }
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
        public runopolyuser owner { get; set; }
        public int level { get; set; }
        public bool isUserArea { get; set; }
        public double MaxKm { get; set; }
        public double TotalKm { get; set; }
        public double UserKm { get; set; }
        public int NoRuns { get; set; }
    }

    public class runopolyarearaw
    {
        public int id { get; set; }
        public string name { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public int radius1 { get; set; }
        public int radius2 { get; set; }
        public int rotation { get; set; }
        public int userid { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string nick_name { get; set; }
        public string gender { get; set; }
        public double areakm { get; set; }
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

    public class runopolyowner
    {
        public runopolyuser owner { get; set; }
        public bool isUser { get; set; }
        public double UserKm { get; set; }
        public int NoRuns { get; set; }
    }

    public class runopolyowners
    {
        public int id { get; set; }
        public string name { get; set; }
        public double TotalKm { get; set; }
        public List<runopolyowner> owners { get; set; } 
    }
}