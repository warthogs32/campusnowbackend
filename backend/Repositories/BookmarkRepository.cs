using System;
using System.Collections.Generic;
using System.Linq;
using backend.Exceptions;
using System.Web;
using System.Data.SqlClient;
using backend.Models;

namespace backend.Repositories
{
    public class BookmarkRepository
    {
        private string _sqlConnectionString;
        public BookmarkRepository(bool isTest = false)
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

        public string AddNewBookmark(EventRecord record)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventQuery = @"insert into cn.Bookmarks (UserId, Listing) values (@user, @event)";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", LoginRepository.CurrentUser.UserId);
                        cmd.Parameters.AddWithValue("@event", record.ListingId);
                        cmd.ExecuteNonQuery();
                    }
                }
                return string.Format("Successfully created bookmark for {0}", record.Title);
            }
            catch (SqlException e)
            {
                throw new RepoException(e.Message);
            }
        }

        public List<EventRecord> GetAllBookmarkedEvents()
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
                        cmd.Parameters.AddWithValue("@user", LoginRepository.CurrentUser.UserId);
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
                throw new RepoException(e.Message);
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
                        string resetIdentityQuery = @"DBCC CHECKIDENT('cn.Bookmarks', RESEED, 0)";
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
                return false;
            }
        }
    }
}