using DataBase.Data;
using DataBase.Helper;
using DataBase.Models;
using DataBase.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repository.Interfaces;
namespace DataAccess.Repository.Implementation
{
    public class Authorization : IAuthorization
    {
        private readonly UserManager<PharmcyInfo> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Authorization(UserManager<PharmcyInfo> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Roles ="Admin")]
        public async Task<AuthModel> AddPharmcyAsync(AddPharmcyModel model)
        {
            //Make Sure That Pharmcy doesnt exist in the Db Based On The AccountNum
           if(_userManager.Users.Any(x => x.AccountNum == model.AccountNum))
                return new AuthModel { Message = "Pharmcy is Alaredy Founded !" };

            //Take the Value From The Model, And Store it in PharmcyInfo obj
            var user = new PharmcyInfo
            {
                 AccountNum = model.AccountNum,  
                  UserName = model.PharmcyName,
            };
            //create it in the Db
            var result = await _userManager.CreateAsync(user, model.Password);
            //if it Failed to added it , return errors in AuthModel in Message
            if (!result.Succeeded)
            {
                var errors = String.Empty;

                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthModel { Message = errors };
            }
            //assign rule to User
            await _userManager.AddToRoleAsync(user, Rules.User.ToString());

            //return AuthModel

            return new AuthModel
            {
                 IsAuthntecated = true,
                  Rules = (List<string>)await _userManager.GetRolesAsync(user)
            };
        }

        //fun get the JwtSecurityToken var then we convert it in the above method to get the token 
        private async Task<JwtSecurityToken> CreateJwtToken(PharmcyInfo user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                /*new Claim(JwtRegisteredClaimNames.Email, user.Email)*/,
                new Claim("uid", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<AuthModel> Login(LoginPharmcy loginpharmcy)
        {
            var authmodel = new AuthModel();

            //Make Sure That AccountNum , Password is Match
            var user =  _userManager.Users.Where(x => x.AccountNum == loginpharmcy.AccountNum).FirstOrDefault();
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginpharmcy.Password))
            {
                authmodel.Message = "User Not Found or Password Not Match !";
                return authmodel;
            }

            //Now Everything Ok , you can return type
            var jwtSecurityToken = await CreateJwtToken(user);
            authmodel.IsAuthntecated = true;
            authmodel.Rules = (List<string>)await _userManager.GetRolesAsync(user);
            authmodel.ExpirseOn = jwtSecurityToken.ValidTo;
            authmodel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return authmodel;

        }

        public async Task<PharmcyInfoViewModel> Profile()
        {
            //Get Login person data 
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userId);
            return new PharmcyInfoViewModel
            {
                AccountNum = user.AccountNum,
                Id = user.Id,
                PharmcyName = user.UserName,
                latitude = user.latitude,
                longitude = user.longitude

            };
        }


    }
}
