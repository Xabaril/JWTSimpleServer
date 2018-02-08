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
        private string _errorMessage = ServerMessages.InvalidAuthentication;
        internal List<Claim> Claims { get; private set;  } = null;
        internal bool IsValid() => _contextValidated;

        internal string GetError() => _errorMessage;

        public void Reject(string errorMessage)
        {
            _contextValidated = false;
            _errorMessage = errorMessage;
        }
        public void Success(List<Claim> claims)
        {
            _contextValidated = true;
            Claims = claims;
        }

    }
}
