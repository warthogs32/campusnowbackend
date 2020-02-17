using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using backend.DTOs;
using backend.Models;

namespace backend.Repositories
{
    public class EventRepository
    {
        public EventRecord GetEventById(int eventId)
        {
            EventRecord retrievedEvent = new EventRecord();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["backend.Properties.Settings.mapsdb"].ConnectionString;
                conn.Open();
                string getEventQuery = "select * from cn.Events where ListingId = @eventId";
                using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@eventId", eventId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retrievedEvent = new EventRecord()
                            {
                                ListingId = Int32.Parse(reader["ListingId"].ToString()),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                StartTime = DateTime.Parse(reader["StartTime"].ToString()),
                                EndTime = DateTime.Parse(reader["EndTime"].ToString()),
                                LocX = float.Parse(reader["LocX"].ToString()),
                                LocY = float.Parse(reader["LocY"].ToString()),
                                UserId = Int32.Parse(reader["UserId"].ToString())
                            };
                        }
                    }
                }               
            }
            return retrievedEvent;
        }

        public List<EventRecord> GetAllEvents()
        {
            List<EventRecord> response = new List<EventRecord>();
            EventRecord retrievedEvent;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["backend.Properties.Settings.mapsdb"].ConnectionString))
            {
                conn.Open();
                string getEventQuery = "select * from cn.Events";
                using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retrievedEvent = new EventRecord()
                            {
                                ListingId = Int32.Parse(reader["ListingId"].ToString()),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                StartTime = DateTime.Parse(reader["StartTime"].ToString()),
                                EndTime = DateTime.Parse(reader["EndTime"].ToString()),
                                LocX = float.Parse(reader["LocX"].ToString()),
                                LocY = float.Parse(reader["LocY"].ToString()),
                                UserId = Int32.Parse(reader["UserId"].ToString())
                            };
                            response.Add(retrievedEvent);
                        }
                    }
                }
            }
            return response;
        }

        public bool PostNewEvent(EventRecord newEvent)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["backend.Properties.Settings.mapsdb"].ConnectionString))
                {
                    conn.Open();
                    string getEventQuery = @"insert into cn.Events (Title, Description, StartTime, EndTime, LocX, Locy, UserId) values
                        (@title, @description, @start, @end, @locX, @locY, @userId);";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", newEvent.Title);
                        cmd.Parameters.AddWithValue("@description", newEvent.Description);
                        cmd.Parameters.AddWithValue("@start", newEvent.StartTime);
                        cmd.Parameters.AddWithValue("@end", newEvent.EndTime);
                        cmd.Parameters.AddWithValue("@locX", newEvent.LocX);
                        cmd.Parameters.AddWithValue("@locY", newEvent.LocY);
                        cmd.Parameters.AddWithValue("@userId", newEvent.UserId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(SqlException e)
            {
                return false;
            }
            return true;
        }
    }
}