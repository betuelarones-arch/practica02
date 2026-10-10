using Microsoft.Data.SqlClient;
using System.Data;
using Entidades;

namespace Datos;

public class DProducto
{
    private readonly Conexion conexion = new Conexion();

    public List<Product> Listar()
    {
        List<Product> lista = new List<Product>();

        try
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("dbo.SP_ListProducts", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Product producto = new Product
                {
                    ProductId = Convert.ToInt32(dr["ProductId"]),
                    Code = dr["Code"].ToString() ?? string.Empty,
                    Name = dr["Name"].ToString() ?? string.Empty,
                    Description = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : null,
                    Price = Convert.ToDecimal(dr["Price"]),
                    Stock = Convert.ToInt32(dr["Stock"]),
                    Category = dr["Category"] != DBNull.Value ? dr["Category"].ToString() : null,
                    Flag = dr["Flag"] != DBNull.Value && Convert.ToBoolean(dr["Flag"]),
                    CreationDate = Convert.ToDateTime(dr["CreationDate"])
                };

                lista.Add(producto);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexion: {ex.Message}");
        }

        return lista;
    }

    public List<Product> Buscar(string filtro)
    {
        List<Product> lista = new List<Product>();

        try
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("dbo.SP_SearchProducts", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Filter", SqlDbType.VarChar) { Value = filtro });

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Product producto = new Product
                {
                    ProductId = Convert.ToInt32(dr["ProductId"]),
                    Code = dr["Code"].ToString() ?? string.Empty,
                    Name = dr["Name"].ToString() ?? string.Empty,
                    Description = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : null,
                    Price = Convert.ToDecimal(dr["Price"]),
                    Stock = Convert.ToInt32(dr["Stock"]),
                    Category = dr["Category"] != DBNull.Value ? dr["Category"].ToString() : null,
                    Flag = dr["Flag"] != DBNull.Value && Convert.ToBoolean(dr["Flag"]),
                    CreationDate = Convert.ToDateTime(dr["CreationDate"])
                };

                lista.Add(producto);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexion: {ex.Message}");
        }

        return lista;
    }

    public Product? ObtenerPorId(int id)
    {
        try
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("dbo.SP_GetProductById", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = id });

            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new Product
                {
                    ProductId = Convert.ToInt32(dr["ProductId"]),
                    Code = dr["Code"].ToString() ?? string.Empty,
                    Name = dr["Name"].ToString() ?? string.Empty,
                    Description = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : null,
                    Price = Convert.ToDecimal(dr["Price"]),
                    Stock = Convert.ToInt32(dr["Stock"]),
                    Category = dr["Category"] != DBNull.Value ? dr["Category"].ToString() : null,
                    Flag = dr["Flag"] != DBNull.Value && Convert.ToBoolean(dr["Flag"]),
                    CreationDate = Convert.ToDateTime(dr["CreationDate"])
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexion: {ex.Message}");
        }

        return null;
    }

    public int Insertar(Product producto)
    {
        try
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("dbo.SP_InsertProduct", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@Code", SqlDbType.VarChar) { Value = producto.Code });
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = producto.Name });
            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar) { Value = (object?)producto.Description ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Value = producto.Price });
            cmd.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = producto.Stock });
            cmd.Parameters.Add(new SqlParameter("@Category", SqlDbType.VarChar) { Value = (object?)producto.Category ?? DBNull.Value });

            var result = cmd.ExecuteScalar();
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : -1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return -1;
        }
    }

    public int Actualizar(Product producto)
    {
        try
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("dbo.SP_UpdateProduct", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = producto.ProductId });
            cmd.Parameters.Add(new SqlParameter("@Code", SqlDbType.VarChar) { Value = producto.Code });
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = producto.Name });
            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar) { Value = (object?)producto.Description ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Value = producto.Price });
            cmd.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = producto.Stock });
            cmd.Parameters.Add(new SqlParameter("@Category", SqlDbType.VarChar) { Value = (object?)producto.Category ?? DBNull.Value });

            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
    }

    public int Eliminar(int id)
    {
        try
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("dbo.SP_DeleteProduct", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = id });

            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
    }
}
