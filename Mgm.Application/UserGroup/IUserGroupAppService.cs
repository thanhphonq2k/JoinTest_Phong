using Abp.Application.Services;
using Mgm.Utility.Dtos;
using System.Web.Http;
using Mgm.UserGroup.Dtos;
using System.Threading.Tasks;
using Mgm.MgmSys.MgmUserGroup;

namespace Mgm.UserGroup
{
    public interface IUserGroupAppService : IApplicationService
    {
        [HttpPost, Authorize(Roles = "USERS_GROUP_SCREEN_VIEW")]
        PageResultDto<MgmUserGroup> GetUserGroupList(FilterInput input);

        [HttpGet, Authorize(Roles = "USERS_GROUP_SCREEN_VIEW")]
        UserGroupOutput GetUserGroupDetail(int id);

        [HttpPost, Authorize(Roles = "USERS_GROUP_SCREEN_UPDATE")]
        Task<ResultDto> UpdateUserGroup(UpdateUserGroupInput input);

        [HttpPost, Authorize(Roles = "USERS_GROUP_SCREEN_UPDATE")]
        Task<ResultDto> CreateUserGroup(CreateUserGroupInput input);

        [HttpGet, Authorize(Roles = "USERS_GROUP_SCREEN_UPDATE")]
        Task<ResultDto> DeleteUserGroup(int id);

    }
}
