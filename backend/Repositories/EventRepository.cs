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
        private String table = "cn.Events";
        private bool DoesEventBelongToUser(int currentEventId)
        {
            bool permit;
            using(SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = backend.Properties.Resources.sqlconnection;
                conn.Open();
                string checkEventQuery = "select * from @table where ListingId = @ListingId and UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(checkEventQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ListingId", currentEventId);
                    cmd.Parameters.AddWithValue("@UserId", LoginRepository.CurrentUser.UserId);
                    cmd.Parameters.AddWithValue("@table", table);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            permit = Int32.Parse(reader["UserId"].ToString()) == LoginRepository.CurrentUser.UserId
                                && Int32.Parse(reader["ListingId"].ToString()) == currentEventId;
                        }
                        else 
                        {
                            permit = false;
                        }
                    }
                }

            }
            return permit;
        }

        public EventRecord GetEventById(int eventId)
        {
            EventRecord retrievedEvent = new EventRecord();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = backend.Properties.Resources.sqlconnection;
                conn.Open();
                string getEventQuery = "select * from @table where ListingId = @eventId";
                using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@eventId", eventId);
                    cmd.Parameters.AddWithValue("@table", table);
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
            using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
            {
                conn.Open();
                string getEventsQuery = "select * from @table";
                using (SqlCommand cmd = new SqlCommand(getEventsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@table", table);
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
                using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
                {
                    conn.Open();
                    string getEventQuery = @"insert into @table (Title, Description, StartTime, EndTime, LocX, Locy, UserId) values
                        (@title, @description, @start, @end, @locX, @locY, @userId);";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", newEvent.Title);
                        cmd.Parameters.AddWithValue("@description", newEvent.Description);
                        cmd.Parameters.AddWithValue("@start", newEvent.StartTime);
                        cmd.Parameters.AddWithValue("@end", newEvent.EndTime);
                        cmd.Parameters.AddWithValue("@locX", newEvent.LocX);
                        cmd.Parameters.AddWithValue("@locY", newEvent.LocY);
                        cmd.Parameters.AddWithValue("@userId", LoginRepository.CurrentUser.UserId);

                        cmd.Parameters.AddWithValue("@table", table);

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

        public bool UpdateEvent(EventRecord updated)
        {
            if (DoesEventBelongToUser(updated.ListingId))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
                    {
                        conn.Open();
                        String updateEventQuery = @"update @table
                        set Title = @title, Description = @description, StartTime = @start, EndTime = @end, LocX = @LocX, LocY = @LocY
                        where ListingId = @id;";
                        using (SqlCommand cmd = new SqlCommand(updateEventQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@title", updated.Title);
                            cmd.Parameters.AddWithValue("@description", updated.Description);
                            cmd.Parameters.AddWithValue("@start", updated.StartTime);
                            cmd.Parameters.AddWithValue("@end", updated.EndTime);
                            cmd.Parameters.AddWithValue("@LocX", updated.LocX);
                            cmd.Parameters.AddWithValue("@LocY", updated.LocY);
                            cmd.Parameters.AddWithValue("@id", updated.ListingId);

                            cmd.Parameters.AddWithValue("@table", table);

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

        public bool DeleteEvent(int eventId)
        {
            if (DoesEventBelongToUser(eventId))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
                    {
                        conn.Open();
                        string deleteEventQuery = "delete from @table where ListingId = @eventId";
                        using (SqlCommand cmd = new SqlCommand(deleteEventQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@eventId", eventId);
                            cmd.Parameters.AddWithValue("@table", table);
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
            else
            {
                return false;
            }
        }

        public List<EventRecord> GetEventsByUserId(int UserId)
        {
            List<EventRecord> response = new List<EventRecord>();
            EventRecord userEvent;
            try 
            {
                using(SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
                {
                    conn.Open();
                    string getEventByUserIdQuery = @"select * from @table where UserId = @UserId;";
                    using (SqlCommand cmd = new SqlCommand(getEventByUserIdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        cmd.Parameters.AddWithValue("@table", table);
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userEvent = new EventRecord()
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
                                response.Add(userEvent);
                            }
                        }
                    }
                }
            }
            catch(SqlException e)
            {
                return new List<EventRecord>();
            }
            return response;
        }

        public List<EventRecord> GetEventsByTimeRange(DateTime startTime, DateTime endTime)
        {
            List<EventRecord> response = new List<EventRecord>();
            EventRecord userEvent;
            
            try
            {
                using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
                {
                    conn.Open();
                    string getEventByUserIdQuery = @"select * from @table where StartTime > @startTime and StartTime < @endTime;";
                    using (SqlCommand cmd = new SqlCommand(getEventByUserIdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@startTime", startTime);
                        cmd.Parameters.AddWithValue("@endTime", endTime);
                        cmd.Parameters.AddWithValue("@database", table);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userEvent = new EventRecord()
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
                                response.Add(userEvent);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                return new List<EventRecord>();
            }
            return response;
        }
        public bool ClearEvents()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
                {
                    conn.Open();
                    string deleteEventQuery = "delete from @table";
                    using (SqlCommand cmd = new SqlCommand(deleteEventQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException e)
            {
                return false;
            }
        }
    }
}
