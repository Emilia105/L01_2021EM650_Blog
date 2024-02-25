using L01_2021EM650.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2021EM650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacionesController : ControllerBase
    {
        private readonly BlogContext _BlogContexto;

        public PublicacionesController(BlogContext blogContexto)
        {
            _BlogContexto = blogContexto;
        }
        //CRUD


        //Getall
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Publicacione> listadoPublicaciones = (from e in _BlogContexto.Publicaciones select e).ToList();

            if (listadoPublicaciones.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoPublicaciones);
        }

        //Filtros
        //Listado filtrado por usuario

        [HttpGet]
        [Route("GetByUser/{id}")]
        public IActionResult Get(int id)
        {
            List<Publicacione> publicaciones = (from e in _BlogContexto.Publicaciones where e.UsuarioId == id select e).ToList();
            if (publicaciones == null)
            {
                return NotFound();
            }
            return Ok(publicaciones);

        }
        //Post
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPublicacion([FromBody] Publicacione publicaciones)
        {
            try
            {
                _BlogContexto.Publicaciones.Add(publicaciones);
                _BlogContexto.SaveChanges();
                return Ok(publicaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Put
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPublicacion(int id, [FromBody] Publicacione publicacionModificar)
        {
            //obtencion de registro original
            Publicacione? PublicacionActual = (from e in _BlogContexto.Publicaciones
                                      where e.PublicacionId == id
                                      select e).FirstOrDefault();

            //verificacion de existencia del registro segun ID
            if (PublicacionActual == null)
            {
                return NotFound();
            }
            //Alteración de los campos
            PublicacionActual.Titulo = publicacionModificar.Titulo;
            PublicacionActual.Descripcion = publicacionModificar.Descripcion;
            PublicacionActual.UsuarioId = publicacionModificar.UsuarioId;

            //registro marcado como modificado en el contexto y se envia a la modificacion a la bd
            _BlogContexto.Entry(PublicacionActual).State = EntityState.Modified;
            _BlogContexto.SaveChanges();

            return Ok(publicacionModificar);
        }
        //Delete
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPublicacion(int id)
        {
            //Obtención de registro
            Publicacione? publicacion = (from e in _BlogContexto.Publicaciones
                                where e.PublicacionId == id
                                select e).FirstOrDefault();
            //verificacion de la existencia del registro
            if (publicacion == null)
            {
                return NotFound();
            }
            //Eliminación del registro
            _BlogContexto.Publicaciones.Attach(publicacion);
            _BlogContexto.Publicaciones.Remove(publicacion);
            _BlogContexto.SaveChanges();

            return Ok(publicacion);
        }
    }
}
