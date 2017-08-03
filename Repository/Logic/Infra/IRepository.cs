using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.Logic.Infra
{
    public interface IRepository<T> where T : class
    {
        void Criar(T instancia, bool salvar = true);
        void Excluir(T instancia);
        void Editar(T instancia, bool salvar = true);
        void Salvar();
        void Dispose();
        IQueryable<T> ObterTodos();
        IQueryable<T> ObterPorObjeto(Expression<Func<T, bool>> predicate);
        bool Any();
    }
}