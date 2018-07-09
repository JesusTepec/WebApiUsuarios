using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiUsuarios.Models;

namespace WebApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
           
            if (_context.Usuarios.Count() == 0)
            {
                _context.Usuarios.Add(new Usuario { Nombre = "Jesus", UserName = "tepec", Password = "1234" });
                _context.SaveChanges();
            }
        }

        // 
        /// <summary>
        /// GET api/usuario 
        /// devuelve una lista de todos los usuarios
        /// </summary>
        /// <returns>lista de usuarios</returns>
        [HttpGet]
        public ActionResult<List<Usuario>> Usuarios()
        {
            return _context.Usuarios.ToList();
        }

        /// <summary>
        /// GET api/usuario/1
        /// Devuelve los datos de un usuario
        /// Se crea una ruta por nombre de metodo para ser llamado dentro de la clase
        /// </summary>
        /// <param name="id">identificador del usuario</param>
        /// <returns>lista de registros</returns>
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

        // 
        /// <summary>
        /// POST api/usuario   
        /// Si se crea correctamente devuelve el nuevo registro
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Crear(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return CreatedAtRoute("ObtenerUsuario", new { id = usuario.Id}, usuario);
        }

        /// <summary>
        /// PUT api/usuario/1   
        /// si la actualizacion tiene exito se motrara una pantalla en blanco
        /// </summary>
        /// <param name="id">identificador de usuario</param>
        /// <param name="usuario">Objeto de datos a actualizar</param>
        /// <returns>nada si el resultado es exitoso</returns>
        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, Usuario usuario)
        {
            var usuarioRegistro = _context.Usuarios.Find(id);
            if (usuarioRegistro != null)
            {
                usuarioRegistro.Nombre = usuario.Nombre;
                usuarioRegistro.UserName = usuario.UserName;
                usuarioRegistro.Password = usuario.Password;

                _context.Usuarios.Update(usuarioRegistro);
                _context.SaveChanges();
                return NoContent();                
            }
            return NotFound();
        }
        
        /// <summary>
        /// DELETE api/usuario/1
        /// Eliminar un registro de usuario
        /// </summary>
        /// <param name="id">identificador de usuario</param>
        /// <returns>nada si la accin se completa con exito</returns>
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