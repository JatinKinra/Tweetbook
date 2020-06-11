using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public static readonly string Base = $"{Root}/{Version}"; 

        public static class Posts 
        {
            public const string GetAll = "api/v1/" + "posts";
            public const string Create = "api/v1/" + "posts";
            public const string Get = "api/v1/" + "posts/{postId}";
            public const string Update = "api/v1/" + "posts/{postId}";
            public const string Delete = "api/v1/" + "posts/{postId}";
        }

        public static class Identity
        {
            public const string LogIn = "api/v1/" + "identity/login";
            public const string Register = "api/v1/" + "identity/register";
            public const string Refresh = "api/v1/" + "identity/refresh";
        }
    }
}
