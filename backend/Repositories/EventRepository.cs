using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using backend.DTOs;

namespace backend.Repositories
{
    public class EventRepository
    {
        public GetEventByIdResponseDTO GetEventById(GetEventByIdRequestDTO eventId)
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

        public GetAllEventsResponseDTO GetAllEvents()
        {
            GetAllEventsResponseDTO response = new GetAllEventsResponseDTO();



            return response;
        }

        public PostNewEventResponseDTO PostNewEvent(PostNewEventRequestDTO newEvent)
        {
            PostNewEventResponseDTO response = new PostNewEventResponseDTO();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["backend.Properties.Settings.mapsdb"].ConnectionString))
            {
                conn.Open();
                string getEventQuery = "";
                using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                {
                    //cmd.Parameters.AddWithValue();
                    
                }
            }
            return response;
        }
    }
}