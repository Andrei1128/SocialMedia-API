﻿using AutoMapper;
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

        [HttpGet("{id:int}", Name = "GetPost")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                var post = await _dbPost.GetAsync(p => p.Id == id, includeProprieties: "Likes,Comments");
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
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }

        [HttpPatch("AddLike/{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> AddLike(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Invalid Id");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Post post = await _dbPost.GetAsync(u => u.Id == id, includeProprieties: "Likes");
                if (post == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Post not found!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                int myId = await GetMyId();
                User myUser = await _dbUser.GetAsync(u => u.Id == myId);
                post.Likes.Add(myUser);
                await _dbPost.SaveAsync();
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }

        [HttpPatch("AddComment/{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> AddComment(int id, [FromBody] PostCreatedDTO comment)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Invalid Id");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Post post = await _dbPost.GetAsync(u => u.Id == id, includeProprieties: "Comments");
                if (post == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Post not found!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                comment.GroupId = null;
                Post model = _mapper.Map<Post>(comment);
                int myId = await GetMyId();
                User myUser = await _dbUser.GetAsync(u => u.Id == myId);
                model.Author = myUser;
                await _dbPost.CreateAsync(model);
                post.Comments.Add(model);
                await _dbPost.SaveAsync();
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreatePost([FromBody] PostCreatedDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                    return BadRequest(createDTO);
                Post model = _mapper.Map<Post>(createDTO);
                int myId = await GetMyId();
                if (createDTO.GroupId != 0 && createDTO.GroupId != null)
                {
                    Group group = await _dbGroup.GetAsync(g => g.Id == createDTO.GroupId, includeProprieties: "Participants,Posts");
                    if (group == null)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Group not found!");
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }
                    bool userIsInGroup = group.Participants.Any(p => p.Id == myId);
                    if (!userIsInGroup)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("You are not member of this group!");
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_response);
                    }
                    model.Group = group;
                    model.GroupId = group.Id;
                    group.Posts.Add(model);
                }
                else
                {
                    model.GroupId = null;
                }
                User myUser = await _dbUser.GetAsync(u => u.Id == myId, includeProprieties: "Posts");
                model.Author = myUser;
                await _dbPost.CreateAsync(model);
                if (createDTO.GroupId != 0)
                    await _dbGroup.SaveAsync();
                myUser.Posts.Add(model);
                await _dbUser.SaveAsync();
                _response.Result = _mapper.Map<PostDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPost", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                var post = await _dbPost.GetAsync(p => p.Id == id, includeProprieties: "Author");
                if (post == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                int myId = await GetMyId();
                if (post.Author.Id != myId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    return Unauthorized(_response);
                }
                User myUser = await _dbUser.GetAsync(u => u.Id == myId, includeProprieties: "Posts");
                myUser.Posts.Remove(post);
                await _dbUser.SaveAsync();
                await _dbPost.RemoveAsync(post);
                _response.Result = _mapper.Map<PostDTO>(post);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }

        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                var post = await _dbPost.GetAsync(p => p.Id == id, includeProprieties: "Author");
                if (post == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                int myId = await GetMyId();
                if (post.Author.Id != myId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    return Unauthorized(_response);
                }
                post.ImageURL = updateDTO.ImageURL;
                post.Content = updateDTO.Content;
                await _dbPost.UpdateAsync(post);
                _response.Result = _mapper.Map<PostDTO>(post);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }
    }
}
