using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Entidades;
using Negocio;

namespace practica2;

public partial class MainWindow : Window
{
    private readonly NProducto nProducto = new NProducto();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        CargarProductos();
    }

    private void CargarProductos()
    {
        try
        {
            var lista = nProducto.Listar();
            dgProductos.ItemsSource = lista;
            txtMensaje.Text = $"Total: {lista.Count} productos";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnBuscar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string filtro = txtBuscar.Text.Trim();
            var lista = nProducto.Buscar(filtro);
            dgProductos.ItemsSource = lista;
            txtMensaje.Text = $"Encontrados: {lista.Count} productos";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al buscar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnMostrarTodos_Click(object sender, RoutedEventArgs e)
    {
        txtBuscar.Text = string.Empty;
        CargarProductos();
    }

    private void BtnNuevo_Click(object sender, RoutedEventArgs e)
    {
        var ventana = new ProductoWindow();
        if (ventana.ShowDialog() == true)
        {
            CargarProductos();
        }
    }

    private void BtnEditar_Click(object sender, RoutedEventArgs e)
    {
        if (dgProductos.SelectedItem is not Product producto)
        {
            MessageBox.Show("Seleccione un producto para editar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var ventana = new ProductoWindow(producto);
        if (ventana.ShowDialog() == true)
        {
            CargarProductos();
        }
    }

    private void BtnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (dgProductos.SelectedItem is not Product producto)
        {
            MessageBox.Show("Seleccione un producto para eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var resultado = MessageBox.Show(
            $"¿Eliminar producto '{producto.Name}'?",
            "Confirmar eliminación",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (resultado == MessageBoxResult.Yes)
        {
            var (exito, mensaje) = nProducto.Eliminar(producto.ProductId);
            MessageBox.Show(mensaje, "Resultado", MessageBoxButton.OK, 
                exito ? MessageBoxImage.Information : MessageBoxImage.Error);
            CargarProductos();
        }
    }
}

public class FlagToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b ? "Eliminado" : "Activo";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
