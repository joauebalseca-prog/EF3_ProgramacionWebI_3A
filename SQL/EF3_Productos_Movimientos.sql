IF DB_ID('EF3ProductosDB') IS NULL
BEGIN
    CREATE DATABASE EF3ProductosDB;
END
GO

USE EF3ProductosDB;
GO

IF OBJECT_ID('dbo.MovimientosInventario', 'U') IS NOT NULL DROP TABLE dbo.MovimientosInventario;
IF OBJECT_ID('dbo.Productos', 'U') IS NOT NULL DROP TABLE dbo.Productos;
IF OBJECT_ID('dbo.Categorias', 'U') IS NOT NULL DROP TABLE dbo.Categorias;
GO

CREATE TABLE Categorias (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    Estado BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE Productos (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    IdCategoria INT NOT NULL,
    Nombre VARCHAR(120) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Productos_Categorias
        FOREIGN KEY (IdCategoria) REFERENCES Categorias(IdCategoria),
    CONSTRAINT CK_Productos_Stock CHECK (Stock >= 0),
    CONSTRAINT CK_Productos_Precio CHECK (Precio >= 0)
);
GO

CREATE TABLE MovimientosInventario (
    IdMovimiento INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto INT NOT NULL,
    Tipo VARCHAR(10) NOT NULL,
    Cantidad INT NOT NULL,
    Fecha DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Observacion VARCHAR(250) NULL,
    StockAnterior INT NOT NULL,
    StockResultante INT NOT NULL,
    CONSTRAINT FK_Movimientos_Productos
        FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto),
    CONSTRAINT CK_Movimientos_Tipo CHECK (Tipo IN ('ENTRADA','SALIDA')),
    CONSTRAINT CK_Movimientos_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_Movimientos_Stock CHECK (StockAnterior >= 0 AND StockResultante >= 0)
);
GO

INSERT INTO Categorias (Nombre, Descripcion, Estado) VALUES
('Computación', 'Accesorios y componentes', 1),
('Oficina', 'Artículos de oficina', 1),
('Audio', 'Equipos y accesorios de audio', 1);
GO

INSERT INTO Productos (IdCategoria, Nombre, Descripcion, Precio, Stock, Estado) VALUES
(1, 'Mouse inalámbrico', 'Mouse USB 2.4 GHz', 18.50, 12, 1),
(1, 'Teclado mecánico', 'Teclado USB', 42.00, 8, 1),
(2, 'Cuaderno universitario', '100 hojas', 3.75, 25, 1),
(3, 'Audífonos USB', 'Audífonos con micrófono', 29.90, 5, 1),
(1, 'Producto inactivo', 'Registro para pruebas', 10.00, 4, 0);
GO

INSERT INTO MovimientosInventario
(IdProducto, Tipo, Cantidad, Fecha, Observacion, StockAnterior, StockResultante)
VALUES
(1, 'ENTRADA', 2, DATEADD(DAY,-2,SYSDATETIME()), 'Compra inicial adicional', 10, 12),
(4, 'SALIDA', 1, DATEADD(DAY,-1,SYSDATETIME()), 'Entrega interna', 6, 5);
GO

SELECT * FROM Categorias;
SELECT * FROM Productos;
SELECT * FROM MovimientosInventario;
GO
