-- Script de verificación y reparación para la base de datos Practica2
USE Practica2;
GO

-- Verificar si existe la tabla Products
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    PRINT 'Creando tabla Products...';
    CREATE TABLE Products (
        ProductId INT IDENTITY(1,1) PRIMARY KEY,
        Code VARCHAR(50) NOT NULL UNIQUE,
        Name VARCHAR(100) NOT NULL,
        Description VARCHAR(500) NULL,
        Price DECIMAL(10,2) NOT NULL DEFAULT 0,
        Stock INT NOT NULL DEFAULT 0,
        Category VARCHAR(50) NULL,
        Flag BIT NOT NULL DEFAULT 0,
        CreationDate DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Crear o actualizar los stored procedures
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_ListProducts')
    DROP PROCEDURE SP_ListProducts;
GO

CREATE PROCEDURE SP_ListProducts
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Products WHERE Flag = 0 ORDER BY Name;
END
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_SearchProducts')
    DROP PROCEDURE SP_SearchProducts;
GO

CREATE PROCEDURE SP_SearchProducts
    @Filter VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Products 
    WHERE Flag = 0 
    AND (Code LIKE '%' + @Filter + '%' 
         OR Name LIKE '%' + @Filter + '%' 
         OR Category LIKE '%' + @Filter + '%')
    ORDER BY Name;
END
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_GetProductById')
    DROP PROCEDURE SP_GetProductById;
GO

CREATE PROCEDURE SP_GetProductById
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Products WHERE ProductId = @ProductId;
END
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_InsertProduct')
    DROP PROCEDURE SP_InsertProduct;
GO

CREATE PROCEDURE SP_InsertProduct
    @Code VARCHAR(50),
    @Name VARCHAR(100),
    @Description VARCHAR(500) = NULL,
    @Price DECIMAL(10,2),
    @Stock INT,
    @Category VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Products (Code, Name, Description, Price, Stock, Category, Flag, CreationDate)
    VALUES (@Code, @Name, @Description, @Price, @Stock, @Category, 0, GETDATE());
    SELECT SCOPE_IDENTITY() AS ProductId;
END
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_UpdateProduct')
    DROP PROCEDURE SP_UpdateProduct;
GO

CREATE PROCEDURE SP_UpdateProduct
    @ProductId INT,
    @Code VARCHAR(50),
    @Name VARCHAR(100),
    @Description VARCHAR(500) = NULL,
    @Price DECIMAL(10,2),
    @Stock INT,
    @Category VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Products 
    SET Code = @Code, Name = @Name, Description = @Description,
        Price = @Price, Stock = @Stock, Category = @Category
    WHERE ProductId = @ProductId AND Flag = 0;
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_DeleteProduct')
    DROP PROCEDURE SP_DeleteProduct;
GO

CREATE PROCEDURE SP_DeleteProduct
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Products SET Flag = 1 WHERE ProductId = @ProductId AND Flag = 0;
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

PRINT 'Base de datos configurada correctamente';
GO
