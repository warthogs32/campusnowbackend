using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using backend.Models;

namespace backend.Repositories
{
    public class UserRepository
    {
        private string _sqlConnectionString;
        public UserRepository(bool isTest = false)
        {
            if (isTest)
            {
                _sqlConnectionString = Properties.Resources.testsqlconnection;
            }
            else
            {
                _sqlConnectionString = Properties.Resources.sqlconnection;
            }
        }
        public bool PostNewUser(UserRecord newUser)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventQuery = @"insert into cn.Users (UserName, Password, FirstName, LastName, JoinDate, IsAdmin) values
                        (@UserName, @Password, @FirstName, @LastName, @JoinDate, @IsAdmin);";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", newUser.UserName);
                        cmd.Parameters.AddWithValue("@Password", newUser.Password);
                        cmd.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", newUser.LastName);
                        cmd.Parameters.AddWithValue("@JoinDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IsAdmin", Convert.ToInt32(false));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool AddNewBookmark (UserRecord user, EventRecord record)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventQuery = @"insert into cn.Bookmarks (UserId, Listing) values (@user, @event)";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", user.UserId);
                        cmd.Parameters.AddWithValue("@event", record.ListingId);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (SqlException e)
            {
                return false;
            }
        }
      

        public List<EventRecord> GetAllBookmarkedEvents(UserRecord user)
        {
            List<EventRecord> events = new List<EventRecord>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventQuery = @"select Title, Description, StartTime, EndTime, LocX, LocY, cn.Bookmarks.UserId, Listing
                        from cn.Bookmarks join cn.Events on ListingId = Listing where cn.Bookmarks.UserId = @user";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", user.UserId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                events.Add(new EventRecord()
                                {
                                    ListingId = Int32.Parse(reader["Listing"].ToString()),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    StartTime = DateTime.Parse(reader["StartTime"].ToString()),
                                    EndTime = DateTime.Parse(reader["EndTime"].ToString()),
                                    LocX = float.Parse(reader["LocX"].ToString()),
                                    LocY = float.Parse(reader["LocY"].ToString()),
                                    UserId = Int32.Parse(reader["UserId"].ToString())
                                });
                            }
                        }
                    }
                }
                return events;
            }
            catch (SqlException e)
            {
                return events;
            }
        }

        /*
         * The Methods below are only to be used for sql testing,
         * and will not work on the production database
         */

        public bool ResetBookmarks()
        {
            if (_sqlConnectionString == Properties.Resources.testsqlconnection)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        string getEventQuery = @"delete from cn.Bookmarks;";
                        using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException e)
                {
                    return false;
                }
                return true;
            }
            else
            {
                Console.WriteLine("You cannot delete all users from the production database");
                return false;
            }
        }
        public bool ResetUsers()
        { 
            if (_sqlConnectionString == Properties.Resources.testsqlconnection)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        string getEventQuery = @"delete from cn.Users;";
                        using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException e)
                {
                    return false;
                }
                return true;
            }
            else
            {
                Console.WriteLine("You cannot delete all users from the production database");
                return false;
            }
        }

        public bool ResetAutoIncrement()
        {
            if (_sqlConnectionString == Properties.Resources.testsqlconnection)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        string resetIdentityQuery = @"DBCC CHECKIDENT('cn.Users', RESEED, 0)";
                        using (SqlCommand cmd = new SqlCommand(resetIdentityQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        resetIdentityQuery = @"DBCC CHECKIDENT ('cn.Bookmarks', RESEED, 0)";
                        using (SqlCommand cmd = new SqlCommand(resetIdentityQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException e)
                {
                    return false;
                }
                return true;
            }
            else
            {
                Console.WriteLine("You cannot reset an identity in the production database");
                return false;
            }
        }
    }
}