using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Contracts.V1.Requests.Queries
{
    public class PaginationQuery
    {

        public PaginationQuery()
        {
            PageNumber = 1;
            PageSize = 100;
        }

        public PaginationQuery(int pageNumber, int pagesize)
        {
            PageNumber = pageNumber;
            PageSize = pagesize;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
