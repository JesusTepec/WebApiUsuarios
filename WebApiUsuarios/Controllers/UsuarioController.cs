using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiUsuarios.Models;

namespace WebApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;

            if (_context.Usuarios.Count() == 0)
            {
                _context.Usuarios.Add(new Usuario { Nombre = "Jesus", UserName = "JesTep", Password = "1234" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<Usuario>> Usuarios()
        {
            return _context.Usuarios.ToList();
        }
        
        /* Se crea una ruta por nombre de metodo,
         * el método devuelve los datos de un usuario
         */
        [HttpGet("{id}", Name = "ObtenerUsuario")]
        public ActionResult<Usuario> ObtenerUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if(usuario != null)
            {
                return usuario;
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Crear(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return CreatedAtRoute("ObtenerUsuario", new { id = usuario.Id}, usuario);
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, Usuario usuario)
        {
            var usuarioRegistro = _context.Usuarios.Find(id);
            if (usuarioRegistro != null)
            {
                usuarioRegistro.Nombre = usuario.Nombre;

                _context.Usuarios.Update(usuarioRegistro);
                _context.SaveChanges();
                return NoContent();                
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var registroUsuario = _context.Usuarios.Find(id);
            if (registroUsuario != null)
            {
                _context.Usuarios.Remove(registroUsuario);
                _context.SaveChanges();
                return NoContent();
            }
            return NotFound();
        }
    }
}