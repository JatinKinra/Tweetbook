using System;
using System.Collections.Generic;

namespace Tweetbook.Contracts.V1.Responses
{
    public class AuthFailureResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
