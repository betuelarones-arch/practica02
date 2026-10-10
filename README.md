# Módulo Productos - Aplicación de escritorio WPF

Aplicación de escritorio WPF (.NET 10) para gestión de productos con arquitectura en capas (Presentación / Negocio / Datos / Entidades) y acceso a SQL Server mediante ADO.NET.

## Estructura del proyecto

```
practica2/
├── practica2.slnx          → Solución
├── practica2/               → Aplicación WPF (presentación)
├── Negocio/                 → Capa de negocio (validaciones y servicios)
├── Datos/                   → Capa de datos (ADO.NET puro)
├── Entidades/               → Modelos de dominio
└── database/
    └── ScriptBD.sql         → Script SQL (tabla + procedimientos almacenados)
```

## Requisitos previos

- **.NET 10 SDK** instalado.
- **SQL Server** (LocalDB, Express o Developer Edition) accesible.
- **SQL Server Management Studio (SSMS)** para ejecutar el script manualmente.

## Configuración de la base de datos

### 1. Ejecutar el script SQL

Abre SSMS y conéctate a tu instancia de SQL Server. Ejecuta manualmente el archivo:

```
database/ScriptBD.sql
```

El script realiza las siguientes operaciones **en orden**:

1. Crea la base de datos `Practica2` si no existe.
2. Crea la tabla `dbo.Products` si no existe.
3. Crea (o recrea) los 6 procedimientos almacenados:
   - `SP_ListProducts` — Lista los productos activos (Flag = 0).
   - `SP_SearchProducts` — Busca por código, nombre, categoría o descripción.
   - `SP_GetProductById` — Obtiene un producto por ID.
   - `SP_InsertProduct` — Inserta un nuevo producto.
   - `SP_UpdateProduct` — Actualiza un producto existente (solo si no está eliminado).
   - `SP_DeleteProduct` — Eliminación lógica (cambia Flag a 1).
4. Inserta 10 productos de prueba usando el procedimiento `SP_InsertProduct`.
5. Incluye ejemplos de ejecución comentados.

**Nota:** La tabla `Products` y sus columnas están en inglés por buenas prácticas, mientras que los procedimientos almacenados y sus parámetros mantienen nombres en español.

### 2. Verificar la conexión

La cadena de conexión está definida en `practica2/App.xaml.cs`:

```
Server=.;Database=Practica2;Integrated Security=True;TrustServerCertificate=True
```

Ajusta `Server=.` si tu instancia de SQL Server tiene un nombre diferente (ej: `Server=localhost\\SQLEXPRESS`).

## Ejecutar la aplicación

Desde la raíz del repositorio:

```bash
dotnet run --project practica2
```

O abre la solución en Visual Studio y establece `practica2` como proyecto de inicio.

## Funcionalidades implementadas

| Función | Descripción |
|---|---|
| **Listar** | Muestra todos los productos activos en un DataGrid |
| **Buscar** | Filtra por código, nombre, categoría o descripción |
| **Registrar** | Formulario para crear un nuevo producto |
| **Editar** | Modifica los datos de un producto activo |
| **Detalle** | Muestra toda la información de un producto |
| **Eliminar** | Eliminación lógica (no borra físicamente) |

## Eliminación lógica

- La tabla usa la columna `Flag BIT` (0 = activo, 1 = eliminado).
- Los listados y búsquedas **excluyen** los productos eliminados.
- No se permite actualizar ni re-eliminar productos ya inactivos.

## Estructura de la base de datos

### Tabla `dbo.Products`

| Columna | Tipo | Descripción |
|---|---|---|
| ProductId | INT IDENTITY | Clave primaria |
| Code | VARCHAR(50) | Código único del producto |
| Name | VARCHAR(100) | Nombre del producto |
| Description | VARCHAR(500) | Descripción (opcional) |
| Price | DECIMAL(10,2) | Precio |
| Stock | INT | Cantidad en inventario |
| Category | VARCHAR(50) | Categoría (opcional) |
| Flag | BIT | Eliminación lógica (0 = activo, 1 = eliminado) |
| CreationDate | DATETIME | Fecha de creación |

## Capas

- **Entidades**: `Product.cs` — modelo de dominio.
- **Datos**: `ConexionFactory` + `ProductRepository` — ADO.NET con `SqlConnection`, `SqlCommand`, `SqlParameter`, `SqlDataReader`.
- **Negocio**: `ProductService` — validaciones (campos obligatorios, rangos, estado activo).
- **Presentación (WPF)**: `MainWindow.xaml` + `MainWindow.xaml.cs` — UI y lógica de interacción.

## Flujo de trabajo

```
MainWindow (WPF)
    → ProductService (Negocio)
        → ProductRepository (Datos/ADO.NET)
            → Stored Procedures → SQL Server
```
# practica02
# practica02
