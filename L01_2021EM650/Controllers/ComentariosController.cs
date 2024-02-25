using Microsoft.AspNetCore.Http;
using L01_2021EM650.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace L01_2021EM650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly BlogContext _BlogContexto;

        public ComentariosController(BlogContext blogContexto)
        {
            _BlogContexto = blogContexto;
        }
        //CRUD


        //Getall
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Comentario> listadoComentarios = (from e in _BlogContexto.Comentarios select e).ToList();

            if (listadoComentarios.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoComentarios);
        }

        //Filtros
        //Listado filtrado por publicacion

        [HttpGet]
        [Route("GetByPublication/{id}")]
        public IActionResult Get(int id)
        {
            List<Comentario> comentarios = (from e in _BlogContexto.Comentarios where e.PublicacionId == id select e).ToList();
            if (comentarios == null)
            {
                return NotFound();
            }
            return Ok(comentarios);

        }
        //Post
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarComentario([FromBody] Comentario comentario)
        {
            try
            {
                _BlogContexto.Comentarios.Add(comentario);
                _BlogContexto.SaveChanges();
                return Ok(comentario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Put
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarComentario(int id, [FromBody] Comentario ComentarioModificar)
        {
            //obtencion de registro original
            Comentario? ComentarioActual = (from e in _BlogContexto.Comentarios
                                               where e.CometarioId == id
                                               select e).FirstOrDefault();

            //verificacion de existencia del registro segun ID
            if (ComentarioActual == null)
            {
                return NotFound();
            }
            //Alteración de los campos
            ComentarioActual.PublicacionId = ComentarioModificar.PublicacionId;
            ComentarioActual.Comentario1 = ComentarioModificar.Comentario1;
            ComentarioActual.UsuarioId = ComentarioModificar.UsuarioId;

            //registro marcado como modificado en el contexto y se envia a la modificacion a la bd
            _BlogContexto.Entry(ComentarioActual).State = EntityState.Modified;
            _BlogContexto.SaveChanges();

            return Ok(ComentarioModificar);
        }
        //Delete
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarComentario(int id)
        {
            //Obtención de registro
            Comentario? comentario = (from e in _BlogContexto.Comentarios
                                         where e.CometarioId == id
                                         select e).FirstOrDefault();
            //verificacion de la existencia del registro
            if (comentario == null)
            {
                return NotFound();
            }
            //Eliminación del registro
            _BlogContexto.Comentarios.Attach(comentario);
            _BlogContexto.Comentarios.Remove(comentario);
            _BlogContexto.SaveChanges();

            return Ok(comentario);
        }
    }
}
