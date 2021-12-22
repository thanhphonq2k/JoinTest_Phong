using Abp.Application.Services;
using System.Web.Http;
using Mgm.User.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mgm.Utility.Dtos;
using Mgm.MgmSys.MgmUser;

namespace Mgm.User
{
    public interface IUserAppService : IApplicationService
    {
        [HttpPost, Authorize(Roles = "USERS_SCREEN_VIEW")]
        PageResultDto<MgmUsersGroup> GetUserList(FilterInput input);

        [HttpGet, Authorize(Roles = "USERS_SCREEN_VIEW")]
        MgmUsers GetUserDetail(int id);

        [HttpPost, Authorize(Roles = "USERS_SCREEN_UPDATE")]
        Task<ResultDto> UpdateUser(UpdateUserInput input);

        [HttpPost, Authorize(Roles = "USERS_SCREEN_UPDATE")]
        Task<ResultDto> CreateUser(CreateUserInput input);

        [HttpGet, Authorize(Roles = "USERS_SCREEN_UPDATE")]
        Task<ResultDto> DeleteUser(int id);

        [HttpGet, Authorize(Roles = "USERS_SCREEN_VIEW, USERS_GROUP_SCREEN_VIEW")]
        List<UserGroupIds> GetUserGroupId(string filter, string screen);
    }
}
