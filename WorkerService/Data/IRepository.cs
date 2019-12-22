using Data.Models;
using System.Collections.Generic;

namespace Data
{
    public interface IRepository
    {
        T GetById<T>(int id) where T : BaseEntity;
        List<T> GetList<T>() where T : BaseEntity;
        T Add<T>(T entity) where T : BaseEntity;
        void Update<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
    }
}
