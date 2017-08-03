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
    public class AlunoController : Controller
    {
        AlunoRepositorio repositorio;

        public AlunoController()
        {
            repositorio = new AlunoRepositorio();
        }
        // GET: Aluno
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Criar()
        {

            ViewBag.id_cidade = SessionLogin.CriarSelectList(new CidadeRepositorio().ObterTodos(), "id_cidade", "nome");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Criar(aluno instancia)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    instancia.status = 1;
                    instancia.data_cadastro = DateTime.Now;

                    repositorio.Criar(instancia);
                    ViewBag.Sucesso = "Operação concluida com sucesso!";

                }
                ViewBag.id_cidade = SessionLogin.CriarSelectList(new CidadeRepositorio().ObterTodos(), "id_cidade", "nome");
                return View("Criar", instancia);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.id_cidade = SessionLogin.CriarSelectList(new CidadeRepositorio().ObterTodos(), "id_cidade", "nome");
                return View("Criar", instancia);
            }
        }

        public ActionResult Editar(long id)
        {
            var dados = repositorio.ObterPorID(id);
            ViewBag.id_cidade = SessionLogin.CriarSelectList(new CidadeRepositorio().ObterTodos(), "id_cidade", "nome", dados.id_cidade);
            ViewBag.data_nascimento = dados.data_nascimento.ToString("dd/MM/yyyy");
            return View("Editar", dados);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editar(aluno instancia)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Editar(instancia);
                    ViewBag.Sucesso = "Operação concluida com sucesso!";
                }
                ViewBag.id_cidade = SessionLogin.CriarSelectList(new CidadeRepositorio().ObterTodos(), "id_cidade", "nome", instancia.id_cidade);
                return View("Editar", instancia);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.id_cidade = SessionLogin.CriarSelectList(new CidadeRepositorio().ObterTodos(), "id_cidade", "nome", instancia.id_cidade);
                return View("Editar", instancia);
            }
        }

        public ActionResult Pais(long id)
        {
            var dados = repositorio.ObterPorID(id);
            var p = new pais();
            p.id_aluno = dados.id_aluno;
            return View("Pais", p);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Pais(pais instancia)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    instancia.data_cadastro = DateTime.Now;
                    instancia.status = 1; 
                    new PaisRepositorio().Criar(instancia);
                    ViewBag.Sucesso = "Operação concluida com sucesso!";
                }

                return View("Pais", instancia);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Pais", instancia);
            }
        }


        public JsonResult GridAluno(GridSettings grid, aluno instancia)
        {
            try
            {
                var inst = repositorio.Listar(instancia).OrderBy(c => c.id_aluno);
                var listaMontador = new List<object>();

                foreach (var resultado in Paginacao<aluno>.Resultado(grid, inst, "id_aluno"))
                {
                    var objeto = new
                    {
                        resultado.id_aluno,
                        resultado.nome,
                        resultado.cpf,
                        resultado.telefone,
                        data_cadastro = resultado.data_cadastro.ToString("dd/MM/yyyy hh:mm"),
                        cidade_ = new CidadeRepositorio().ObterPorID(resultado.id_cidade).nome,
                        sexo = resultado.sexo == 1 ? "Masculino" : "Feminino"
                    };
                    listaMontador.Add(objeto);
                }

                return Json(new
                {
                    rows = listaMontador,
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