using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Mgm.MgmSys.MgmUser;
using Mgm.MgmSys.MgmUserGroup;
using Mgm.UserGroup.Dtos;
using Mgm.Utility;
using Mgm.Utility.Dtos;
using Abp.EntityFramework;
using Mgm.EntityFramework.MgmSys;

namespace Mgm.UserGroup
{
    public class UserGroupAppService : MgmAppServiceBase, IUserGroupAppService
    {
        private readonly IRepository<MgmUsers> _mgmUsersRepository;
        private readonly IRepository<MgmUserGroup> _mgmUserGroupRepository;

        private readonly IDbContextProvider<MgmSysDbContext> _mgmSysDbContext;

        public UserGroupAppService(
            IDbContextProvider<MgmSysDbContext> mgmSysDbContext,

            IRepository<MgmUsers> mgmUsersRepository,
            IRepository<MgmUserGroup> mgmUserGroupRepository)
        {
            _mgmSysDbContext = mgmSysDbContext;

            _mgmUsersRepository = mgmUsersRepository;
            _mgmUserGroupRepository = mgmUserGroupRepository;
        }

        public PageResultDto<MgmUserGroup> GetUserGroupList(FilterInput input)
        {
            try
            {
                PageResultDto<MgmUserGroup> objResult = new PageResultDto<MgmUserGroup>();

                objResult.items = _mgmUserGroupRepository.GetAll()
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                        obj => obj.GroupCode.Contains(input.Keyword) ||
                        obj.GroupName.Contains(input.Keyword))
                    .Select(x => new MgmUserGroup()
                    {
                        Id = x.Id,
                        GroupCode = x.GroupCode,
                        GroupName = x.GroupName,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.UpdatedDate

                    })
                    .Skip((input.SkipCount - 1) * input.MaxResultCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                objResult.totalCount = _mgmUserGroupRepository.GetAll()
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                        obj => obj.GroupCode.Contains(input.Keyword) ||
                        obj.GroupName.Contains(input.Keyword))
                    .Count();

                return objResult;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public UserGroupOutput GetUserGroupDetail(int id)
        {
            try
            {
                UserGroupOutput objResult = new UserGroupOutput();

                if (id != 0)
                {
                    /*
                    var userGroupRight = _mgmUserGroupRightRepository.GetAll()
                        .Where(x => x.UserGroupId == id)
                        .Select(x => x.FunctionCode).ToArray();

                    var userFunction = _mgmUserFunctionRepository.GetAll()
                       .Select(x => new RightInsertListOutput()
                       {
                           UserGroupId = id,
                           FunctionCode = x.FunctionCode,
                           FunctionName = x.FunctionName,
                           ViewRight = (userGroupRight.Contains(x.FunctionCode) ? 1 : 0),
                           UpdateRight = (x.HasUpdate == 1 ? (userGroupRight.Contains(x.FunctionCode) ? 1 : 0) : -1)
                       }).ToList();
                    */

                    var userGroup = _mgmUserGroupRepository.GetAll()
                        .Where(x => x.Id == id)
                        .Select(x => new UserGroupOutput()
                        {
                            GroupCode = x.GroupCode,
                            GroupName = x.GroupName,
                            Description = x.Description,
                            IsActive = x.IsActive
                        })
                        .FirstOrDefault();

                    objResult.GroupCode = userGroup.GroupCode;
                    objResult.GroupName = userGroup.GroupName;
                    objResult.Description = userGroup.Description;
                    objResult.IsActive = userGroup.IsActive;
                    //objResult.RightInsertList = userFunction;
                }
                else
                {
                    /*
                    var userFunction = _mgmUserFunctionRepository.GetAll()
                       .Select(x => new RightInsertListOutput()
                       {
                           UserGroupId = id,
                           FunctionCode = x.FunctionCode,
                           FunctionName = x.FunctionName,
                           ViewRight = 0,
                           UpdateRight = (x.HasUpdate == 1 ? 0 : -1)
                       }).ToList();
                    */

                    objResult.GroupCode = null;
                    objResult.GroupName = null;
                    objResult.Description = null;
                    objResult.IsActive = 1;
                    //objResult.RightInsertList = userFunction;
                }
                
                return objResult;

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public async Task<ResultDto> UpdateUserGroup(UpdateUserGroupInput input)
        {
            try
            {
                var userGroup = _mgmUserGroupRepository.GetAll()
                    .Where(x => x.Id == input.Id)
                    .FirstOrDefault();
                
                if (userGroup != null)
                {
                    userGroup.GroupName = input.GroupName;
                    userGroup.Description = input.Description;
                    userGroup.IsActive = input.IsActive;
                    userGroup.UpdatedDate = DateTime.UtcNow;

                    await _mgmUserGroupRepository.UpdateAsync(userGroup);
                    /*
                    var userGroupRight = _mgmUserGroupRightRepository.GetAll()
                    .Where(x => x.UserGroupId == input.Id)
                    .ToList();

                    if (userGroupRight.Count != 0)
                    {
                        for (int i = 0; i < userGroupRight.Count; i++)
                        {
                            await _mgmUserGroupRightRepository.DeleteAsync(userGroupRight[i]);
                        }
                    }
                    
                    for (int i = 0; i < input.RightInsertList.Count; i++)
                    {
                        
                        if (input.RightInsertList[i].ViewRight != 0)
                        {
                            await _mgmUserGroupRightRepository.InsertAsync(new MgmUserGroupRight()
                            {
                                UserGroupId = input.RightInsertList[i].UserGroupId,
                                FunctionCode = input.RightInsertList[i].FunctionCode,
                                ViewRight = input.RightInsertList[i].ViewRight,
                                UpdateRight = input.RightInsertList[i].UpdateRight
                            });
                        }
                    }
                    */
                }
                else
                {
                    throw new UserFriendlyException(400, L("UserGroupNotFound"));
                }

                ResultDto result = new ResultDto();
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public async Task<ResultDto> CreateUserGroup(CreateUserGroupInput input)
        {
            try
            {
                var chckUserGroup = await _mgmUserGroupRepository.GetAll()
                    .Where(x => x.GroupCode.Equals(input.GroupCode))
                    .ToListAsync();

                if (chckUserGroup.Count == 0)
                {
                    Utils utils = new Utils();

                    var ret = await _mgmUserGroupRepository.InsertAndGetIdAsync(new MgmUserGroup()
                    {
                        GroupCode = input.GroupCode,
                        GroupName = input.GroupName,
                        Description = input.Description,
                        IsActive = input.IsActive,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    });
                    /*
                    for (int i = 0; i < input.RightInsertList.Count; i++)
                    {
                        var userGroupRight = _mgmUserGroupRightRepository.GetAll()
                        .Where(x => x.UserGroupId == ret)
                        .FirstOrDefault(); 

                        if (input.RightInsertList[i].ViewRight != 0)
                        {
                            await _mgmUserGroupRightRepository.InsertAsync(new MgmUserGroupRight()
                            {
                                UserGroupId = ret,
                                FunctionCode = input.RightInsertList[i].FunctionCode,
                                ViewRight = input.RightInsertList[i].ViewRight,
                                UpdateRight = input.RightInsertList[i].UpdateRight

                            });
                        }
                    }
                    */
                }
                else
                {
                    throw new UserFriendlyException(400, L("UserGroupExisted"));
                }

                ResultDto result = new ResultDto();
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public async Task<ResultDto> DeleteUserGroup(int id)
        {
            try
            {
                var user = await _mgmUsersRepository.GetAll()
                    .Where(x => x.UserGroupId == id)
                    .FirstOrDefaultAsync();

                var userGroup = await _mgmUserGroupRepository.GetAll()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                /*
                var userGroupRight = _mgmUserGroupRightRepository.GetAll()
                    .Where(x => x.UserGroupId == id)
                    .ToList();

                if (userGroup != null && user == null)
                {
                    if (userGroupRight.Count != 0)
                    {
                        for (int i = 0; i < userGroupRight.Count; i++)
                        {
                            await _mgmUserGroupRightRepository.DeleteAsync(userGroupRight[i]);
                        }
                    }

                    await _mgmUserGroupRepository.DeleteAsync(userGroup);
                }
                else
                {
                    throw new UserFriendlyException(400, L("UserGroupNotFoundOrBeingUsed"));
                }
                */
                ResultDto result = new ResultDto();
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
