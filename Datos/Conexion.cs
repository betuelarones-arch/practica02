using Microsoft.Data.SqlClient;

namespace Datos;

public class Conexion
{
    private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Practica2;Integrated Security=True;TrustServerCertificate=True;";

    public SqlConnection ObtenerConexion()
    {
        return new SqlConnection(connectionString);
    }
}
