using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace JWTSimpleServer
{
    public class JwtSimpleServerContext
    {
        private JwtSimpleServerContext(string userName, string password)
        {
            UserName = UserName;
            Password = password;
        }

        public static JwtSimpleServerContext Create(string userName, string password)
        {
            return new JwtSimpleServerContext(userName, password);
        }

        public string UserName { get; }
        public string Password { get; }
        private bool _contextValidated = false;
        private string _errorMessage = "Invalid authentication";
        public ClaimsIdentity claimsIdentity = null;
        public bool IsValid() => _contextValidated;

        public string GetError() => _errorMessage;

        public void Reject(string errorMessage)
        {
            _contextValidated = false;
            _errorMessage = errorMessage;
        }
        public void Success(ClaimsIdentity identity)
        {
            _contextValidated = true;
            claimsIdentity = identity;
        }

    }
}
