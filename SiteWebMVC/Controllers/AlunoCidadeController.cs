using Model.Logic.Model;
using Repository.Logic.Banco;
using SiteWebMVC.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteWebMVC.Controllers
{
    public class AlunoCidadeController : Controller
    {
        CidadeRepositorio repositorio;

        public AlunoCidadeController()
        {
            repositorio = new CidadeRepositorio();
        }
        // GET: Cidade
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Alunos(long id)
        {
            ViewBag.dados = new AlunoRepositorio().ObterPorObjeto(x=>x.id_cidade == id);
            ViewBag.cidade = repositorio.ObterPorID(id).nome;
            return View("Alunos");
        }

        public JsonResult GridCidade(GridSettings grid, cidade instancia)
        {
            try
            {
                var inst = repositorio.Listar(instancia).OrderBy(c => c.id_cidade);
                var listacid = new List<object>();

                foreach (var resultado in Paginacao<cidade>.Resultado(grid, inst, "id_cidade"))
                {
                    var objeto = new
                    {
                        resultado.id_cidade,
                        resultado.nome,
                        resultado.cep,
                        resultado.estado
                    };
                    listacid.Add(objeto);
                }

                return Json(new
                {
                    rows = listacid,
                    total = inst.Count(),
                    page = (int)Math.Ceiling(inst.Count() / (decimal)grid.rows)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { mensagem = ex.Message, tipoMensagem = "Alerta" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}