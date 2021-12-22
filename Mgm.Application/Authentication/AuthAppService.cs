using Abp.Domain.Repositories;
using Abp.UI;
using Mgm.Authentication.Dtos;
using Mgm.MgmSys.MgmUser;
using Mgm.Utility;
using Mgm.Utility.Authentication;
using Mgm.Utility.Authentication.Dtos;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mgm.Authentication
{
    public class AuthAppService : MgmAppServiceBase, IAuthAppService
    {
        private readonly IRepository<MgmUsers> _mgmUserRepository;

        public AuthAppService(
            IRepository<MgmUsers> mgmUserRepository)
        {
            _mgmUserRepository = mgmUserRepository;
        }

        public async Task<LoginOutput> CheckLogin(LoginInput input)
        {
            try
            {
                AuthenticationService authenticationService = new AuthenticationService();

                var user = await _mgmUserRepository.GetAll()
                    .Where(x => x.Username.Equals(input.Username))
                    .FirstOrDefaultAsync();
                
                Utils utils = new Utils();
                if (user == null || !user.Password.Equals(utils.MD5Hash(input.Password)))
                {
                    throw new UserFriendlyException(400, L("TheUsernameOrPasswordInvalid"));
                }

                if (user.IsActive != 1)
                {
                    throw new UserFriendlyException(400, L("YourAccountHasBeenTemporarilyLocked"));
                }

                LoginOutput result = new LoginOutput();
                result.UserId = user.Id;
                result.RoleList = ",";

                /*
                var roles = await _mgmUserGroupRightRepository.GetAll()
                    .Where(x => x.UserGroupId == user.UserGroupId)
                    .ToListAsync();
                foreach(var item in roles)
                {
                    if (item.ViewRight == 1)
                    {
                        result.RoleList += item.FunctionCode + "_VIEW,";
                    }
                    if (item.UpdateRight == 1)
                    {
                        result.RoleList += item.FunctionCode + "_UPDATE,";
                    }
                }
                */

                IAuthContainerModel model = authenticationService.GetJWTContainerModel(user.Username, user.Email);
                result.AccessToken = authenticationService.GenerateToken(model);
                
                return result;
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public async Task<CheckTokenOutput> CheckToken(CheckTokenInput input)
        {
            try
            {
                CheckTokenOutput result = new CheckTokenOutput();

                AuthenticationService authenticationService = new AuthenticationService();
                if (!authenticationService.IsTokenValid(input.Token))
                {
                    throw new UserFriendlyException(400, L("TheTokenInvalid"));
                }
                else
                {
                    var tokenClaims = authenticationService.GetTokenClaims(input.Token).ToList();
                    var username = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;
                    var user = await _mgmUserRepository.GetAll()
                        .Where(x => x.Username.Equals(username) && x.IsActive == 1)
                        .FirstOrDefaultAsync();
                    if (user == null)
                    {
                        throw new UserFriendlyException(400, L("TheTokenInvalid"));
                    }

                    result.UserId = user.Id;
                    result.RoleList = ",";
                    /*
                    var roles = await _mgmUserGroupRightRepository.GetAll()
                    .Where(x => x.UserGroupId == user.UserGroupId)
                    .ToListAsync();
                    foreach (var item in roles)
                    {
                        if (item.ViewRight == 1)
                        {
                            result.RoleList += item.FunctionCode + "_VIEW,";
                        }
                        if (item.UpdateRight == 1)
                        {
                            result.RoleList += item.FunctionCode + "_UPDATE,";
                        }
                    }
                    */
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
    }
}
