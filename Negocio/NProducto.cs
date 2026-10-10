using Datos;
using Entidades;

namespace Negocio;

public class NProducto
{
    public List<Product> Listar()
    {
        DProducto dProducto = new DProducto();
        return dProducto.Listar();
    }

    public List<Product> Buscar(string filtro)
    {
        if (string.IsNullOrWhiteSpace(filtro))
        {
            return Listar();
        }

        DProducto dProducto = new DProducto();
        return dProducto.Buscar(filtro.Trim());
    }

    public Product? ObtenerPorId(int id)
    {
        if (id <= 0) return null;

        DProducto dProducto = new DProducto();
        return dProducto.ObtenerPorId(id);
    }

    public (bool Exito, string Mensaje) Insertar(Product producto)
    {
        if (string.IsNullOrWhiteSpace(producto.Code))
            return (false, "El código es obligatorio.");

        if (string.IsNullOrWhiteSpace(producto.Name))
            return (false, "El nombre es obligatorio.");

        if (producto.Price < 0)
            return (false, "El precio no puede ser negativo.");

        if (producto.Stock < 0)
            return (false, "El stock no puede ser negativo.");

        DProducto dProducto = new DProducto();
        int result = dProducto.Insertar(producto);
        return result > 0 ? (true, "Producto insertado correctamente.") : (false, "No se pudo insertar el producto.");
    }

    public (bool Exito, string Mensaje) Actualizar(Product producto)
    {
        if (string.IsNullOrWhiteSpace(producto.Code))
            return (false, "El código es obligatorio.");

        if (string.IsNullOrWhiteSpace(producto.Name))
            return (false, "El nombre es obligatorio.");

        if (producto.Price < 0)
            return (false, "El precio no puede ser negativo.");

        if (producto.Stock < 0)
            return (false, "El stock no puede ser negativo.");

        DProducto dProducto = new DProducto();
        int filas = dProducto.Actualizar(producto);
        return filas > 0 ? (true, "Producto actualizado correctamente.") : (false, "No se pudo actualizar el producto.");
    }

    public (bool Exito, string Mensaje) Eliminar(int id)
    {
        if (id <= 0)
            return (false, "ID inválido.");

        DProducto dProducto = new DProducto();
        int filas = dProducto.Eliminar(id);
        return filas > 0 ? (true, "Producto eliminado correctamente.") : (false, "No se pudo eliminar el producto.");
    }
}
