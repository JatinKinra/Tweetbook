using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Cache;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Requests.Queries;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Extensions;
using Tweetbook.Helpers;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUriService _uriService;

        public PostsController(IPostService postService, IUriService uriService)
        {
            _postService = postService;
            _uriService = uriService;
        }

        /// <summary>
        /// Returns all the Posts in the system.
        /// </summary>
        /// <response code = "200">Returs all the posts in the system</response>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Posts.GetAll)]
        [Cached(600)]
        public async Task<IActionResult> GetAll([FromQuery] string userId, [FromQuery]PaginationQuery paginationQuery)
        {
            var paginationFilter = new PaginationFilter
            {
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize
            };

            var posts = await _postService.GetPostsAsync(userId, paginationFilter);

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return Ok(new PagedResponse<Post>(posts));
            }

            var paginationResponse = PaginationHelper.CreatePaginationResponse(_uriService, paginationFilter, posts);

            return Ok(paginationResponse);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest requestPost)
        {
            var post = new Post 
            { 
                Name = requestPost.Name,
                UserId = HttpContext.GetUserid()
            };

            await _postService.CreatePostAsync(post);

            var locationUri = _uriService.GetPostUri(post.Id.ToString());

            var response = new PostResponse { Id = post.Id };
            return Created(locationUri, new Response<PostResponse>(response));
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        [Cached(600)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await  _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();

            return Ok(new Response<Post>(post));
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserid());
            if (!userOwnsPost)
            {
                return BadRequest(new { Error = "You do not own this post." });
            }

            var post = await _postService.GetPostByIdAsync(postId);
            post.Name = request.Name;

            var updated = await _postService.UpdatePostAsync(post);
            if (updated)
                return Ok(new Response<Post>(post));
            
            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserid());
            if (!userOwnsPost)
            {
                return BadRequest(new { Error = "You do not own this post." });
            }

            var deleted = await _postService.DeletePostAsync(postId);
            if (deleted)
                return NoContent();
            
            return NotFound();
        }
    }
}
