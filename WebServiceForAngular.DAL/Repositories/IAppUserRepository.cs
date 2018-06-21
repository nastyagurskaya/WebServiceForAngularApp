using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceForAngular.DAL.Repositories
{
    public interface IAppUserRepository<T> where T : IdentityUser
    {
        IEnumerable<T> GetAll();
        T Get(string id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Insert(T entity);
        void InsertAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
        void SaveChanges();
    }
}
