using System;
using System.Web;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data;

namespace RunopolyWebAPI.Models
{
  public class RunopolyConnector
  {
        private OdbcConnection conn = new OdbcConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RunopolyConnectionString"].ConnectionString);
        
        // User functions
        public bool UserExists(long id)
        {
            try
            {
                conn.Open();
                string strSQL = "SELECT * from runopoly_users WHERE Id = " + id.ToString();
                OdbcCommand myCommand = new OdbcCommand(strSQL, conn);
                bool retVal = (1 == myCommand.ExecuteNonQuery());
                return retVal;
            }
            catch
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public long UserAdd(runopolyuser user)
        {
            long id = 0;
            try
            {
                conn.Open();
                string strSQL = "INSERT INTO runopoly_users (nick_name, first_name, last_name, gender, email,lastlogindate) VALUES (" +
                                  "'" + user.nick_name + "', " +
                                  "'" + user.first_name + "', " +
                                  "'" + user.last_name + "', " +
                                  "'" + user.gender + "', " +
                                  "'" + user.email + "', " +
                                  "'" + toMySQLDateTime(user.lastlogindate) + "')";

                OdbcCommand myCommand = new OdbcCommand(strSQL, conn);
                myCommand.ExecuteNonQuery();
                myCommand.CommandText = "SELECT LAST_INSERT_ID()";
                IDataReader reader = myCommand.ExecuteReader();

                if (reader != null && reader.Read()) id = reader.GetInt64(0);
                return id;
            }
            catch
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }      
        public bool UserUpdate(runopolyuser user)
        {
            try
            {
                conn.Open();
                string strSQL = "UPDATE runopoly_users SET " +
                                    "nick_name = " + "'" + user.nick_name + "', " +
                                    "first_name = " + "'" + user.first_name + "', " +
                                    "last_name = " + "'" + user.last_name + "', " +
                                    "gender = " + "'" + user.gender + "', " +
                                    "email = " + "'" + user.email + "', " +
                                    "LastLoginDate = '" + toMySQLDateTime(DateTime.Now) + "' " +
                                    "WHERE id = '" + user.id.ToString() + "'";

                OdbcCommand myCommand = new OdbcCommand(strSQL, conn);
                bool retVal = (1 == myCommand.ExecuteNonQuery());
                return retVal;
            }
            catch
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public bool UserDelete(long id)
        {
            try
            {
                conn.Open();
                string strSql = "DELETE FROM runopoly_users " +
                                "WHERE id = " + id.ToString();
                OdbcCommand myCommand = new OdbcCommand(strSql, conn);
                bool UserDeleted = (1 <= myCommand.ExecuteNonQuery());
                conn.Close();
                if (UserDeleted)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public runopolyuser UserGet(long id)
        {
            runopolyuser returnUser = null;
            try
            {
                string strSQL0 = "SELECT * from runopoly_users WHERE id = " + id.ToString();
                OdbcCommand myCommand0 = new OdbcCommand(strSQL0, conn);
                conn.Open();
                OdbcDataReader myReader0 = myCommand0.ExecuteReader();
                while (myReader0.Read())
                {
                    returnUser = new runopolyuser();
                    returnUser.id = Int32.Parse(myReader0.GetValue(myReader0.GetOrdinal("id")).ToString());
                    returnUser.nick_name = myReader0.GetValue(myReader0.GetOrdinal("nick_name")).ToString();
                    returnUser.first_name = myReader0.GetValue(myReader0.GetOrdinal("first_name")).ToString();
                    returnUser.last_name = myReader0.GetValue(myReader0.GetOrdinal("last_name")).ToString();
                    returnUser.gender = myReader0.GetValue(myReader0.GetOrdinal("gender")).ToString();
                    returnUser.email = myReader0.GetValue(myReader0.GetOrdinal("email")).ToString();
                    returnUser.lastlogindate = myReader0.GetDateTime(myReader0.GetOrdinal("lastlogindate"));
                }
                myReader0.Close();
                conn.Close();
                return returnUser;
            }
            catch
            {
                return null;
            }
        }
        
      // Area functions
        public IQueryable<runopolyarea> AreasGet(long id)
        {
            conn.Open();
            try
            {
                string strSQL1 = "SELECT " +
                                    " a.id, " +
                                    " a.name," +
                                    " a.longitude, " +
                                    " a.latitude, " +
                                    " a.radius1, " +
                                    " a.radius2, " +
                                    " a.rotation, " +
                                    " b.nick_name, " +
                                    " b.first_name, " +
                                    " b.last_name, " +
                                    " b.gender, " +
                                    " c.areakm, " +
                                    " c.userid " +
                                 "from runopoly_areas a " +
                                 "left join runopoly_runs c " +
                                 "on a.id = c.areaid " +
                                 "left join runopoly_users b " +
                                 "on c.userid = b.id";

                OdbcCommand myCommand1 = new OdbcCommand(strSQL1, conn);
                OdbcDataReader myReader1 = myCommand1.ExecuteReader();
                List<runopolyarearaw> tempresult = new List<runopolyarearaw>();
                    
                while (myReader1.Read())
                {
                    runopolyarearaw area = new runopolyarearaw();
                    area.id = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("id")).ToString());
                    area.name = myReader1.GetValue(myReader1.GetOrdinal("name")).ToString();
                    area.longitude = Double.Parse(myReader1.GetValue(myReader1.GetOrdinal("longitude")).ToString());
                    area.latitude = Double.Parse(myReader1.GetValue(myReader1.GetOrdinal("latitude")).ToString());
                    area.radius1 = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("radius1")).ToString());
                    area.radius2 = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("radius2")).ToString());
                    area.rotation = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("rotation")).ToString());
                    area.nick_name = myReader1.GetValue(myReader1.GetOrdinal("nick_name")).ToString();
                    area.first_name = myReader1.GetValue(myReader1.GetOrdinal("first_name")).ToString();
                    area.last_name =  myReader1.GetValue(myReader1.GetOrdinal("last_name")).ToString();
                    area.gender = myReader1.GetValue(myReader1.GetOrdinal("gender")).ToString();
                    area.areakm = Double.Parse(myReader1.GetValue(myReader1.GetOrdinal("areakm")).ToString());
                    area.userid = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("userid")).ToString());

                    tempresult.Add(area);
                }
                myReader1.Close();
                conn.Close();
                return (from t in GroupByArea(id, tempresult.AsEnumerable()) select t).OrderBy(t => t.id).ThenByDescending(t => t.UserKm).AsQueryable();
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<runopolyarea> GroupByArea(long id, IEnumerable<runopolyarearaw> rawareas)
        {
            return from e in rawareas
                   group e by new { e.id } into g
                   orderby g.Key.id
                   select new runopolyarea
                   {
                       id = g.Key.id,
                       name = g.FirstOrDefault().name,
                       latitude = g.FirstOrDefault().latitude,
                       longitude = g.FirstOrDefault().longitude,
                       radius1 = g.FirstOrDefault().radius1,
                       radius2 = g.FirstOrDefault().radius2,
                       rotation = g.FirstOrDefault().rotation,
                       level = g.Count(c => c.id > 0),
                       isUserArea = g.Any(c => c.userid == id),
                       MaxKm = g.Max(c => c.areakm),
                       TotalKm = Math.Round(g.Sum(c => c.areakm),1),
                       UserKm = g.Where(c => c.userid == id).Sum(c => c.areakm),
                       NoRuns = g.Count(),
                       owner = new runopolyuser {
                           nick_name = g.FirstOrDefault().nick_name,
                           first_name = g.FirstOrDefault().first_name,
                           last_name = g.FirstOrDefault().last_name,
                           gender = g.FirstOrDefault().gender,
                           id = g.FirstOrDefault().userid,
                           lastlogindate = DateTime.Now
                       }                         
                   };
        }
        /*
        public runopolyarea AreaGet(int id)
        {
            runopolyarea returnArea = null;
            try
            {
                string strSQL0 = "SELECT * from runopoly_areas WHERE id = " + id.ToString();
                OdbcCommand myCommand0 = new OdbcCommand(strSQL0, conn);
                conn.Open();
                OdbcDataReader myReader0 = myCommand0.ExecuteReader();
                while (myReader0.Read())
                {
                    returnArea = new runopolyarea();
                    returnArea.id = Int32.Parse(myReader0.GetValue(myReader0.GetOrdinal("id")).ToString());
                    returnArea.name = myReader0.GetValue(myReader0.GetOrdinal("name")).ToString();
                    returnArea.longitude = Double.Parse(myReader0.GetValue(myReader0.GetOrdinal("longitude")).ToString());
                    returnArea.latitude = Double.Parse(myReader0.GetValue(myReader0.GetOrdinal("latitude")).ToString());
                    returnArea.radius1 = Int32.Parse(myReader0.GetValue(myReader0.GetOrdinal("radius1")).ToString());
                    returnArea.radius2 = Int32.Parse(myReader0.GetValue(myReader0.GetOrdinal("radius1")).ToString());
                    returnArea.rotation = Int32.Parse(myReader0.GetValue(myReader0.GetOrdinal("rotation")).ToString());
                }
                myReader0.Close();
                conn.Close();
                return returnArea;
            }
            catch
            {
                return null;
            }
        }
        */

        // Run functions
        public bool RunAdd(runopolyrun run)
        {
            conn.Open();
            bool retVal = false;
            try
            {
                string strSQL = "INSERT INTO runopoly_runs (id, userid, areaid, totalkm, areakm, time, creationdate) VALUES (" +
                                    run.id.ToString() + ", " +
                                    run.userid.ToString() + ", " +
                                    run.areaid.ToString() + ", " +
                                    run.totalkm.ToString() + ", " +
                                    run.areakm.ToString() + ", " +
                                    run.time + ", " +
                                    run.creationdate;

                OdbcCommand myCommand = new OdbcCommand(strSQL, conn);
                retVal = (1 == myCommand.ExecuteNonQuery());
                return retVal;
            }
            catch
            {
                return retVal;
            }
            finally
            {
                conn.Close();
            }
        }
        public bool RunDelete(int id)
        {
            try
            {
                string strSql = "DELETE FROM runopoly_runs " +
                                "WHERE id = '" + id.ToString() + "'";
                conn.Open();
                OdbcCommand myCommand = new OdbcCommand(strSql, conn);
                bool eventDeleted = (1 == myCommand.ExecuteNonQuery());
                conn.Close();
                if (eventDeleted)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public IQueryable<runopolyrun> MyRunsGet(long userid)
        {
            conn.Open();
            try
            {
                string strSQL1 = "SELECT " +
                                   "id, " +
                                   "userid, " +
                                   "areaid, " +
                                   "totalkm, " +
                                   "areakm, " +
                                   "time, " +
                                   "creationdate " +
                                 "from runopoly_runs " +
                                 "where userid=" + userid.ToString();

                OdbcCommand myCommand1 = new OdbcCommand(strSQL1, conn);
                OdbcDataReader myReader1 = myCommand1.ExecuteReader();
                List<runopolyrun> runs = new List<runopolyrun>();

                while (myReader1.Read())
                {
                    runopolyrun run = new runopolyrun();
                    run.id = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("id")).ToString());
                    run.userid = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("userid")).ToString());
                    run.areaid = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("areaid")).ToString());
                    run.totalkm = Double.Parse(myReader1.GetValue(myReader1.GetOrdinal("totalkm")).ToString());
                    run.areakm = Double.Parse(myReader1.GetValue(myReader1.GetOrdinal("areakm")).ToString());
                    //run.time = TimeSpan.Parse(myReader1.GetValue(myReader1.GetOrdinal("time")).ToString());
                    run.creationdate = myReader1.GetDate(myReader1.GetOrdinal("creationdate"));
                    runs.Add(run);
                }
                myReader1.Close();
                conn.Close();
                return runs.AsQueryable() ;
            }
            catch
            {
                return null;
            }
        }
        public runopolyrun MyRunGet(int id)
        {
            conn.Open();
            try
            {
                string strSQL1 = "SELECT " +
                                   "id, " +
                                   "userid, " +
                                   "areaid, " +
                                   "totalkm, " +
                                   "areakm, " +
                                   "time, " +
                                   "creationdate " +
                                 "from runopoly_runs " +
                                 "where id=" + id.ToString();

                OdbcCommand myCommand1 = new OdbcCommand(strSQL1, conn);
                OdbcDataReader myReader1 = myCommand1.ExecuteReader();
                runopolyrun run = new runopolyrun();

                while (myReader1.Read())
                {
                    run.id = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("id")).ToString());
                    run.userid = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("userid")).ToString());
                    run.areaid = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("areaid")).ToString());
                    run.totalkm = Double.Parse(myReader1.GetValue(myReader1.GetOrdinal("totalkm")).ToString());
                    run.areakm = Double.Parse(myReader1.GetValue(myReader1.GetOrdinal("areakm")).ToString());
                    run.time = TimeSpan.Parse(myReader1.GetValue(myReader1.GetOrdinal("time")).ToString());
                    run.creationdate = myReader1.GetDate(myReader1.GetOrdinal("creationdate"));
                }
                myReader1.Close();
                conn.Close();
                return run ;
            }
            catch
            {
                return null;
            }
        }
        public string toMySQLDateTime(DateTime date)
        {
            return date.ToString("yyyy-MM-dd H:mm:ss");
        }  
        // Owner functions
        public IQueryable<runopolyowners> OwnersGet(long userid)
        {
            conn.Open();
            try
            {
                string strSQL1 = "SELECT " +
                                    " a.id, " +
                                    " a.name," +
                                    " b.nick_name, " +
                                    " b.first_name, " +
                                    " b.last_name, " +
                                    " b.gender, " +
                                    " c.areakm, " +
                                    " c.userid " +
                                 "from runopoly_areas a " +
                                 "left join runopoly_runs c " +
                                 "on a.id = c.areaid " +
                                 "left join runopoly_users b " +
                                 "on c.userid = b.id";

                OdbcCommand myCommand1 = new OdbcCommand(strSQL1, conn);
                OdbcDataReader myReader1 = myCommand1.ExecuteReader();
                List<runopolyarearaw> tempresult = new List<runopolyarearaw>();
                    
                while (myReader1.Read())
                {
                    runopolyarearaw area = new runopolyarearaw();
                    area.id = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("id")).ToString());
                    area.name = myReader1.GetValue(myReader1.GetOrdinal("name")).ToString();
                    area.nick_name = myReader1.GetValue(myReader1.GetOrdinal("nick_name")).ToString();
                    area.first_name = myReader1.GetValue(myReader1.GetOrdinal("first_name")).ToString();
                    area.last_name =  myReader1.GetValue(myReader1.GetOrdinal("last_name")).ToString();
                    area.gender = myReader1.GetValue(myReader1.GetOrdinal("gender")).ToString();
                    area.areakm = Double.Parse(myReader1.GetValue(myReader1.GetOrdinal("areakm")).ToString());
                    area.userid = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("userid")).ToString());
                    tempresult.Add(area);
                }
                myReader1.Close();
                conn.Close();
                return (from t in GroupByOwner(userid, tempresult.AsEnumerable()) select t).OrderBy(t => t.id).AsQueryable();
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<runopolyowners> GroupByOwner(long id, IEnumerable<runopolyarearaw> rawareas)
        {
            return from e in rawareas
                   group e by new { e.id } into g
                   orderby g.Key.id
                   select new runopolyowners
                   {
                       id = g.Key.id,
                       name = g.FirstOrDefault().name,
                       TotalKm = Math.Round(g.Sum(c => c.areakm), 1),
                       owners = (from f in g
                                 group f by new { f.userid } into u
                                    select new runopolyowner {
                                          NoRuns = u.Count(),
                                          UserKm = Math.Round(u.Sum(c => c.areakm),1),
                                          isUser = u.Any(c => c.userid == id),
                                          owner = new runopolyuser {
                                          nick_name = u.FirstOrDefault().nick_name,
                                          first_name = u.FirstOrDefault().first_name,
                                          last_name = u.FirstOrDefault().last_name,
                                          gender = u.FirstOrDefault().gender,
                                          id = u.Key.userid
                                         }
                                    }).OrderBy(t => -t.UserKm).ToList()
                   };
        }
  
  }
}