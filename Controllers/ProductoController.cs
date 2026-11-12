using Microsoft.AspNetCore.Mvc;
using PrototipoCRUD.Data;
using PrototipoCRUD.Models;

namespace PrototipoCRUD.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoDAO _dao = new ProductoDAO();

        public IActionResult Index()
        {
            var productos = _dao.ObtenerTodos();
            return View(productos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            _dao.Insertar(producto);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var producto = _dao.ObtenerPorId(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            _dao.Actualizar(producto);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _dao.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
