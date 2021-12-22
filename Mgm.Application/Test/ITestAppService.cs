using Abp.Application.Services;
using Mgm.Test.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgm.Test
{
    public interface ITestAppService : IApplicationService
    {
        List<ProductCategory> GetProductCategory();

        List<ProductCategory> GetCategorySql();
        int CreateSql(CreateProductCategory input);
        void CreateProductCategory(CreateProductCategory input);
    }
}
