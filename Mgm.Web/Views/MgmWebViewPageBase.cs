using Abp.Web.Mvc.Views;

namespace Mgm.Web.Views
{
    public abstract class MgmWebViewPageBase : MgmWebViewPageBase<dynamic>
    {

    }

    public abstract class MgmWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected MgmWebViewPageBase()
        {
            LocalizationSourceName = MgmConsts.LocalizationSourceName;
        }
    }
}