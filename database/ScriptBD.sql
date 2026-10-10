-- =============================================
-- ScriptBD.sql - Product Module
-- Base de datos: Practica2
-- =============================================

-- Crear base de datos (si no existe)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Practica2')
BEGIN
    CREATE DATABASE Practica2;
END
GO

USE Practica2;
GO

-- =============================================
-- Tabla: dbo.Products
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Products') AND type = 'U')
BEGIN
    CREATE TABLE dbo.Products (
        ProductId      INT IDENTITY(1,1) PRIMARY KEY,
        Code           VARCHAR(50)   NOT NULL UNIQUE,
        Name           VARCHAR(100)  NOT NULL,
        Description    VARCHAR(500)  NULL,
        Price          DECIMAL(10,2) NOT NULL DEFAULT 0,
        Stock          INT           NOT NULL DEFAULT 0,
        Category       VARCHAR(50)   NULL,
        Flag           BIT           NOT NULL DEFAULT 0,
        CreationDate   DATETIME      NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Tabla dbo.Products creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla dbo.Products ya existe.';
END
GO

-- =============================================
-- Procedimiento: SP_ListProducts
-- Descripción: Lista todos los productos activos (Flag = 0)
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.SP_ListProducts') AND type = 'P')
    DROP PROCEDURE dbo.SP_ListProducts;
GO

CREATE PROCEDURE dbo.SP_ListProducts
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ProductId, Code, Name, Description, Price, Stock, Category, Flag, CreationDate
    FROM dbo.Products
    WHERE Flag = 0
    ORDER BY Name ASC;
END
GO

PRINT 'Procedimiento SP_ListProducts creado correctamente.';
GO

-- =============================================
-- Procedimiento: SP_SearchProducts
-- Descripción: Busca productos activos por filtro de texto
-- Parámetros: @Filter - Texto a buscar en Code, Name, Category o Description
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.SP_SearchProducts') AND type = 'P')
    DROP PROCEDURE dbo.SP_SearchProducts;
GO

CREATE PROCEDURE dbo.SP_SearchProducts
    @Filter VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ProductId, Code, Name, Description, Price, Stock, Category, Flag, CreationDate
    FROM dbo.Products
    WHERE Flag = 0
      AND (
            Code LIKE '%' + @Filter + '%'
         OR Name LIKE '%' + @Filter + '%'
         OR Category LIKE '%' + @Filter + '%'
         OR Description LIKE '%' + @Filter + '%'
          )
    ORDER BY Name ASC;
END
GO

PRINT 'Procedimiento SP_SearchProducts creado correctamente.';
GO

-- =============================================
-- Procedimiento: SP_GetProductById
-- Descripción: Obtiene un producto por su identificador
-- Parámetros: @ProductId - Identificador del producto
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.SP_GetProductById') AND type = 'P')
    DROP PROCEDURE dbo.SP_GetProductById;
GO

CREATE PROCEDURE dbo.SP_GetProductById
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ProductId, Code, Name, Description, Price, Stock, Category, Flag, CreationDate
    FROM dbo.Products
    WHERE ProductId = @ProductId;
END
GO

PRINT 'Procedimiento SP_GetProductById creado correctamente.';
GO

-- =============================================
-- Procedimiento: SP_InsertProduct
-- Descripción: Inserta un nuevo producto
-- Parámetros: Datos del producto
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.SP_InsertProduct') AND type = 'P')
    DROP PROCEDURE dbo.SP_InsertProduct;
GO

CREATE PROCEDURE dbo.SP_InsertProduct
    @Code        VARCHAR(50),
    @Name        VARCHAR(100),
    @Description VARCHAR(500) = NULL,
    @Price       DECIMAL(10,2),
    @Stock       INT,
    @Category    VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO dbo.Products (Code, Name, Description, Price, Stock, Category, Flag, CreationDate)
    VALUES (@Code, @Name, @Description, @Price, @Stock, @Category, 0, GETDATE());
    
    SELECT SCOPE_IDENTITY() AS ProductId;
END
GO

PRINT 'Procedimiento SP_InsertProduct creado correctamente.';
GO

-- =============================================
-- Procedimiento: SP_UpdateProduct
-- Descripción: Actualiza un producto existente (solo si no está eliminado)
-- Parámetros: @ProductId + datos del producto
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.SP_UpdateProduct') AND type = 'P')
    DROP PROCEDURE dbo.SP_UpdateProduct;
GO

CREATE PROCEDURE dbo.SP_UpdateProduct
    @ProductId   INT,
    @Code        VARCHAR(50),
    @Name        VARCHAR(100),
    @Description VARCHAR(500) = NULL,
    @Price       DECIMAL(10,2),
    @Stock       INT,
    @Category    VARCHAR(50) = NULL
AS
BEGIN
    UPDATE dbo.Products
    SET 
        Code = @Code,
        Name = @Name,
        Description = @Description,
        Price = @Price,
        Stock = @Stock,
        Category = @Category
    WHERE ProductId = @ProductId
      AND Flag = 0;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

PRINT 'Procedimiento SP_UpdateProduct creado correctamente.';
GO

-- =============================================
-- Procedimiento: SP_DeleteProduct
-- Descripción: Eliminación lógica (cambia Flag a 1)
-- Parámetros: @ProductId - Identificador del producto
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.SP_DeleteProduct') AND type = 'P')
    DROP PROCEDURE dbo.SP_DeleteProduct;
GO

CREATE PROCEDURE dbo.SP_DeleteProduct
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE dbo.Products
    SET Flag = 1
    WHERE ProductId = @ProductId
      AND Flag = 0;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

PRINT 'Procedimiento SP_DeleteProduct creado correctamente.';
GO

-- =============================================
-- Inserción de productos de prueba
-- Usando el procedimiento SP_InsertProduct
-- =============================================
EXEC dbo.SP_InsertProduct @Code = 'P001', @Name = 'Laptop HP ProBook', @Description = 'Laptop 14 pulgadas, Intel i5, 8GB RAM', @Price = 899.99, @Stock = 15, @Category = 'Electrónica';
EXEC dbo.SP_InsertProduct @Code = 'P002', @Name = 'Mouse Inalámbrico', @Description = 'Mouse ergonómico Bluetooth', @Price = 25.50, @Stock = 50, @Category = 'Accesorios';
EXEC dbo.SP_InsertProduct @Code = 'P003', @Name = 'Teclado Mecánico', @Description = 'Teclado RGB con switches azules', @Price = 75.00, @Stock = 30, @Category = 'Accesorios';
EXEC dbo.SP_InsertProduct @Code = 'P004', @Name = 'Monitor 24"', @Description = 'Monitor Full HD 144Hz', @Price = 249.99, @Stock = 20, @Category = 'Electrónica';
EXEC dbo.SP_InsertProduct @Code = 'P005', @Name = 'Silla Gamer', @Description = 'Silla ergonómica reclinable', @Price = 199.99, @Stock = 10, @Category = 'Mobiliario';
EXEC dbo.SP_InsertProduct @Code = 'P006', @Name = 'Audífonos Bluetooth', @Description = 'Audífonos con cancelación de ruido', @Price = 149.99, @Stock = 25, @Category = 'Audio';
EXEC dbo.SP_InsertProduct @Code = 'P007', @Name = 'Webcam HD', @Description = 'Webcam 1080p con micrófono', @Price = 59.99, @Stock = 40, @Category = 'Accesorios';
EXEC dbo.SP_InsertProduct @Code = 'P008', @Name = 'Disco Duro Externo 1TB', @Description = 'Disco USB 3.0 portátil', @Price = 69.99, @Stock = 35, @Category = 'Almacenamiento';
EXEC dbo.SP_InsertProduct @Code = 'P009', @Name = 'Impresora Multifuncional', @Description = 'Impresora láser a color', @Price = 299.99, @Stock = 8, @Category = 'Oficina';
EXEC dbo.SP_InsertProduct @Code = 'P010', @Name = 'Tablet 10"', @Description = 'Tablet Android 128GB', @Price = 349.99, @Stock = 12, @Category = 'Electrónica';

PRINT 'Se insertaron 10 productos de prueba correctamente.';
GO

-- =============================================
-- Ejemplos de ejecución de los procedimientos
-- =============================================

-- Listar todos los productos activos:
-- EXEC dbo.SP_ListProducts;

-- Buscar productos con filtro:
-- EXEC dbo.SP_SearchProducts @Filter = 'Mouse';

-- Obtener producto por ID:
-- EXEC dbo.SP_GetProductById @ProductId = 1;

-- Actualizar producto:
-- EXEC dbo.SP_UpdateProduct
--     @ProductId = 1,
--     @Code = 'P001',
--     @Name = 'Laptop HP ProBook 450',
--     @Description = 'Laptop 15.6 pulgadas, Intel i7, 16GB RAM',
--     @Price = 999.99,
--     @Stock = 12,
--     @Category = 'Electrónica';

-- Eliminar producto (lógico):
-- EXEC dbo.SP_DeleteProduct @ProductId = 1;
