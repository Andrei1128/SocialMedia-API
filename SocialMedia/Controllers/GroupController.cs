using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using SocialMedia.Models.DTOs;
using SocialMedia.Repository.IRepository;
using System.Net;

namespace SocialMedia.Controllers
{
    [Route("group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IGroupRepository _dbGroup;
        private readonly IMapper _mapper;
        public GroupController(IGroupRepository dbGroup, IMapper mapper)
        {
            this._response = new APIResponse();
            _mapper = mapper;
            _dbGroup = dbGroup;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetGroups()
        {
            try
            {
                IEnumerable<Group> GroupList = await _dbGroup.GetAllAsync(includeProprieties: "Posts");
                _response.Result = _mapper.Map<List<GroupDTO>>(GroupList);
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

        [HttpGet("{id:int}", Name = "GetGroup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<APIResponse>> GetGroup(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var group = await _dbGroup.GetAsync(item => item.Id == id, includeProprieties: "Posts");
                if (group == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<GroupDTO>(group);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateGroup([FromBody] GroupCreatedDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                    return BadRequest(createDTO);
                Group model = _mapper.Map<Group>(createDTO);
                await _dbGroup.CreateAsync(model);
                _response.Result = _mapper.Map<GroupDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetGroup", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // Adauga rol de admin pentru stergere
        public async Task<ActionResult<APIResponse>> DeleteGroup(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var group = await _dbGroup.GetAsync(item => item.Id == id);
                if (group == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _dbGroup.RemoveAsync(group);
                _response.Result = _mapper.Map<GroupDTO>(group);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // Adauga rol de admin pentru modificare
        public async Task<ActionResult<APIResponse>> UpdateGroup(int id, [FromBody] GroupUpdatedDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var group = await _dbGroup.GetAsync(item => item.Id == id);
                if (group == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                if (await _dbGroup.GetAsync(u => u.Id == updateDTO.Id) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Group Id is invalid!");
                    return BadRequest(ModelState);
                }
                Group model = _mapper.Map<Group>(updateDTO);
                await _dbGroup.UpdateAsync(model);
                _response.Result = _mapper.Map<GroupDTO>(group);
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
