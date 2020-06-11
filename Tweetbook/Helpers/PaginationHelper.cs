using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1.Requests.Queries;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Helpers
{
    public class PaginationHelper
    {
        public static object CreatePaginationResponse<T>(IUriService uriService, PaginationFilter paginationFilter, List<T> posts)
        {
            var nextPage = paginationFilter.PageNumber >= 1
                ? uriService.GetAllPostUri(new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize)).ToString()
                : null;

            var previousPage = paginationFilter.PageNumber - 1 >= 1
                ? uriService.GetAllPostUri(new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)).ToString()
                : null;

            var paginationResponse = new PagedResponse<T>
            {
                Data = posts,
                PageNumber = paginationFilter.PageNumber >= 1 ? paginationFilter.PageNumber : (int?)null,
                PageSize = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : (int?)null,
                NextPage = posts.Any() ? nextPage : null,
                PreviousPage = previousPage
            };

            return paginationResponse;
        }
    }
}
