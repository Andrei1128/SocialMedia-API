using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using SocialMedia.Models.DTOs;
using SocialMedia.Repository.IRepository;
using SocialMedia.Utilities;
using System.Net;

namespace SocialMedia.Controllers
{
    [Route("post")]
    [ApiController]
    public class PostController : CustomControllerBase
    {
        protected APIResponse _response;
        private readonly IUserRepository _dbUser;
        private readonly IPostRepository _dbPost;
        private readonly IGroupRepository _dbGroup;
        private readonly IMapper _mapper;
        public PostController(IPostRepository dbPost, IUserRepository dbUser, IGroupRepository dbGroup, IMapper mapper)
        {
            this._response = new APIResponse();
            _mapper = mapper;
            _dbGroup = dbGroup;
            _dbPost = dbPost;
            _dbUser = dbUser;
        }

        //[HttpGet]
        //[Authorize]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<APIResponse>> GetPosts([FromQuery] int pageSize = 24, int pageNumber = 1)
        //{
        //    try
        //    {
        //        IEnumerable<Post> postList = await _dbPost.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
        //        _response.Result = _mapper.Map<List<PostDTO>>(postList);
        //        _response.StatusCode = HttpStatusCode.OK;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages = new List<string>() { ex.ToString() };
        //    }
        //    return _response;
        //}

        [HttpGet("{id:int}", Name = "GetPost")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPost(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var post = await _dbPost.GetAsync(item => item.Id == id);
                if (post == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<PostDTO>(post);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()
    };
            }
            return _response;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreatePost([FromBody] PostCreatedDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                    return BadRequest(createDTO);
                Post model = _mapper.Map<Post>(createDTO);
                if (createDTO.GroupId != 0)
                {
                    Group group = await _dbGroup.GetAsync(g => g.Id == createDTO.GroupId);
                    if (group == null)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Group not found!");
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }
                    model.Group = group;
                    model.GroupId = group.Id;
                    if (group.Posts == null)
                        group.Posts = new List<Post>();
                    group.Posts.Add(model);
                }
                await _dbPost.CreateAsync(model);
                if (createDTO.GroupId != 0)
                    await _dbGroup.SaveAsync();
                int myId = await GetMyId();
                User myUser = await _dbUser.GetAsync(u => u.Id == myId);
                if (myUser.Posts == null)
                {
                    myUser.Posts = new List<Post>();
                }
                myUser.Posts.Add(model);
                await _dbUser.SaveAsync();
                _response.Result = _mapper.Map<PostDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPost", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePost(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var post = await _dbPost.GetAsync(item => item.Id == id);
                if (post == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                int myId = await GetMyId();
                if (post.AuthorId != myId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    return Unauthorized(_response);
                }
                await _dbPost.RemoveAsync(post);
                _response.Result = _mapper.Map<PostDTO>(post);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdatePost(int id, [FromBody] PostUpdatedDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var post = await _dbPost.GetAsync(item => item.Id == id);
                if (post == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                int myId = await GetMyId();
                if (post.AuthorId != myId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    return Unauthorized(_response);
                }
                Post model = _mapper.Map<Post>(updateDTO);
                await _dbPost.UpdateAsync(model);
                _response.Result = _mapper.Map<PostDTO>(post);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
