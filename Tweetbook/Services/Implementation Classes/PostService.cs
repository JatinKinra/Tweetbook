using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dataContext.Posts.AddAsync(post);
            int createdCount = await _dataContext.SaveChangesAsync();
            return createdCount > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);
            _dataContext.Posts.Remove(post);
            int deletedCount = await _dataContext.SaveChangesAsync();
            return deletedCount > 0;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<List<Post>> GetPostsAsync(string userId, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Posts.AsQueryable();
            if (paginationFilter == null)
            {
                return await queryable.ToListAsync();
            }

            if (!string.IsNullOrEmpty(userId))
            {
                queryable = queryable.Where(x => x.UserId == userId);
            }

            var skipCount = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Skip(skipCount).Take(paginationFilter.PageSize).ToListAsync();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            _dataContext.Posts.Update(postToUpdate);
            int updatedCount = await _dataContext.SaveChangesAsync();
            return updatedCount > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);
            if (post == null)
                return false;

            if (post.UserId != userId)
                return false;

            return true;
        }
    }
}
