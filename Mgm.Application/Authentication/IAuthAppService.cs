using Abp.Application.Services;
using Mgm.Authentication.Dtos;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mgm.Authentication
{
    public interface IAuthAppService : IApplicationService
    {
        [HttpPost]
        Task<LoginOutput> CheckLogin(LoginInput input);
        
        [HttpPost]
        Task<CheckTokenOutput> CheckToken(CheckTokenInput input);
    }
}
