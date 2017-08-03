using Model.Logic.Contexto;
using Model.Logic.Model;
using Repository.Logic.Infra;
using System;
using System.Linq;

namespace Repository.Logic.Banco
{
    public class CidadeRepositorio : GenericRepository<FernandoEntities, cidade>
    {
        public cidade ObterPorID(long id)
        {
            return ObterPorObjeto(b => b.id_cidade == id).SingleOrDefault();
        }

        public IQueryable<cidade> Listar(cidade instancia)
        {
            IQueryable<cidade> cid;
            var query = "select * from cidade where 1=1 ";
            var tamanhoquery = query.Count();

            if (!String.IsNullOrEmpty(instancia.nome))
                query += " and " + "to_ascii(nome) ILIKE to_ascii('%" + instancia.nome + "%')";

            if (!String.IsNullOrEmpty(instancia.cep))
                query += " and " + "to_ascii(cep) ILIKE to_ascii('%" + instancia.cep + "%')";

            if (instancia.estado > 0)
                query += " and estado =" + instancia.estado;


            cid = query.Count() != tamanhoquery ? ExecutarSQL(query) : ObterTodos();          

            return cid;
        }
    }
}