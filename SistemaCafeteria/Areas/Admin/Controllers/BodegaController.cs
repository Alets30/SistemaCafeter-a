using Microsoft.AspNetCore.Mvc;
using SistemaCafeteria.AccesoDatos.Repositorio.IRepositorio;
using SistemaCafeteria.Modelos;
using SistemaCafeteria.Utilidades;

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

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if (bodega.Id == 0)
                {
                    await _unidadTrabajo.Bodega.Agregar(bodega);
                    TempData[DS.Exitosa] = "La Bodega se creó con Éxito.";
                }
                else
                {
                    _unidadTrabajo.Bodega.Actualizar(bodega);
                    TempData[DS.Exitosa] = "La Bodega se actualizó con Éxito.";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al Grabar la Bodega.";
            return View(bodega);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegaDB = await _unidadTrabajo.Bodega.Obtener(id);
            if (bodegaDB == null)
            {
                return Json(new { success = false, message = "Error al borrar el Registro en la Base de Datos." });
            }
            _unidadTrabajo.Bodega.Remover(bodegaDB);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Bodega eliminada con éxito." });
        }

        #region API 
        [HttpGet]
        public async Task <IActionResult> ObtenerTodos() { 
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos(); 
            return Json(new {data = todos}); 
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Bodega.ObtenerTodos();

            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim()
                        == nombre.ToLower().Trim()
                        && b.Id != id);
            }

            if (valor)
            {
                return Json(new { data = true });
            }
            else
            {
                return Json(new { data = false });
            }
        }
        #endregion

        public async Task<IActionResult> Upsert(int? id)
        {
            Bodega bodega = new Bodega();
            if (id == null)
            {
                //creamos un nuevo registro
                bodega.Estado = true;
                return View(bodega);

            }
            bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());
            if (bodega == null)
            {
                return NotFound();
            }
            return View(bodega);
        }
    }
    }
