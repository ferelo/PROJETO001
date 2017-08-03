using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.Logic.Infra
{
    public abstract class GenericRepository<C, T> : IRepository<T>
        where T : class
        where C : DbContext, new()
    {
        private C _context = new C();
        private bool disposed;

        protected GenericRepository()
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public C Contexto
        {
            get { return _context; }
            set { _context = value; }
        }

        public virtual void Criar(T instancia, bool salvar = true)
        {
            _context.Set<T>().Add(instancia);
            if (salvar)
                Salvar();
        }

        public virtual void Excluir(T instancia)
        {
            var entry = _context.Entry(instancia);
            if (entry.State == EntityState.Detached)
                _context.Set<T>().Attach(instancia);
            _context.Set<T>().Remove(instancia);
        }

        public virtual void Editar(T instancia, bool salvar = true)
        {
            _context.Entry(instancia).State = EntityState.Modified;
            if (salvar)
                Salvar();
        }
        
        public virtual void Salvar()
        {
            ((IObjectContextAdapter)_context).ObjectContext.DetectChanges();
            _context.SaveChanges();
        }

        public virtual IQueryable<T> ObterTodos()
        {
            return _context.Set<T>();
        }

        public virtual IQueryable<T> ObterPorObjeto(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public bool Any()
        {
            return _context.Set<T>().Any();
        }

        public void Dispose()
        {
            Dispose(true);
            _context = null;
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void ModificarContexto(C novo)
        {
            Contexto = novo;
        }

        public virtual IQueryable<T> ExecutarSQL(string query, params object[] parameters)
        {
            return _context.Set<T>().SqlQuery(query).AsQueryable();
        }
    }
}