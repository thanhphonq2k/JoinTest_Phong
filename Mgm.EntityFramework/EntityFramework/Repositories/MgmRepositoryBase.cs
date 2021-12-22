using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using Mgm.EntityFramework.MgmSys;

namespace Mgm.EntityFramework.Repositories
{
    public abstract class MgmRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<MgmSysDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected MgmRepositoryBase(IDbContextProvider<MgmSysDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class MgmRepositoryBase<TEntity> : MgmRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected MgmRepositoryBase(IDbContextProvider<MgmSysDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
