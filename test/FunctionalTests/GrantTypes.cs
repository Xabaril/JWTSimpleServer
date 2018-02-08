using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace FunctionalTests
{
    public class GrantTypes
    {
        public static FormUrlEncodedContent AnInvalidGrantType()
        {
            var formValues = new Dictionary<string, string>
            {
                { "grant_type" , "foo"}
            };

            return new FormUrlEncodedContent(formValues);
        }

        public static FormUrlEncodedContent APasswordGrantType()
        {
            var formValues = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"username", "SimpleServer" },
                {"password", "SimpleServerPassword" }
            };

            return new FormUrlEncodedContent(formValues);
        }

        public static FormUrlEncodedContent ARefreshTokenGranType(string refreshToken)
        {
            var formValues = new Dictionary<string, string>
            {
                {"grant_type", "refresh_token"},
                {"refresh_token", refreshToken }                
            };

            return new FormUrlEncodedContent(formValues);
        }
    }
}
