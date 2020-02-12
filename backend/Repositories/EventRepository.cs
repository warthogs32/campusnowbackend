using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using backend.Models;
using System.Data.SqlClient;
using System.Configuration;
using backend.DTOs;

namespace backend.Repositories
{
    public class EventRepository
    {
        public GetEventByIdResponseDTO GetEventById(int eventId)
        {
            GetEventByIdResponseDTO retrievedEvent = new GetEventByIdResponseDTO();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["backend.Properties.Settings.mapsdb"].ConnectionString))
            {
                conn.Open();
                string getEventQuery = "select * from cn.Events where ListingId = @eventId";
                using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@eventId", eventId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retrievedEvent = new GetEventByIdResponseDTO()
                            {
                                //ListingId = Int32.Parse(reader["ListingId"].ToString())
                            };
                        }
                    }
                }               
            }
            return retrievedEvent;
        }

        public bool PostNewEvent()
        {
            EventModel newEvent = new EventModel();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["backend.Properties.Settings.mapsdb"].ConnectionString))
            {
                conn.Open();
                string getEventQuery = "select * from cn.Events where ListingId = @eventId";
                using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@eventId", eventId);
                    
                }
            }

        }
    }
}