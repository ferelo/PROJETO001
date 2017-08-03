using Model.Logic.Contexto;
using Model.Logic.Model;
using Repository.Logic.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Logic.Banco
{
    public class UsuarioRepositorio : GenericRepository<FernandoEntities, usuario>
    {
        public usuario ObterPorID(long id)
        {
            return ObterPorObjeto(b => b.id_usuario == id).SingleOrDefault();
        }
    }
}