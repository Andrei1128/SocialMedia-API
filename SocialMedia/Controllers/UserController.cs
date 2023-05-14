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
        public async Task<ActionResult<APIResponse>> GetFeed([FromBody] int pageSize = 24, int pageNumber = 1)
        {
            try
            {
                int myId = await GetMyId();
                var feed = await _dbUser.GetFeed(myId, pageSize: pageSize, pageNumber: pageNumber);
                _response.Result = feed;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
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
                _response.ErrorMessages = new List<string>() { ex.ToString() };
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
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                User userFound = await _dbUser.GetAsync(u => u.Id == id, includeProprieties: "Requests");
                if (userFound == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User not found!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                int myId = await GetMyId();
                bool friendRequestExists = userFound.Requests.Any(r => r.UserId == id && r.RequestedUserId == myId);
                if (friendRequestExists)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Friend request already exists.");
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
                _response.ErrorMessages = new List<string>() { ex.ToString() };
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
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                int myId = await GetMyId();
                User myUser = await _dbUser.GetAsync(u => u.Id == myId, includeProprieties: "Requests,Friends");

                UserRequest request = myUser.Requests.FirstOrDefault(r => r.UserId == myId);
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

                UserFriend userFriend = new UserFriend()
                {
                    UserId = myId,
                    FriendId = friend.Id,
                };

                if (myUser.Friends == null)
                    myUser.Friends = new List<UserFriend>();
                myUser.Friends.Add(userFriend);

                if (friend.Friends == null)
                    friend.Friends = new List<UserFriend>();
                friend.Friends.Add(userFriend);

                await _dbUser.SaveAsync();


                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
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
                User myUser = await _dbUser.GetAsync(u => u.Id == myId);

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
                _response.ErrorMessages = new List<string>() { ex.ToString() };
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
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                int myId = await GetMyId();
                User myUser = await _dbUser.GetAsync(u => u.Id == myId);

                UserFriend friend = myUser.Friends.FirstOrDefault(f => f.FriendId == id);
                if (friend == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Friend not found!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                myUser.Friends.Remove(friend);
                await _dbUser.SaveAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
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

                Group group = await _dbGroup.GetAsync(g => g.Id == id);

                if (group == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                int myId = await GetMyId();
                User myUser = await _dbUser.GetAsync(u => u.Id == myId);

                myUser.Groups.Add(group);
                await _dbUser.SaveAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
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
