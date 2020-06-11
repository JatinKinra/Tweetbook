﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1.Requests.Queries;

namespace Tweetbook.Services
{
    public interface IUriService
    {
        Uri GetPostUri(string postId);

        Uri GetAllPostUri(PaginationQuery pagination = null);
    }
}
