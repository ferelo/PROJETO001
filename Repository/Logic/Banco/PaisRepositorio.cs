using Model.Logic.Contexto;
using Model.Logic.Model;
using Repository.Logic.Infra;
using System.Linq;

namespace Repository.Logic.Banco
{
    public class PaisRepositorio : GenericRepository<FernandoEntities, pais>
    {
        public pais ObterPorID(long id)
        {
            return ObterPorObjeto(b => b.id_pais == id).SingleOrDefault();
        }
    }
}