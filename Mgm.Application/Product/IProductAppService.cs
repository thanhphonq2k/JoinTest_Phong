using Abp.Application.Services;
using Mgm.Product.Dtos;
using Mgm.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgm.Product
{
    public interface IProductAppService : IApplicationService
    {
        int CreateProductSql(CreateProduct input);
        int EditProductSql(UpdateProduct input);
        List<Products> GetProductSql();
        List<ProductCategoryProduct> GetProductSqlInnerJoin();
        List<ProductCategoryProduct> GetProductSqlLeftJoin();
        List<ProductCategoryProduct> GetProductLinqInnerJoin();
        List<ProductCategoryProduct> GetProductLinqLeftJoin();
        List<ProductCategoryProduct> GetProductEntityInnerJoin();
        List<ProductCategoryProduct> GetProductEntityLeftJoin();
        List<Products> GetProductEntity();
        List<Products> GetProductLinq();
        bool DeleteProductSql(int id);
        bool DeleteProductEntity(int id);
        bool DeleteProductLinq(int id);
    }
}
