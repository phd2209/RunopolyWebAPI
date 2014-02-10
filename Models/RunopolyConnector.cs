using System;
using System.Web;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

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
        public bool UserAdd(runopolyuser user)
        {
            try
            {
                conn.Open();
                string strSQL = "INSERT INTO runopoly_users (id, first_name, last_name, gender, email) VALUES (" +
                                  user.id.ToString() + ", " +
                                  "'" + user.first_name + "', " +
                                  "'" + user.last_name + "', " +
                                  "'" + user.gender + "', " +
                                  "'" + user.email + "'";

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
        public bool UserUpdate(runopolyuser user)
        {
            try
            {
                conn.Open();
                string strSQL = "UPDATE runopoly_users SET " +
                                    "first_name = " + "'" + user.first_name + "', " +
                                    "last_name = " + "'" + user.last_name + "', " +
                                    "gender = " + "'" + user.gender + "', " +
                                    "email = " + "'" + user.email + "' " +
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
        public bool AddUpdateUser(runopolyuser user)
        {
            bool success = false;
            try
            {

                if (UserExists(user.id))
                    success = UserUpdate(user);
                else
                    success = UserAdd(user);
                return success;
            }
            catch
            {
                return false;
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
        public runopolyuser UserGet(int id)
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
                    returnUser.first_name = myReader0.GetValue(myReader0.GetOrdinal("first_name")).ToString();
                    returnUser.last_name = myReader0.GetValue(myReader0.GetOrdinal("last_name")).ToString();
                    returnUser.gender = myReader0.GetValue(myReader0.GetOrdinal("gender")).ToString();
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
        public int UsersTotal()
        {
            int total = 0;
            try
            {
                string strSQL0 = "SELECT Count(Distinct id) as count from runopoly_users";
                OdbcCommand myCommand0 = new OdbcCommand(strSQL0, conn);
                conn.Open();
                OdbcDataReader myReader0 = myCommand0.ExecuteReader();
                while (myReader0.Read())
                {
                    total = Int32.Parse(myReader0.GetValue(myReader0.GetOrdinal("count")).ToString());
                }
                myReader0.Close();
                conn.Close();
                return total;
            }
            catch
            {
                return 0;
            }
        }    

        // Area functions
        public IQueryable<runopolyarea> AreasGet()
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
                                    " b.first_name, " +
                                    " b.last_name, " +
                                    " c.areakm, " +
                                    " c.userid " +
                                 "from runopoly_areas a " +
                                 "left join runopoly_runs c " +
                                 "on a.id = c.areaid " +
                                 "left join runopoly_users b " +
                                 "on c.userid = b.id";

                OdbcCommand myCommand1 = new OdbcCommand(strSQL1, conn);
                OdbcDataReader myReader1 = myCommand1.ExecuteReader();
                List<runopolyarea> tempresult = new List<runopolyarea>();

                while (myReader1.Read())
                {
                    runopolyarea area = new runopolyarea();
                    area.id = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("id")).ToString());
                    area.name = myReader1.GetValue(myReader1.GetOrdinal("name")).ToString();
                    area.longitude = Double.Parse(myReader1.GetValue(myReader1.GetOrdinal("longitude")).ToString());
                    area.latitude = Double.Parse(myReader1.GetValue(myReader1.GetOrdinal("latitude")).ToString());
                    area.radius1 = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("radius1")).ToString());
                    area.radius2 = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("radius2")).ToString());
                    area.rotation = Int32.Parse(myReader1.GetValue(myReader1.GetOrdinal("rotation")).ToString());
                    area.owner = myReader1.GetValue(myReader1.GetOrdinal("first_name")).ToString() + " " + myReader1.GetValue(myReader1.GetOrdinal("last_name")).ToString();
                    tempresult.Add(area);
                }
                myReader1.Close();
                conn.Close();
                return tempresult.AsQueryable();
            }
            catch
            {
                return null;
            }
        }
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
        public IQueryable<runopolyrun> MyRunsGet(long id)
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
                                 "where userid=" + id.ToString();

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
                    run.time = TimeSpan.Parse(myReader1.GetValue(myReader1.GetOrdinal("time")).ToString());
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
  }
}