using Abp.Application.Services;

namespace Mgm
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class MgmAppServiceBase : ApplicationService
    {
        protected MgmAppServiceBase()
        {
            LocalizationSourceName = MgmConsts.LocalizationSourceName;
        }
    }
}