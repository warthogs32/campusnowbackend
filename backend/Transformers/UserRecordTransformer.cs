using System;
using System.Collections.Generic;
using System.Linq;
using backend.DTOs;
using System.Web;
using backend.Models;

namespace backend.Transformers
{
    public class UserRecordTransformer
    {
        public static UserRecordDTO Transform(UserRecord user)
        {
            if(user == null)
            {
                return null;
            }
            var dto = new UserRecordDTO()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                JoinDate = user.JoinDate,
                IsAdmin = Convert.ToInt32(user.IsAdmin)
            };

            return dto;
        }

        public static UserRecord Transform(UserRecordDTO dto)
        {
            if(dto == null)
            {
                return null;
            }
            var user = new UserRecord()
            {
                UserId = dto.UserId,
                UserName = dto.UserName,
                Password = dto.Password,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                JoinDate = dto.JoinDate,
                IsAdmin = Convert.ToBoolean(dto.IsAdmin)
            };

            return user;
        }
    }
}
