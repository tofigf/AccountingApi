using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos;
using AccountingApi.Dtos.User;
using AccountingApi.Models;
using AutoMapper;
using EOfficeAPI.Dtos;
using EOfficeAPI.Dtos.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AccountingApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
        }
        //Post [baseUrl]/api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto registerDto)
        {
            //Check
            #region Check
            if (await _repo.CheckUsersMail(registerDto.Email))
                return StatusCode(409, "This email already exist");
            registerDto.Email = registerDto.Email.ToLower();

            var userToCreate = _mapper.Map<User>(registerDto);

            if (!ModelState.IsValid)
                return BadRequest();
            #endregion
            //Register
            var createdUser = await _repo.Register(userToCreate, registerDto.Password);
            //User to return
            var userToReturn = _mapper.Map<UserForReturnDto>(createdUser);
            //Company Dto
            //CompanyPostDto companyPost = new CompanyPostDto
            //{
            //    Name = registerDto.Name,
            //    Surname = registerDto.SurName
            //};
            //return true false for companyCreating page showing
            var companyCount = _repo.CompanyCountForRegister(createdUser.Id);

            //Company For Create
            //Company company = _mapper.Map<Company>(companyPost);
            ////created company
            //var createCompany = _userRepo.CreateCompany(company, createdUser.Id);
            ////didnt use
            //var userCompany = await _repo.UserCompany(createdUser.Id);
            //// return company
            //var companyToReturn = _mapper.Map<CompanyGetDto>(createCompany);

            //GWT 
            #region GWT
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, createdUser.Id.ToString()),
                new Claim(ClaimTypes.Name, createdUser.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),

                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            #endregion

            return Ok(new
            {
                userToReturn,
                token = tokenHandler.WriteToken(token),
                companyCount.Result

            });
        }
        //Post [baseUrl]/api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            if (await _repo.UserExists(userForLoginDto.Email, userForLoginDto.Password))
                return StatusCode(401, "email or password in correct");
            if (userForLoginDto == null)
                return Unauthorized();

            User userFromRepo = await _repo.Login(userForLoginDto.Email.ToLower(), userForLoginDto.Password);
            //return true false for companyCreating page showing
            var companyCount = _repo.CompanyCountForRegister(userFromRepo.Id);
            //qaytardigimiz istifadeci melumatlari
            var user = _mapper.Map<UserForReturnDto>(userFromRepo);
            //Istifadecinin sriketlerini  tapmaq
            var userCompanies = await _repo.UserCompanies(user.Id);

            //GWT 
            #region GWT
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),

                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            #endregion

            // sirketleri qaytarmaq
            var companyToReturn = _mapper.Map<IEnumerable<CompanyGetDto>>(userCompanies);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user,
                companyToReturn,
                companyCount.Result
            });

        }
    }
}
