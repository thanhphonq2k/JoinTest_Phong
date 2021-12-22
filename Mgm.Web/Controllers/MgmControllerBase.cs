using Abp.Web.Mvc.Controllers;

namespace Mgm.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class MgmControllerBase : AbpController
    {
        protected MgmControllerBase()
        {
            LocalizationSourceName = MgmConsts.LocalizationSourceName;
        }
    }
}