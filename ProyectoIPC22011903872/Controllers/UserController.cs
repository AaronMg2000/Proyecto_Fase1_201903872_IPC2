using System;
using System.Collections.Generic;
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
            return View();
        }
    }
}