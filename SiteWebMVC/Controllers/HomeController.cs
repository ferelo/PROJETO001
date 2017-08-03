using Model.Logic.Model;
using Repository.Logic.Banco;
using SiteWebMVC.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SiteWebMVC.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("LoginGerenciador");
        }

        public ActionResult LoginGerenciador()
        {
            return !SessionLogin.EstaLogado() ? (ActionResult)View() : RedirectToAction("PainelGerenciador");
        }

        public ActionResult Cadastro()
        {
            return View();
        }


        public ActionResult PainelGerenciador()
        {
            return View();
        }

        public ActionResult UsuarioSair()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginGerenciador");
        }

        public ActionResult CadastroSalvo(usuario usr)
        {
            var repositorio = new UsuarioRepositorio();

            #region Validação dos campos

            if (String.IsNullOrEmpty(usr.nome))
                ModelState.AddModelError("", "Campo 'nome' não pode ser vazio");

            if (String.IsNullOrEmpty(usr.cpf))
                ModelState.AddModelError("", "Campo 'cpf' não pode ser vazio");

            if (String.IsNullOrEmpty(usr.cidade))
                ModelState.AddModelError("", "Campo 'cidade' não pode ser vazio");

            if (String.IsNullOrEmpty(usr.email))
                ModelState.AddModelError("", "Campo 'email' não pode ser vazio");

            if (String.IsNullOrEmpty(usr.senha))
                ModelState.AddModelError("", "Campo 'senha' não pode ser vazio");

            var resultCpf = repositorio.ObterPorObjeto(x => x.cpf == usr.cpf).ToList();
            if (resultCpf.Any())
                ModelState.AddModelError("", "Campo 'cpf' já está cadastrado");

            var resultEmail = repositorio.ObterPorObjeto(x => x.email == usr.email).ToList();
            if (resultEmail.Any())
                ModelState.AddModelError("", "Campo 'email' já está cadastrado");

            #endregion

            if (ModelState.IsValid)
            {


                usr.data_cadastro = DateTime.Now;
                usr.status = 1; // Cadastro ativo

                repositorio.Criar(usr);
                ViewBag.Sucesso = "Operação concluida com sucesso!";
                return RedirectToAction("Index");
            }


            return View("Cadastro");

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LoginGerenciador(usuario instancia)
        {
            if (String.IsNullOrEmpty(instancia.email) || String.IsNullOrEmpty(instancia.senha))
                ModelState.AddModelError("", "Preencher E-mail e Senha");

            if (ModelState.IsValid)
            {
                var usuario = new UsuarioRepositorio().ObterTodos().Where(u => u.email.Equals(instancia.email.Trim()) && u.senha.Equals(instancia.senha.Trim()));

                if (usuario.Any(u => u.status == 1)) // usuario Ativo
                {
                    SessionLogin.CriarSessionParaSistema(instancia.email);

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        instancia.email,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(20),
                        false,
                        "Autenticado",
                        FormsAuthentication.FormsCookiePath);
                    string hashCookies = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
                    Response.Cookies.Add(cookie);

                    return RedirectToAction("PainelGerenciador");
                }
                else
                {
                    ModelState.AddModelError("", "E-mail e Senha não confere, por favor informe os dados corretos!");
                }
            }
            return View("LoginGerenciador", instancia);
        }


    }
}