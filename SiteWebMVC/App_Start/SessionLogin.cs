using Repository.Logic.Banco;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;

namespace SiteWebMVC.App_Start
{
    public class SessionLogin
    {
        public static string CriarSessionParaSistema(string email)
        {
            var repositorioUsuario = new UsuarioRepositorio();

            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            try
            {
                var usuario = repositorioUsuario.ObterPorObjeto(u => u.email.Equals(email.Trim()) && u.status == 1).ToList(); // Ativo (Obs.: Criar uma enumeração )

                if (usuario.Any() && usuario.Count() == 1)
                {
                    var dados = usuario.First();

                    var user_logado = new UsuarioSistema
                    {
                        ID = dados.id_usuario,
                        NOME = dados.nome,

                        EMAIL = dados.email,
                        CPF = dados.cpf,
                        TELEFONE = dados.telefone,
                        IP = ip
                    };

                    HttpContext.Current.Session["UsuarioSistema"] = user_logado;
                }
            }
            finally
            {
                repositorioUsuario.Dispose();
            }
            return ip;
        }

        public static bool EstaLogado()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated && !String.IsNullOrEmpty(UsuarioSistema.User.EMAIL);
        }

        public static IEnumerable<SelectListItem> CriarSelectList(IQueryable<object> iq, string nome_valor,
string nome_label, object selecionado = null, bool adicionaSelecione = true)
        {
            var lista = new List<KeyValuePair<string, string>>();

            foreach (var obj in iq)
            {
                var valor = GetMethod(obj, "get_" + nome_valor);
                var label = GetMethod(obj, "get_" + nome_label);
                var descricao = label.ToString();
                lista.Add(new KeyValuePair<string, string>(valor.ToString(), descricao));
            }

            lista = lista.OrderByDescending(c => c.Value).ToList();

            if (adicionaSelecione)
            {
                if (selecionado == null)
                    lista.Add(new KeyValuePair<string, string>("", "Selecione..."));
                else
                    lista.Add(new KeyValuePair<string, string>("0", "Selecione..."));
            }

            return new SelectList(lista, "Key", "Value", selecionado).Reverse();
        }

        public static object GetMethod(object current, string getterName)
        {
            try
            {
                //Caso possua ponto, é alguma propriedade de outra entidade
                //Então com base nisso, faz um split, pega os nomes e com eles
                //vai pegando os objetos relacionados e quando estiver na ultima
                //propriedade da lista, será a propriedade buscada e retornada.
                if (getterName.Contains('.'))
                {
                    //Armazena aqui cada objeto do relacionamento que o for ler
                    var aux_obj = current;

                    //Relacionamentos
                    var relationships = getterName.Split('.');
                    for (var i = 0; i < relationships.Length; i++)
                    {
                        //Adicionando "get_" caso nao tenha, porque é obrigatório.
                        if (!relationships[i].Contains("get_"))
                            relationships[i] = "get_" + relationships[i];

                        //Caso seja o ultimo, ja retorna o valor do atributo
                        if (i == relationships.Length - 1)
                            return aux_obj.GetType().GetMethod(relationships[i]).Invoke(aux_obj, null);

                        //Caso nao seja o último, é uma entidade de relacionamento. Poe na var auxiliar e passa para o próximo
                        aux_obj = aux_obj.GetType().GetMethod(relationships[i]).Invoke(aux_obj, null);
                    }
                }

                //Caso nao possua ponto, retorna direto o valor da propriedade
                else
                {
                    var cMi = current.GetType().GetMethod(getterName);
                    return cMi.Invoke(current, null);
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

    }

    [Serializable]
    public class UsuarioSistema
    {
        public Int64 ID { get; set; }
        public string NOME { get; set; }
        public string EMAIL { get; set; }
        public string TELEFONE { get; set; }
        public string CPF { get; set; }
        public string IP { get; set; }
        public string ACAO_EXECUTADA { get; set; }

        public static UsuarioSistema User
        {
            get
            {
                if (HttpContext.Current.Session == null)
                    return new UsuarioSistema();
                if (HttpContext.Current.Session["UsuarioSistema"] == null)
                    HttpContext.Current.Session["UsuarioSistema"] = new UsuarioSistema();
                return (UsuarioSistema)HttpContext.Current.Session["UsuarioSistema"];
            }
        }

        public void Limpar()
        {
            HttpContext.Current.Session.Remove("UsuarioSistema");
        }
    }

    [Serializable]
    public class GridSettings
    {
        public GridSettings()
        {
            rows = 10;
            order = "desc";
        }

        public int page { get; set; }
        public int rows { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
    }

    public static class Paginacao<C> where C : class
    {
        public static List<C> Resultado(GridSettings grid, IQueryable<C> lista, string sortnull, string order = null)
        {
            if (grid.order != null && grid.sort != null)
            {

                List<C> q = lista.OrderBy(grid.sort + " " + grid.order).Skip((grid.page - 1) * grid.rows).Take(grid.rows).Cast<C>().ToList();
                return new List<C>(q);
            }
            else
            {
                if (!String.IsNullOrEmpty(order))
                    grid.order = order;

                return lista
                    .OrderBy(sortnull + " " + grid.order)
                    .Skip((grid.page - 1) * grid.rows)
                    .Take(grid.rows).Cast<C>().ToList();
            }
        }
    }



}