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
    [Route("user")]
    [ApiController]
    public class UserController : CustomControllerBase
    {
        private readonly IUserRepository _dbUser;
        private readonly IGroupRepository _dbGroup;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        public UserController(IUserRepository dbUser, IGroupRepository dbGroup, IMapper mapper)
        {
            _mapper = mapper;
            this._response = new APIResponse();
            _dbUser = dbUser;
            _dbGroup = dbGroup;
        }

        [HttpGet("GetFeed")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetFeed([FromQuery] int pageSize = 24, int pageNumber = 1)
        {
            try
            {
                int myId = await GetMyId();
                var feed = await _dbUser.GetFeed(myId, pageSize: pageSize, pageNumber: pageNumber);
                List<PostDTO> feedDTO = _mapper.Map<List<PostDTO>>(feed);
                _response.Result = feedDTO;
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

        [HttpGet("FindPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<APIResponse>> FindPeople([FromQuery] string find, int pageSize = 24, int pageNumber = 1)
        {
            try
            {
                if (find == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Query is missing!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                else
                {
                    IEnumerable<User> userList = await _dbUser.GetAllAsync(u => u.Name.ToLower().Contains(find.ToLower()), includeProprieties: "Friends,Groups,Posts", pageSize: pageSize, pageNumber: pageNumber);
                    _response.Result = _mapper.Map<List<UserDTO>>(userList);
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }
        [HttpPatch("AddFriend/{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> AddFriend(int id)
        {
            try
            {
                int myId = await GetMyId();
                if (id == 0 || myId == id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                User userFound = await _dbUser.GetAsync(u => u.Id == id, includeProprieties: "Requests,Friends");
                if (userFound == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User not found!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                bool friendRequestExists = false;
                bool friendExists = false;

                friendRequestExists = userFound.Requests.Any(r => r.UserId == id && r.RequestedUserId == myId);
                friendExists = userFound.Friends.Any(f => f.Id == myId);
                if (friendRequestExists || friendExists)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Friend request/Friend already exists.");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                UserRequest Request = new UserRequest
                {
                    UserId = id,
                    RequestedUserId = myId
                };
                if (userFound.Requests == null)
                    userFound.Requests = new List<UserRequest>();
                userFound.Requests.Add(Request);
                await _dbUser.SaveAsync();
                _response.IsSuccess = true;
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
        [HttpPatch("AcceptRequest/{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> AcceptRequest(int id)
        {
            try
            {
                int myId = await GetMyId();
                if (id == 0 || myId == id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                User myUser = await _dbUser.GetAsync(u => u.Id == myId, includeProprieties: "Requests,Friends");

                UserRequest request = myUser.Requests.FirstOrDefault(r => r.RequestedUserId == id);
                if (request == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Friend request not found!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                User friend = await _dbUser.GetAsync(u => u.Id == id, includeProprieties: "Friends");
                if (friend == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Friend user not found!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                myUser.Requests.Remove(request);


                if (myUser.Friends == null)
                    myUser.Friends = new List<User>();
                myUser.Friends.Add(friend);

                if (friend.Friends == null)
                    friend.Friends = new List<User>();
                friend.Friends.Add(myUser);


                await _dbUser.SaveAsync();


                _response.IsSuccess = true;
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
        [HttpPatch("DeclineRequest/{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeclineRequest(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                int myId = await GetMyId();
                User myUser = await _dbUser.GetAsync(u => u.Id == myId, includeProprieties: "Requests");

                UserRequest request = myUser.Requests.FirstOrDefault(r => r.UserId == myId);
                if (request == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Friend request not found!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                myUser.Requests.Remove(request);
                await _dbUser.SaveAsync();

                _response.IsSuccess = true;
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
        [HttpPatch("RemoveFriend/{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> RemoveFriend(int id)
        {
            try
            {
                int myId = await GetMyId();
                if (id == 0 || myId == id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                User myUser = await _dbUser.GetAsync(u => u.Id == myId, includeProprieties: "Friends");
                User friend = await _dbUser.GetAsync(u => u.Id == id, includeProprieties: "Friends");

                if (friend == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Friend not found!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                friend.Friends.Remove(myUser);
                myUser.Friends.Remove(friend);
                await _dbUser.SaveAsync();

                _response.IsSuccess = true;
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
        [HttpPatch("EnterGroup/{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> EnterGroup(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                Group group = await _dbGroup.GetAsync(g => g.Id == id, includeProprieties: "Participants");

                if (group == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Group not found!");
                    return NotFound(_response);
                }

                int myId = await GetMyId();
                User myUser = await _dbUser.GetAsync(u => u.Id == myId, includeProprieties: "Groups");
                bool alreadyInGroup = myUser.Groups.Any(g => g.Id == group.Id);
                if (alreadyInGroup)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("You are alredy in this group!");
                    return BadRequest(_response);
                }

                group.Participants.Add(myUser);
                myUser.Groups.Add(group);
                await _dbUser.SaveAsync();
                await _dbGroup.SaveAsync();

                _response.IsSuccess = true;
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

    }
}
