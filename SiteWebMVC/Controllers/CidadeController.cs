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
    public class CidadeController : Controller
    {
        CidadeRepositorio repositorio;

        public CidadeController()
        {
            repositorio = new CidadeRepositorio();
        }
        // GET: Cidade
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Criar()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Criar(cidade instancia)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Criar(instancia);
                }

                return View("Criar", instancia);
            }
            catch (Exception ex)
            {
                return View("Criar", instancia);
            }
        }

        public ActionResult Editar(long id)
        {
            var dados = repositorio.ObterPorID(id);
            return View("Editar", dados);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editar(cidade instancia)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Editar(instancia);
                    ViewBag.Sucesso = "Operação concluida com sucesso!";
                }

                return View("Editar", instancia);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Editar", instancia);
            }
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