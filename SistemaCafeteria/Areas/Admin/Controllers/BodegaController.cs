using Microsoft.AspNetCore.Mvc;
using SistemaCafeteria.AccesoDatos.Repositorio.IRepositorio;

namespace SistemaCafeteria.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BodegaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo; 
        public BodegaController(IUnidadTrabajo unidadTrabajo) {
            _unidadTrabajo = unidadTrabajo; 
        }
        public IActionResult Index() { return View(); }
        #region API 
        [HttpGet]
        public async Task <IActionResult> ObtenerTodos() { 
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos(); 
            return Json(new {data = todos}); 
        } 
        #endregion 
    }
    }
