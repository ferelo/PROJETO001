using Repository.Logic.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteWebMVC.Controllers
{
    public class RelatoriosController : Controller
    {
        // GET: Relatorios
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Alunos()
        {
            ViewBag.Alunos = new AlunoRepositorio().ObterTodos().ToList();
            return View();
        }

        public ActionResult Cidade()
        {
            ViewBag.Cidades = new CidadeRepositorio().ObterTodos().ToList();
            return View();
        }

        public ActionResult AlunoCidade()
        {
            return View();
        }
    }
}