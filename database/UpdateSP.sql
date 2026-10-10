-- Script para actualizar SP_UpdateProduct
-- Ejecutar en SSMS para actualizar el stored procedure

USE Practica2;
GO

-- Actualizar SP_UpdateProduct para que devuelva correctamente el número de filas afectadas
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

PRINT 'SP_UpdateProduct actualizado correctamente.';
GO

-- Actualizar SP_DeleteProduct también
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.SP_DeleteProduct') AND type = 'P')
    DROP PROCEDURE dbo.SP_DeleteProduct;
GO

CREATE PROCEDURE dbo.SP_DeleteProduct
    @ProductId INT
AS
BEGIN
    UPDATE dbo.Products
    SET Flag = 1
    WHERE ProductId = @ProductId
      AND Flag = 0;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

PRINT 'SP_DeleteProduct actualizado correctamente.';
GO
