using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIPC22011903872.Models;
using ProyectoIPC22011903872.Models.ViewModels;

namespace ProyectoIPC22011903872.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login() {
            return View();
        }
        public ActionResult getUser()
        {
            List<getUserViewModel> ListaUsuarios;
            using (OthelloEntities db = new OthelloEntities())
            {
                ListaUsuarios = (from d in db.USUARIO
                                 select new getUserViewModel
                                 {
                                    Nombre = d.Nombre,
                                    Apellido = d.Apellido,
                                    Usuario = d.Usuario1,
                                    Correo_Electronico = d.Correo_Electronico,
                                    Codigo_Pais = d.Codigo_Pais,
                                    Fecha_nacimiento = d.Fecha_nacimiento
                                 }).ToList();
            }
            return View(ListaUsuarios);
        }
        [HttpGet]
        public ActionResult Validar(string Usuario, string password) {
            using (OthelloEntities db = new OthelloEntities()) {
                if (!String.IsNullOrEmpty(Usuario) && !String.IsNullOrEmpty(password))
                {
                    db.USUARIO.FirstOrDefault(e => e.Usuario1 == Usuario && e.Contraseña == Encriptar.CrearHASH(password));

                }
            }
                return View();
        }
        public ActionResult Registrar() {
            List<getPaisViewModel> ListaPais;
            using (OthelloEntities db = new OthelloEntities())
            {
                ListaPais = (from d in db.PAIS
                             select new getPaisViewModel
                             {
                                 codigo = d.codigo_Pais,
                                 nombre = d.Nombre,
                                 ISO3 = d.ISO3

                             }).ToList();
            }
            ViewBag.paises = ListaPais;
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(addUserViewModel model) {
            List<getPaisViewModel> ListaPais;
            using (OthelloEntities db = new OthelloEntities())
            {
                ListaPais = (from d in db.PAIS
                             select new getPaisViewModel
                             {
                                 codigo = d.codigo_Pais,
                                 nombre = d.Nombre,
                                 ISO3 = d.ISO3

                             }).ToList();
            }
            ViewBag.paises = ListaPais;

                if (ModelState.IsValid) {
                    using (OthelloEntities db= new OthelloEntities())
                    {
                        var user = db.USUARIO.FirstOrDefault(e => e.Usuario1 == model.Usuario);
                        if (user==null)
                        {
                            if (model.Password==model.RePassword)
                            {
                                var Usuario = new USUARIO();
                                Usuario.Nombre = model.Nombre;
                                Usuario.Apellido = model.Apellido;
                                Usuario.Correo_Electronico = model.Correo_Electronico;
                                Usuario.Usuario1 = model.Usuario;
                                Usuario.Contraseña = Encriptar.CrearHASH(model.Password);
                                Usuario.Codigo_Pais = model.Codigo_Pais;
                                Usuario.Fecha_nacimiento = model.Fecha_nacimiento;
                                db.USUARIO.Add(Usuario);
                                db.SaveChanges();
                            }
                        }
                    }
                    return Redirect("~/");
                }
                return View(model);
            
        }
    }
}