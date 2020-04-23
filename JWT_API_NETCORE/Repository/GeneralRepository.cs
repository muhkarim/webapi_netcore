using JWT_API_NETCORE.Base;
using JWT_API_NETCORE.Context;
using JWT_API_NETCORE.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_API_NETCORE.Repository
{
    public class GeneralRepository<TEntity, TContext> : IRepository<TEntity>

        where TEntity : class, IEntity
        where TContext : MyContext
    {
        private readonly MyContext _myContext;

        public GeneralRepository(MyContext myContext)
        {
            _myContext = myContext;
        }
        
        public async Task<TEntity> Delete(int Id)
        {

            var entity = await Get(Id);
            if (entity == null)
            {
                return entity;
            }
            entity.DeleteDate = DateTimeOffset.Now;
            entity.IsDelete = true;
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;

            //throw new NotImplementedException();
        }

        public async Task<List<TEntity>> Get()
        {
            return await _myContext.Set<TEntity>().Where(x => x.IsDelete == false).ToListAsync();

            //throw new NotImplementedException();
        }

        public async Task<TEntity> Get(int Id)
        {
            return await _myContext.Set<TEntity>().FindAsync(Id);

            //throw new NotImplementedException();
        }

        public async Task<TEntity> Post(TEntity entity)
        {
            entity.CreateDate = DateTimeOffset.Now;
            entity.IsDelete = false;
            await _myContext.Set<TEntity>().AddAsync(entity);
            await _myContext.SaveChangesAsync();
            return entity;

            //throw new NotImplementedException(); 
        }

        public async Task<TEntity> Put(TEntity entity)
        {
           
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;

        }

        public async Task<TEntity> GetEmail(string email)
        {
            return await _myContext.Set<TEntity>().FindAsync(email);

            //throw new NotImplementedException();
        }
    }
}



