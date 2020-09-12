using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ProyectoIPC22011903872.Models;
using ProyectoIPC22011903872.Models.ViewModels;

namespace ProyectoIPC22011903872.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login(string Usuario, string Contraseña) {
            ViewBag.Usuario = Usuario;
            ViewBag.Contraseña = Contraseña;
            if(User.Identity.IsAuthenticated){
                return RedirectToAction("Index", "Principal");
            }
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
        [HttpPost]
        public ActionResult Validar(string Usuario, string password) {
            var us = "";
            var Contraseña = "";
            using (OthelloEntities db = new OthelloEntities())
            {
                var pass = Encriptar.CrearHASH(password);
                var usuario2 = db.USUARIO.FirstOrDefault(e => e.Usuario1 == Usuario);
                var usuario = db.USUARIO.FirstOrDefault(e => e.Usuario1 == Usuario && e.Contraseña == pass);
                if (usuario2 != null)
                {
                    if (usuario != null)
                    {
                        FormsAuthentication.SetAuthCookie(usuario.Usuario1, true);
                        return RedirectToAction("Index", "Principal");
                    }
                    else
                    {
                        Contraseña = "Contraseña Incorrecta";
                    }
                }
                else
                {
                    us = "El usuario ingresado no existe.";
                }
            }
            return RedirectToAction("Login", "User", new { Usuario = us, Contraseña = Contraseña});
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Principal");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(addUserViewModel model) {
            

                if (ModelState.IsValid) {
                    using (OthelloEntities db= new OthelloEntities())
                    {
                        var user = db.USUARIO.FirstOrDefault(e => e.Usuario1 == model.Usuario);
                        var user2 = db.USUARIO.FirstOrDefault(e => e.Correo_Electronico == model.Correo_Electronico);
                        if (user==null)
                        {
                            if (user2 == null) { 
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
                                    return Redirect("~/");
                                }
                                else
                                {
                                    ViewBag.mensajeContraseña = "Contraseña no coincide";
                                }
                            }
                            else
                            {
                                ViewBag.mensajeCorreo = "Correo ya registrado";
                            }
                        }
                        else
                        {
                            ViewBag.mensajeUsuario = "Usuario ya Registrado";
                        }
                    }
                }
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
            return View(model);
            
        }
    }
}