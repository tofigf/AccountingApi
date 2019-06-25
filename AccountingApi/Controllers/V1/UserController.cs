using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.User;
using AccountingApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingApi.Controllers.V1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IAccountsPlanRepository _accountsPlanRepo;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repo, IMapper mapper,IAccountsPlanRepository accountsPlanRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _accountsPlanRepo = accountsPlanRepo;
        }
        //Post [baseUrl]/api/user/addcompany
        [HttpPost("addcompany")]
        public async Task<IActionResult> AddCompany(CompanyPostDto companyPost)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Company company = _mapper.Map<Company>(companyPost);

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Company CompanyToReturn = await _repo.CreateCompany(company, userId);

            CompanyGetDto CompanyToGet = _mapper.Map<CompanyGetDto>(CompanyToReturn);

            await _accountsPlanRepo.ImportFromExcel(CompanyToReturn.Id);
            return Ok(CompanyToGet);
        }
        //Get [baseUrl]/api/user/geteditcompany
        [HttpGet]
        [Route("geteditcompany")]
        public async Task<IActionResult> GetEditCompany([FromHeader]int? companyId)
        {
            if (companyId == null)
                return BadRequest("companyId null");

            Company company = await _repo.GetEditCompany(companyId);

            CompanyEditDto userToReturn = _mapper.Map<CompanyEditDto>(company);
            return Ok(userToReturn);
        }
        //Get [baseUrl]/api/user/getcompany
        [HttpGet]
        [Route("getcompany")]
        public async Task<IActionResult> GetCompany()
        {
            int? userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (userId == null)
                return StatusCode(409, "userId null");

            var companies = await _repo.GetCompany(userId);

            return Ok(companies);
        }
        //Get [baseUrl]/api/user/getcompanybyid
        [HttpGet]
        [Route("getcompanybyid")]
        public async Task<IActionResult> GetCompanyById([FromHeader]int? companyId)
        {
            int? userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (userId == null)
                return StatusCode(409, "userId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");

            var companyToReturn = await _repo.GetCompanyById(userId, companyId);
            if (companyToReturn == null)
                return StatusCode(409, "object null");

            // System.Globalization.CultureInfo
            var culture = new System.Globalization.CultureInfo("az");
            var weekday = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
            var weekdayCase = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(weekday.ToLower());

            CompanyGetDto companyToGet = _mapper.Map<CompanyGetDto>(companyToReturn);
            companyToGet.Culture = DateTime.UtcNow.ToString("dd MMMM yyyy", culture);
            companyToGet.Weekday = weekdayCase;

            return Ok(companyToGet);
        }
        //Put [baseUrl]/api/user/editcompany
        [HttpPut]
        [Route("editcompany")]
        public async Task<IActionResult> EditCompany([FromBody]CompanyPutDto companyForeditDto, [FromHeader] int? companyId)
        {
            //Check:
            #region Check
            int? thisuserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (thisuserId == null)
                return StatusCode(409, "thisuserId null");
            #endregion

            if (!ModelState.IsValid)
                return BadRequest();

            //id ye gore sirketi getiririk
            Company companyFromRepo = await _repo.GetEditCompany(companyId);
            //map edirik
            Company companyForUpdate = _mapper.Map(companyForeditDto, companyFromRepo);

            //edit etmek reposu
            Company updatedCompany = await _repo.EditCompany(companyForUpdate);

            //qaytaracagimiz info
            CompanyAfterPutDto companyToReturn = _mapper.Map<CompanyAfterPutDto>(updatedCompany);
            return Ok(companyToReturn);
        }
        //Get [baseUrl]/api/user/getuser
        [HttpGet]
        [Route("getuser")]
        public async Task<IActionResult> GetUser()
        {
            //Check:
            #region Check

            int? thisuserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (thisuserId == null)
                return StatusCode(409, "thisuserId null");
            #endregion

            var user = await _repo.GetEditUser(thisuserId);
            if (user == null)
                return StatusCode(409, "object null");
            var userToReturn = _mapper.Map<UserGetEditDto>(user);

            return Ok(userToReturn);
        }
        //Put [baseUrl]/api/user/edituser
        [HttpPut]
        [Route("edituser")]
        public async Task<IActionResult> EditUser([FromBody]UserPutDto userPutDto)
        {
            //Check
            #region Check
            int? thisuserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string email = User.FindFirst(ClaimTypes.Name).Value;

            if (thisuserId == null)
                return StatusCode(409, "thisuserId null");
            if (_repo.CheckOldPassword(userPutDto.OldPassword, thisuserId))
                return StatusCode(409, "OldPassword not correct or token  Id no correct");
            #endregion

            User userFromRepo = await _repo.GetEditUser(thisuserId);

            //map edirik
            User userForUpdate = _mapper.Map(userPutDto, userFromRepo);
            //if (email != userForUpdate.Email)
            //    return StatusCode(409, "email not correct");

            var editedUser = _repo.EditUser(userForUpdate, userPutDto.OldPassword);

            var userToReturn = _mapper.Map<UserGetEditDto>(userForUpdate);

            return Ok(new
            {
                userToReturn,
                thisuserId,
                email
            }
                );
        }

    }
}