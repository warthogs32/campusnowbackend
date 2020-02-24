using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace backend.Models
{
	[ExcludeFromCodeCoverage]
    public class UserRecord
    {
		private int _userId;
		public int UserId
		{
			get { return _userId; }
			set { _userId = value; }
		}

		private string _userName;
		public string UserName
		{
			get { return _userName; }
			set { _userName = value.ToLower(); }
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		private string _firstName;
		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		private string _lastName;
		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		private DateTime _joinDate;
		public DateTime JoinDate
		{
			get { return DateTime.Now; }
			set { _joinDate = value;  }
		}

		private string _token;
		public string Token
		{
			get { return _token; }
			set { _token = value; }
		}

		private bool _isAdmin;
		public bool IsAdmin
		{
			get { return _isAdmin; }
			set { _isAdmin = value; }
		}

	}
}