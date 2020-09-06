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
                db.USUARIO.FirstOrDefault(e => e.Usuario1 == Usuario && e.Contraseña == Encriptar.CrearHASH(password));
            }
                return View();
        }
        public ActionResult Registrar() {
            return View();
        }
    }
}