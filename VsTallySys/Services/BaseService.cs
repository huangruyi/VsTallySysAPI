using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VsTallySys.Models;

namespace VsTallySys.Services
{
    public class BaseService<T> where T : class
    {
        private DbContext context = new VS_TALLY_DBContext();

        internal DbContext Context
        {
            get { return context; }
            set { context = value; }
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T QueryByID(object id)
        {
            T entity = context.Set<T>().Find(id);
            return entity;
        }
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public virtual List<T> Query()
        {
            List<T> entitys = context.Set<T>().ToList();
            return entitys;
        }

        /// <summary>
        /// 查询所有数据数量
        /// </summary>
        /// <returns></returns>
        public virtual int Count()
        {
            return context.Set<T>().Count();
        }

        /// <summary>
        /// 条件查询一条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual T QuerySingle(Expression<Func<T, bool>> where)
        {
            T entity = context.Set<T>().Where(where).ToList().FirstOrDefault();
            return entity;
        }

        /// <summary>
        /// 条件查询多条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual List<T> Query(Expression<Func<T, bool>> where)
        {
            List<T> entitys = context.Set<T>().Where(where).ToList();
            return entitys;
        }

        /// <summary>
        /// 条件查询数据数量
        /// </summary>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return context.Set<T>().Where(where).Count();
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public virtual int Add(params T[] entitys)
        {
            try
            {
                entitys.ToList().ForEach(entity =>
                {
                    context.Entry(entity).State = EntityState.Added;
                });
                return context.SaveChanges();
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        /// <summary>
        /// 添加数据并返回错误信息
        /// </summary>
        /// <param name="error"></param>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public virtual int TryAdd(out string error, params T[] entitys)
        {
            try
            {
                entitys.ToList().ForEach(entity =>
                {
                    context.Entry(entity).State = EntityState.Added;
                });
                error = "";
                return context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.InnerException.ToString();
                return 0;
            }
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public virtual int Update(params T[] entitys)
        {
            try
            {
                entitys.ToList().ForEach(entity =>
                {
                    context.Entry<T>(entity).State = EntityState.Modified;
                });
                return context.SaveChanges();
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        /// <summary>
        /// 更新数据并返回错误信息
        /// </summary>
        /// <param name="error"></param>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public virtual int TryUpdate(out string error, params T[] entitys)
        {
            try
            {
                entitys.ToList().ForEach(entity =>
                {
                    context.Entry<T>(entity).State = EntityState.Modified;
                });
                error = "";
                return context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.InnerException.ToString();
                return 0;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public virtual int Delete(params T[] entitys)
        {
            try
            {
                entitys.ToList().ForEach(entity =>
                {
                    context.Entry<T>(entity).State = EntityState.Deleted;
                });
                return context.SaveChanges();
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        /// <summary>
        /// 删除数据并返回错误信息
        /// </summary>
        /// <param name="error"></param>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public virtual int TryDelete(out string error, params T[] entitys)
        {
            try
            {
                entitys.ToList().ForEach(entity =>
                {
                    context.Entry<T>(entity).State = EntityState.Deleted;
                });
                error = "";
                return context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.InnerException.ToString();
                return 0;
            }
        }
    }

}
