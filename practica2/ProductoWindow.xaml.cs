using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Entidades;
using Negocio;

namespace practica2;

public partial class ProductoWindow : Window
{
    private readonly NProducto nProducto = new NProducto();
    private Product? productoEditando;

    public ProductoWindow()
    {
        InitializeComponent();
        Title = "Nuevo Producto";
        txtTitulo.Text = "Nuevo Producto";
    }

    public ProductoWindow(Product producto)
    {
        InitializeComponent();
        productoEditando = producto;
        Title = "Editar Producto";
        txtTitulo.Text = "Editar Producto";
        CargarDatos();
    }

    private void CargarDatos()
    {
        if (productoEditando == null) return;

        spId.Visibility = Visibility.Visible;
        txtId.Text = productoEditando.ProductId.ToString();
        txtCodigo.Text = productoEditando.Code;
        txtNombre.Text = productoEditando.Name;
        txtDescripcion.Text = productoEditando.Description ?? string.Empty;
        txtPrecio.Text = productoEditando.Price.ToString(CultureInfo.InvariantCulture);
        txtStock.Text = productoEditando.Stock.ToString();
        cboCategoria.Text = productoEditando.Category ?? string.Empty;
    }

    private void BtnGuardar_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtCodigo.Text))
        {
            MessageBox.Show("El código es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtCodigo.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtNombre.Focus();
            return;
        }

        if (!decimal.TryParse(txtPrecio.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
        {
            MessageBox.Show("El precio debe ser un número válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            txtPrecio.Focus();
            return;
        }

        if (precio < 0)
        {
            MessageBox.Show("El precio no puede ser negativo.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtPrecio.Focus();
            return;
        }

        if (!int.TryParse(txtStock.Text, out int stock))
        {
            MessageBox.Show("El stock debe ser un número entero válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            txtStock.Focus();
            return;
        }

        if (stock < 0)
        {
            MessageBox.Show("El stock no puede ser negativo.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtStock.Focus();
            return;
        }

        var producto = new Product
        {
            Code = txtCodigo.Text.Trim(),
            Name = txtNombre.Text.Trim(),
            Description = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
            Price = precio,
            Stock = stock,
            Category = string.IsNullOrWhiteSpace(cboCategoria.Text) ? null : cboCategoria.Text.Trim()
        };

        if (productoEditando != null)
        {
            producto.ProductId = productoEditando.ProductId;
            var (exito, mensaje) = nProducto.Actualizar(producto);
            if (exito)
            {
                MessageBox.Show(mensaje, "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            var (exito, mensaje) = nProducto.Insertar(producto);
            if (exito)
            {
                MessageBox.Show(mensaje, "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void BtnCancelar_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
