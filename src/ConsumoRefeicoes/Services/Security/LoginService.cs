using System;
using System.Threading.Tasks;
using backend.Interfaces.Services.Security;
using backend.Models;
using Microsoft.Extensions.Options;

namespace backend.Services.Security
{
    public class LoginService : ILogin
    {
        private readonly IAuthentication _ldapAuthService;
        private readonly ITokenService _tokenService;
        private readonly LdapConfig _ldapConfig;

        public LoginService(IAuthentication ldapAuthService, IOptions<LdapConfig> ldapConfig, ITokenService tokenService)
        {
            _ldapAuthService = ldapAuthService;
            _ldapConfig = ldapConfig.Value;
            _tokenService = tokenService;
        }

        public async Task<ResponseLogin> Authenticate(Login login)
        {


            try
            {
                User user = await _ldapAuthService.Autenthicate(login.Username, login.Password);

                // _ldapAuthService.BelongToGroup(_ldapConfig.MemberOf);


                string token = _tokenService.GenerateToken(user);
                ResponseLogin response = new ResponseLogin
                {
                    User = user,
                    Token = token
                };

                return response;

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }



        }
    }
}