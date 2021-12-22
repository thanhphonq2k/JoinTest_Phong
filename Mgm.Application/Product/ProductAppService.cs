using Abp.Domain.Repositories;
using Abp.EntityFramework;
using Abp.UI;
using Mgm.EntityFramework.MgmSys;
using Mgm.Product.Dtos;
using Mgm.Test;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgm.Product
{
    public class ProductAppService : MgmAppServiceBase, IProductAppService
    {
        private readonly IRepository<Products> _productRepository;
        private readonly IDbContextProvider<MgmSysDbContext> _mgmSysDbContext;

        public ProductAppService(IRepository<Products> productRepository, IDbContextProvider<MgmSysDbContext> mgmSysDbContext)
        {
            _productRepository = productRepository;
            _mgmSysDbContext = mgmSysDbContext;
        }

        //Sql
        public List<Products> GetProductSql()
        {
            try
            {
                var sql = "select * from Products";
                var getAll = _mgmSysDbContext.GetDbContext().Database.SqlQuery<Products>(sql).ToList();
                return getAll;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        public int CreateProductSql(CreateProduct input)
        {
            try
            {
                Products products = new Products();
                products.CategoryId = input.CategoryId;
                products.ProductName = input.ProductName;

                var sql = "insert into Products(CategoryId, ProductName) values(@CategoryId, @ProductName)";
                var cateId = new SqlParameter("@CategoryId", products.CategoryId);
                var productName = new SqlParameter("@ProductName", products.ProductName);
                var insert = _mgmSysDbContext.GetDbContext().Database.ExecuteSqlCommand(sql,cateId, productName);
                return insert;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public int EditProductSql(UpdateProduct input)
        {
            try
            {
                var getId = "select * from Products where Id=@id";
                var id = new SqlParameter("@id", input.Id);
                var product = _mgmSysDbContext.GetDbContext().Database.SqlQuery<Products>(getId, id).FirstOrDefault();

                var sql = "update Products set CategoryId=@categoryId,ProductName=@productName where Id=@id ";
                var productId = new SqlParameter("id", product.Id);
                var categoryId = new SqlParameter("categoryId", input.CategoryId);
                var productName = new SqlParameter("productName", input.ProductName);
                var update = _mgmSysDbContext.GetDbContext().Database.ExecuteSqlCommand(sql, categoryId, productName, productId);
                return update;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public bool DeleteProductSql(int id)
        {
            try
            {
                var getId = "select * from Products where Id=@id";
                var productId = new SqlParameter("@id", id);
                var product = _mgmSysDbContext.GetDbContext().Database.SqlQuery<Products>(getId, productId).FirstOrDefault();
                if (product.Id != 0)
                {
                    var sql = "delete Products where Id=@id";
                    var Id = new SqlParameter("id", product.Id);
                    var delete = _mgmSysDbContext.GetDbContext().Database.ExecuteSqlCommand(sql, Id);
                }
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public List<ProductCategoryProduct> GetProductSqlInnerJoin()
        {
            try
            {
                var sql = "select B.CategoryId,A.Name,B.Id,B.ProductName from ProductCategory A inner join Products B on A.Id = B.CategoryId";
                var getAll = _mgmSysDbContext.GetDbContext().Database.SqlQuery<ProductCategoryProduct>(sql).ToList();
                return getAll;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public List<ProductCategoryProduct> GetProductSqlLeftJoin()
        {
            try
            {
                var sql = "select A.Id,A.Name,B.Id,B.ProductName from ProductCategory A left join Products B on A.Id = B.CategoryId";
                var getAll = _mgmSysDbContext.GetDbContext().Database.SqlQuery<ProductCategoryProduct>(sql).ToList();
                return getAll;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        //Entity
        public void CreateProductEntity(CreateProduct input)
        {
            try
            {
                var product = new Products()
                {
                    Id = input.Id,
                    CategoryId = input.CategoryId,
                    ProductName = input.ProductName
                };
                _productRepository.Insert(product);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public void UpdateProductEntity(UpdateProduct input)
        {
            try
            {
                var product = _productRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
                if (product != null)
                {
                    product.CategoryId = input.CategoryId;
                    product.ProductName = input.ProductName;
                    _productRepository.UpdateAsync(product);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        public bool DeleteProductEntity(int id)
        {
            try
            {
                var product = _productRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
                if (product != null)
                {
                    _productRepository.Delete(product);
                }
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public List<Products> GetProductEntity()
        {
            try
            {
                var listProduct = _productRepository.GetAllList().Select(x => new Products
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    ProductName = x.ProductName
                }).ToList();
                return listProduct;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public List<ProductCategoryProduct> GetProductEntityInnerJoin()
        {
            try
            {
                MgmSysDbContext db = new MgmSysDbContext();
                var inner = db.ProductCategories
                    .Join(
                        db.Products,
                        pc => pc.Id,
                        p => p.CategoryId,
                        (pc, p) => new
                        {
                            CategoryId = pc.Id,
                            CategoryName = pc.Name,
                            ProductId = p.Id,
                            p.ProductName
                        }
                    );
                var join = inner.ToList().Select(r => new ProductCategoryProduct
                {
                    Id = r.CategoryId,
                    Name = r.CategoryName,
                    ProductId = r.ProductId,
                    ProductName = r.ProductName
                }).ToList();
                return join;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public List<ProductCategoryProduct> GetProductEntityLeftJoin()
        {
            try
            {
                MgmSysDbContext db = new MgmSysDbContext();
                var left = db.ProductCategories
                    .GroupJoin(
                        db.Products,
                        pc => pc.Id,
                        p => p.CategoryId,
                        (pc, ProductGroup) => new { pc, ProductGroup })
                    .SelectMany(x => x.ProductGroup.DefaultIfEmpty(), (x, p) => new
                    {
                        CategoryId = x.pc.Id,
                        CategoryName = x.pc.Name,
                        ProductId = p == null?0 : p.Id,
                        p.ProductName
                    }
                    //(pc, p) => new
                    //{
                    //    CategoryId = pc.Id,
                    //    CategoryName = pc.Name,
                    //    ProductId = p.Id,
                    //    p.ProductName
                    //}
                    );
                var join = left.ToList().Select(r => new ProductCategoryProduct
                {
                    Id = r.CategoryId,
                    Name = r.CategoryName,
                    ProductId = r.ProductId,
                    ProductName = r.ProductName
                }).ToList();
                return join;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        //Linq
        public List<Products> GetProductLinq()
        {
            try
            {
                MgmSysDbContext db = new MgmSysDbContext();
                var result = from p in db.Products
                             select new
                             {
                                 p.Id,
                                 p.CategoryId,
                                 p.ProductName
                             };
                //var product = (from c in _mgmSysDbContext.GetDbContext().Products
                //               select new Products
                //                       {
                //                           Id = c.Id,
                //                           CategoryId = c.CategoryId,
                //                           ProductName = c.ProductName
                //                       })
                //                .ToList();
                var Product = result.ToList().Select(r => new Products
                {
                    Id = r.Id,
                    CategoryId = r.CategoryId,
                    ProductName = r.ProductName
                }).ToList();
                return Product;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public void CreateProductLinq(CreateProduct input)
        {
            try
            {
                Products product = new Products
                {
                    Id = input.Id,
                    CategoryId = input.CategoryId,
                    ProductName = input.ProductName
                };
                //Products product = new Products();
                //product.Id = input.Id;
                //product.CategoryId = input.CategoryId;
                //product.ProductName = input.ProductName;

                var Products = _mgmSysDbContext.GetDbContext().Products.Add(product);

                _productRepository.InsertAsync(Products);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public void UpdateProductLinq(UpdateProduct input)
        {
            try
            {
                var product = (from c in _mgmSysDbContext.GetDbContext().Products
                                       where c.Id == input.Id
                                       select c).FirstOrDefault();
                if (product.Id != 0)
                {
                    product.CategoryId = input.CategoryId;
                    product.ProductName = input.ProductName;
                    _productRepository.Update(product);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public bool DeleteProductLinq(int id)
        {
            try
            {
                var product = (from c in _mgmSysDbContext.GetDbContext().Products
                                       where c.Id == id
                                       select c).FirstOrDefault();
                if (product.Id != 0)
                {
                    _productRepository.Delete(product);

                    //_repository
                }
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public List<ProductCategoryProduct> GetProductLinqInnerJoin()
        {
            try
            {
                MgmSysDbContext db = new MgmSysDbContext();
                var result = from p in db.Products
                             join e in db.ProductCategories
                             on p.CategoryId equals e.Id
                             select new
                             {
                                 CategoryId = e.Id,
                                 CategoryName = e.Name,
                                 ProductId = p.Id,
                                 p.ProductName
                             };
                var innerJoin = result.ToList().Select(r => new ProductCategoryProduct
                {
                    Id = r.CategoryId,
                    Name = r.CategoryName,
                    ProductId = r.ProductId,
                    ProductName = r.ProductName
                }).ToList();
                return innerJoin;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public List<ProductCategoryProduct> GetProductLinqLeftJoin()
        {
            try
            {
                MgmSysDbContext db = new MgmSysDbContext();
                var result = from p in db.ProductCategories
                             join e in db.Products
                             on p.Id equals e.CategoryId into dept
                             from department in dept.DefaultIfEmpty()
                             select new
                             {
                                 CategoryId = p.Id,
                                 CategoryName = p.Name,
                                 ProductId = department == null?0 : department.Id,
                                 department.ProductName,
                             };
                var leftJoin = result.ToList().Select(r => new ProductCategoryProduct
                {
                    Id = r.CategoryId,
                    Name = r.CategoryName,
                    ProductId = r.ProductId,
                    ProductName = r.ProductName
                }).ToList();
                return leftJoin;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
