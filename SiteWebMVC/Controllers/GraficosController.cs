using Repository.Logic.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteWebMVC.Controllers
{
    public class GraficosController : Controller
    {
        // GET: Graficos
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GerarGrafico()
        {

            var dados = new AlunoRepositorio().ObterTodos().ToList();
            var lista = new List<object>();

            foreach (var d in dados)
            {


                var obj = new
                {
                    info = new CidadeRepositorio().ObterPorID(d.id_cidade).nome,
                    qtd_aluno = dados.Where(x => x.id_cidade == d.id_cidade).Count()
                };

                lista.Add(obj);
            }

            var listaGrid = lista.Distinct();

            return Json(new
            {
                listaGrid
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GerarGraficoDataCadastro()
        {

            var dados = new AlunoRepositorio().ObterTodos().ToList();
            var lista = new List<object>();

            foreach (var d in dados)
            {


                var obj = new
                {
                    info = d.data_cadastro.ToString("dd/MM/yyyy"),
                    qtd_aluno = dados.Where(x => x.data_cadastro.ToString("dd/MM/yyyy") == d.data_cadastro.ToString("dd/MM/yyyy")).Count()
                };

                lista.Add(obj);
            }

            var listaGrid = lista.Distinct();

            return Json(new
            {
                listaGrid
            }, JsonRequestBehavior.AllowGet);

        }
    }
}