using Model.Logic.Contexto;
using Model.Logic.Model;
using Repository.Logic.Infra;
using System;
using System.Linq;

namespace Repository.Logic.Banco
{
    public class AlunoRepositorio : GenericRepository<FernandoEntities, aluno>
    {
        public aluno ObterPorID(long id)
        {
            return ObterPorObjeto(b => b.id_aluno == id).SingleOrDefault();
        }

        public IQueryable<aluno> Listar(aluno instancia)
        {
            IQueryable<aluno> alu;
            var query = "select * from aluno where 1=1 ";
            var tamanhoquery = query.Count();

            if (!String.IsNullOrEmpty(instancia.nome))
                query += " and " + "to_ascii(nome) ILIKE to_ascii('%" + instancia.nome + "%')";

            if (!String.IsNullOrEmpty(instancia.cpf))
                query += " and " + "to_ascii(cpf) ILIKE to_ascii('%" + instancia.cpf + "%')";

            if (instancia.data_nascimento > DateTime.MinValue)
                query += " and data_nascimento =" + instancia.data_nascimento;

            if (instancia.data_cadastro > DateTime.MinValue)
                query += " and data_cadastro =" + instancia.data_cadastro;

            alu = query.Count() != tamanhoquery ? ExecutarSQL(query) : ObterTodos();

            alu = alu.Where(b => b.status == 1);

            return alu;
        }
    }
}