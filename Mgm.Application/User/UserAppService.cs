using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Mgm.MgmSys.MgmUser;
using Mgm.MgmSys.MgmUserGroup;
using Mgm.User.Dtos;
using Mgm.Utility;
using Mgm.Utility.Dtos;

namespace Mgm.User
{
    public class UserAppService : MgmAppServiceBase, IUserAppService
    {
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        private readonly IRepository<MgmUsers> _mgmUsersRepository;
        private readonly IRepository<MgmUserGroup> _mgmUsersGroup;

        public UserAppService(
            IRepository<MgmUsers> mgmUsersRepository,
            IRepository<MgmUserGroup> mgmUsersGroup)
        {
            _mgmUsersRepository = mgmUsersRepository;
            _mgmUsersGroup = mgmUsersGroup;
        }

        public PageResultDto<MgmUsersGroup> GetUserList(FilterInput input)
        {
            try
            {
                PageResultDto<MgmUsersGroup> objResult = new PageResultDto<MgmUsersGroup>();
                objResult.items = _mgmUsersRepository.GetAll()
                    .Join(_mgmUsersGroup.GetAll(), t1 => t1.UserGroupId, t2 => t2.Id,
                    (t1, t2) => new
                    {
                        t1.Id,
                        t1.Username,
                        t1.FirstName,
                        t1.LastName,
                        t1.Email,
                        t1.IsActive,
                        t1.CreatedDate,
                        t1.UpdatedDate,
                        t2.GroupName,
                        t1.UserGroupId
                    })
                    .Where(x => x.IsActive == 1)
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                        obj => obj.FirstName.Contains(input.Keyword) ||
                        obj.LastName.Contains(input.Keyword) ||
                        obj.Username.Contains(input.Keyword) ||
                        obj.Email.Contains(input.Keyword) ||
                        obj.GroupName.Contains(input.Keyword))
                    .Select(x => new MgmUsersGroup()
                    {
                        Id = x.Id,
                        Username = x.Username,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        IsActive = x.IsActive,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.UpdatedDate,
                        UserGroupId = x.UserGroupId,
                        GroupName = x.GroupName
                    })
                    .Skip((input.SkipCount - 1) * input.MaxResultCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                objResult.totalCount = _mgmUsersRepository.GetAll()
                    .Join(_mgmUsersGroup.GetAll(), t1 => t1.UserGroupId, t2 => t2.Id,
                    (t1, t2) => new
                    {
                        t1.FirstName,
                        t1.LastName,
                        t1.Username,
                        t1.Email,
                        t2.GroupName,
                        t1.UserGroupId
                    })
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                        obj => obj.FirstName.Contains(input.Keyword) ||
                        obj.LastName.Contains(input.Keyword) ||
                        obj.Username.Contains(input.Keyword) ||
                        obj.Email.Contains(input.Keyword) ||
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

        public MgmUsers GetUserDetail(int id)
        {
            try
            {
                return _mgmUsersRepository.GetAll()
                    .Where(x => x.Id == id)
                    .ToList()
                    .Select(x => new MgmUsers()
                    {
                        Id = x.Id,
                        Username = x.Username,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        IsActive = x.IsActive,
                        UserGroupId = x.UserGroupId,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.UpdatedDate
                    })
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public async Task<ResultDto> UpdateUser(UpdateUserInput input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input.Password) && !new Regex(PasswordRegex).IsMatch(input.Password))
                {
                    throw new UserFriendlyException(400, L("InvalidPasswordFormat"));
                }

                if (!string.IsNullOrEmpty(input.Password) && !input.Password.Equals(input.ConfirmPassword))
                {
                    throw new UserFriendlyException(400, L("TwoPasswordsThatYouEnterIsInconsistent"));
                }

                var user = await _mgmUsersRepository.GetAll()
                    .Where(x => x.Id == input.Id)
                    .FirstOrDefaultAsync();

                var CheckEmail = _mgmUsersRepository.GetAllList()
                    .Where(x => !x.Id.Equals(input.Id) && x.Email.Equals(input.Email)).FirstOrDefault();

                if (user != null)
                {
                    if (CheckEmail == null)
                    {
                        user.Email = input.Email;
                    }
                    else
                    {
                        throw new UserFriendlyException(404, L("TheEmailInputAlreadyExist"));
                    }
                    user.FirstName = input.FirstName;
                    user.LastName = input.LastName;
                    user.IsActive = input.IsActive;
                    user.UserGroupId = input.UserGroupId;
                    user.UpdatedDate = DateTime.UtcNow;

                    await _mgmUsersRepository.UpdateAsync(user);
                }
                else
                {
                    throw new UserFriendlyException(400, L("UserNotPound"));
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

        public async Task<ResultDto> CreateUser(CreateUserInput input)
        {
            try
            {
                if (!new Regex(PasswordRegex).IsMatch(input.Password))
                {
                    throw new UserFriendlyException(400, L("InvalidPasswordFormat"));
                }

                if (!input.Password.Equals(input.ConfirmPassword))
                {
                    throw new UserFriendlyException(400, L("TwoPasswordsThatYouEnterIsInconsistent"));
                }

                var chckUser = await _mgmUsersRepository.GetAll()
                    .Where(x => x.Username.Equals(input.Username) || x.Email.Equals(input.Email))
                    .ToListAsync();

                if (chckUser.Count == 0)
                {
                    Utils utils = new Utils();
                    await _mgmUsersRepository.InsertAsync(new MgmUsers()
                    {
                        Username = input.Username,
                        FirstName = input.FirstName,
                        LastName = input.LastName,
                        Email = input.Email,
                        Password = utils.MD5Hash(input.Password),
                        IsActive = input.IsActive,
                        UserGroupId = input.UserGroupId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow

                    });
                }
                else
                {
                    throw new UserFriendlyException(400, L("UserOrEmailExisted"));
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

        public async Task<ResultDto> DeleteUser(int id)
        {
            try
            {
                var user = await _mgmUsersRepository.GetAll()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    await _mgmUsersRepository.DeleteAsync(user);
                }
                else
                {
                    throw new UserFriendlyException(400, L("UserNotPound"));
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

        public List<UserGroupIds> GetUserGroupId(string filter, string screen)
        {
            try
            {
                if (string.IsNullOrEmpty(screen))
                {
                    var rs = _mgmUsersGroup.GetAll()
                        .Select(x => new UserGroupIds()
                        {
                            GroupName = x.GroupName,
                            UserGroupId = x.Id
                        })
                        .ToList();

                    return rs;
                }
                else
                {
                    var rs = _mgmUsersGroup.GetAll()
                        .Select(x => new UserGroupIds()
                        {
                            GroupName = x.GroupName,
                            UserGroupId = x.Id
                        })
                        .ToList();

                    return rs;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
    }
}
