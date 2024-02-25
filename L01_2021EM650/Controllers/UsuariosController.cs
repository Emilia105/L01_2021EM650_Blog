using Microsoft.AspNetCore.Http;
using L01_2021EM650.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace L01_2021EM650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly BlogContext _BlogContexto;

        public UsuariosController(BlogContext blogContexto)
        {
            _BlogContexto = blogContexto;
        }
        //CRUD


        //Getall
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Usuario> listadoUsuarios = (from e in _BlogContexto.Usuarios select e).ToList();

            if (listadoUsuarios.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoUsuarios);
        }

        //Filtros
        //Listado filtrado por Nombre y Apellido
        [HttpGet]
        [Route("FindNomAp")]
        public IActionResult FiltroNombreApellido(string nombre, string apellido)
        {
            List<Usuario> listadoNA = (from e in _BlogContexto.Usuarios where e.Nombre == nombre && e.Apellido == apellido select e).ToList();

            if (listadoNA.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoNA);
        }
        //Rol
        [HttpGet]
        [Route("GetByRole/{id}")]
        public IActionResult Get(int id)
        {
            List<Usuario> usuario = (from e in _BlogContexto.Usuarios where e.RolId == id select e).ToList();
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);

        }
        //Post
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarUsuario([FromBody] Usuario usuarios)
        {
            try
            {
                _BlogContexto.Usuarios.Add(usuarios);
                _BlogContexto.SaveChanges();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Put
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] Usuario usuarioModificar)
        {
            //obtencion de registro original
            Usuario? usuarioActual = (from e in _BlogContexto.Usuarios
                                    where e.UsuarioId == id
                                    select e).FirstOrDefault();

            //verificacion de existencia del registro segun ID
            if (usuarioActual == null)
            {
                return NotFound();
            }
            //Alteración de los campos
            usuarioActual.RolId = usuarioModificar.RolId;
            usuarioActual.NombreUsuario = usuarioModificar.NombreUsuario;
            usuarioActual.Clave = usuarioModificar.Clave;
            usuarioActual.Nombre = usuarioModificar.Nombre;
            usuarioActual.Apellido = usuarioModificar.Apellido;

            //registro marcado como modificado en el contexto y se envia a la modificacion a la bd
            _BlogContexto.Entry(usuarioActual).State = EntityState.Modified;
            _BlogContexto.SaveChanges();

            return Ok(usuarioModificar);
        }
        //Delete
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            //Obtención de registro
            Usuario? usuario = (from e in _BlogContexto.Usuarios
                              where e.UsuarioId == id
                              select e).FirstOrDefault();
            //verificacion de la existencia del registro
            if (usuario == null)
            {
                return NotFound();
            }
            //Eliminación del registro
            _BlogContexto.Usuarios.Attach(usuario);
            _BlogContexto.Usuarios.Remove(usuario);
            _BlogContexto.SaveChanges();

            return Ok(usuario);
        }
    }
}
