using Microsoft.AspNetCore.Mvc;
using SistemaCafeteria.AccesoDatos.Repositorio.IRepositorio;
using SistemaCafeteria.Modelos;
using SistemaCafeteria.Utilidades;

namespace SistemaCafeteria.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MarcaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo; 
        public MarcaController(IUnidadTrabajo unidadTrabajo) {
            _unidadTrabajo = unidadTrabajo; 
        }
        public IActionResult Index() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Upsert(Marca marca)
        {
            if (ModelState.IsValid)
            {
                if (marca.Id == 0)
                {
                    await _unidadTrabajo.Marca.Agregar(marca);
                    TempData[DS.Exitosa] = "La Marca se creó con Éxito.";
                }
                else
                {
                    _unidadTrabajo.Marca.Actualizar(marca);
                    TempData[DS.Exitosa] = "La Marca se actualizó con Éxito.";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al Grabar la Marca.";
            return View(marca);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var marcaDB = await _unidadTrabajo.Marca.Obtener(id);
            if (marcaDB == null)
            {
                return Json(new { success = false, message = "Error al borrar el Registro en la Base de Datos." });
            }
            _unidadTrabajo.Marca.Remover(marcaDB);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca eliminada con éxito." });
        }

        #region API 
        [HttpGet]
        public async Task <IActionResult> ObtenerTodos() { 
            var todos = await _unidadTrabajo.Marca.ObtenerTodos(); 
            return Json(new {data = todos}); 
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Marca.ObtenerTodos();

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
            Marca marca = new Marca();
            if (id == null)
            {
                //creamos un nuevo registro
                marca.Estado = true;
                return View(marca);

            }
            marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);
        }
    }
    }
