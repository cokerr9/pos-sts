/*
==============================================================================
 DATABASE CREATION & SETUP SCRIPT FOR OKE_MART
 Compatible with Microsoft SQL Server (2012 / 2014 / 2016 / 2019 / 2022 / 2025)
==============================================================================
*/

-- 1. Create Database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Oke_db')
BEGIN
    CREATE DATABASE Oke_db;
    PRINT 'Database [Oke_db] created successfully.';
END
ELSE
BEGIN
    PRINT 'Database [Oke_db] already exists.';
END
GO

USE Oke_db;
GO

/*
==============================================================================
 2. CREATE TABLES
==============================================================================
*/

-- Table 1: tblUsers
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'tblUsers')
BEGIN
    CREATE TABLE tblUsers (
        UserId INT IDENTITY(1,1) PRIMARY KEY,
        UserName NVARCHAR(100) NOT NULL UNIQUE,
        Password NVARCHAR(100) NOT NULL,
        UserType NVARCHAR(50) NOT NULL -- 'Super_Admin', 'Admin', 'User'
    );
    PRINT 'Table [tblUsers] created.';
END
GO

-- Table 2: tblCategory
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'tblCategory')
BEGIN
    CREATE TABLE tblCategory (
        CategoryId INT IDENTITY(1,1) PRIMARY KEY,
        CategoryName NVARCHAR(100) NOT NULL UNIQUE,
        Description NVARCHAR(255) NULL,
        Status NVARCHAR(50) NULL DEFAULT 'Active',
        UserId INT NULL,
        Create_At DATETIME NULL DEFAULT GETDATE(),
        Update_At DATETIME NULL,
        CONSTRAINT FK_tblCategory_tblUsers FOREIGN KEY (UserId) REFERENCES tblUsers(UserId) ON DELETE SET NULL
    );
    PRINT 'Table [tblCategory] created.';
END
GO

-- Table 3: tblSupplier
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'tblSupplier')
BEGIN
    CREATE TABLE tblSupplier (
        SupplierId INT IDENTITY(1,1) PRIMARY KEY,
        CompanyName NVARCHAR(150) NOT NULL,
        SupplierName NVARCHAR(150) NOT NULL,
        PhoneNumber NVARCHAR(50) NULL,
        Address NVARCHAR(255) NULL,
        UserId INT NULL,
        UpdateBy INT NULL,
        Update_At DATETIME NULL,
        CONSTRAINT FK_tblSupplier_UserCreate FOREIGN KEY (UserId) REFERENCES tblUsers(UserId) ON DELETE SET NULL,
        CONSTRAINT FK_tblSupplier_UserUpdate FOREIGN KEY (UpdateBy) REFERENCES tblUsers(UserId) ON DELETE NO ACTION
    );
    PRINT 'Table [tblSupplier] created.';
END
GO

-- Table 4: tblProduct
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'tblProduct')
BEGIN
    CREATE TABLE tblProduct (
        ProductId INT IDENTITY(1,1) PRIMARY KEY,
        CategoryId INT NOT NULL,
        Barcode NVARCHAR(100) NULL,
        ProductName NVARCHAR(200) NOT NULL,
        SupplierId INT NOT NULL,
        UnitPrice FLOAT NOT NULL DEFAULT 0,
        SalePrice FLOAT NOT NULL DEFAULT 0,
        QtyInStock INT NOT NULL DEFAULT 0,
        ExpireDate DATE NULL,
        Status NVARCHAR(50) NULL DEFAULT 'Yes',
        Photo VARBINARY(MAX) NULL,
        UserCreate INT NULL,
        Create_At DATETIME NULL DEFAULT GETDATE(),
        UserUpdate INT NULL,
        Update_At DATETIME NULL,
        CONSTRAINT FK_tblProduct_tblCategory FOREIGN KEY (CategoryId) REFERENCES tblCategory(CategoryId) ON DELETE CASCADE,
        CONSTRAINT FK_tblProduct_tblSupplier FOREIGN KEY (SupplierId) REFERENCES tblSupplier(SupplierId) ON DELETE CASCADE,
        CONSTRAINT FK_tblProduct_UserCreate FOREIGN KEY (UserCreate) REFERENCES tblUsers(UserId) ON DELETE SET NULL,
        CONSTRAINT FK_tblProduct_UserUpdate FOREIGN KEY (UserUpdate) REFERENCES tblUsers(UserId) ON DELETE NO ACTION
    );
    PRINT 'Table [tblProduct] created.';
END
GO

-- Table 5: tblPurchase
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'tblPurchase')
BEGIN
    CREATE TABLE tblPurchase (
        PurchaseId INT IDENTITY(1,1) PRIMARY KEY,
        ProductId INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
        Quantity INT NOT NULL DEFAULT 0,
        TotalPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
        PurchaseDate DATE NULL DEFAULT GETDATE(),
        UserId INT NULL,
        CONSTRAINT FK_tblPurchase_tblProduct FOREIGN KEY (ProductId) REFERENCES tblProduct(ProductId) ON DELETE CASCADE,
        CONSTRAINT FK_tblPurchase_tblUsers FOREIGN KEY (UserId) REFERENCES tblUsers(UserId) ON DELETE SET NULL
    );
    PRINT 'Table [tblPurchase] created.';
END
GO

-- Table 6: tblSale
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'tblSale')
BEGIN
    CREATE TABLE tblSale (
        SaleId INT IDENTITY(1,1) PRIMARY KEY,
        ReceiptId INT NULL,
        ProductId INT NOT NULL,
        Price DECIMAL(18,2) NOT NULL DEFAULT 0,
        Qty INT NOT NULL DEFAULT 0,
        TotalPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
        SaleDate DATE NULL DEFAULT GETDATE(),
        UserId INT NULL,
        CONSTRAINT FK_tblSale_tblProduct FOREIGN KEY (ProductId) REFERENCES tblProduct(ProductId) ON DELETE CASCADE,
        CONSTRAINT FK_tblSale_tblUsers FOREIGN KEY (UserId) REFERENCES tblUsers(UserId) ON DELETE SET NULL
    );
    PRINT 'Table [tblSale] created.';
END
GO

-- Table: tblReceipt
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'tblReceipt')
BEGIN
    CREATE TABLE tblReceipt (
        ReceiptId INT IDENTITY(1,1) PRIMARY KEY,
        ReceiptNumber NVARCHAR(50) NULL,
        TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        Payment DECIMAL(18,2) NOT NULL DEFAULT 0,
        Change DECIMAL(18,2) NOT NULL DEFAULT 0,
        SaleDate DATETIME NOT NULL DEFAULT GETDATE(),
        UserId INT NULL,
        CONSTRAINT FK_tblReceipt_tblUsers FOREIGN KEY (UserId) REFERENCES tblUsers(UserId) ON DELETE SET NULL
    );
    PRINT 'Table [tblReceipt] created.';
END
GO

-- Table 7: tblSetting
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'tblSetting')
BEGIN
    CREATE TABLE tblSetting (
        SettingId INT IDENTITY(1,1) PRIMARY KEY,
        CompanyName NVARCHAR(150) NOT NULL,
        CompanyLogo VARBINARY(MAX) NULL
    );
    PRINT 'Table [tblSetting] created.';
END
GO

/*
==============================================================================
 3. CREATE DATABASE VIEWS
==============================================================================
*/

USE Oke_db;
GO

-- View 1: V_CategoryForProduct
CREATE OR ALTER VIEW V_CategoryForProduct AS
SELECT 
    CategoryId, 
    CategoryName,
    Status
FROM tblCategory;
GO

-- View 2: V_Supplier
CREATE OR ALTER VIEW V_Supplier AS
SELECT 
    SupplierId, 
    CompanyName, 
    SupplierName, 
    PhoneNumber, 
    Address, 
    UserId
FROM tblSupplier;
GO

-- View 3: V_SupplierForProduct
CREATE OR ALTER VIEW V_SupplierForProduct AS
SELECT 
    SupplierId, 
    SupplierName, 
    CompanyName
FROM tblSupplier;
GO

-- View 4: V_ProductDetails
-- Provides aliases for QtyInStock, QtyInStcok, and QuantityInStcok to match all DataGridView column bindings
CREATE OR ALTER VIEW V_ProductDetails AS
SELECT 
    p.ProductId,
    p.CategoryId,
    c.CategoryName,
    p.Barcode,
    p.ProductName,
    p.SupplierId,
    s.SupplierName,
    p.UnitPrice,
    p.SalePrice,
    p.QtyInStock,
    p.QtyInStock AS QtyInStcok,
    p.QtyInStock AS QuantityInStcok,
    p.QtyInStock AS QuantityInStock,
    p.ExpireDate,
    p.Status,
    p.Photo,
    p.UserCreate,
    p.Create_At,
    p.UserUpdate,
    p.Update_At
FROM tblProduct p
LEFT JOIN tblCategory c ON p.CategoryId = c.CategoryId
LEFT JOIN tblSupplier s ON p.SupplierId = s.SupplierId;
GO

-- View 5: V_Purchase
CREATE OR ALTER VIEW V_Purchase AS
SELECT 
    pu.PurchaseId,
    pu.ProductId,
    pr.ProductName,
    pu.UnitPrice,
    pu.Quantity,
    pu.TotalPrice,
    pu.PurchaseDate,
    pu.UserId
FROM tblPurchase pu
LEFT JOIN tblProduct pr ON pu.ProductId = pr.ProductId;
GO

-- View 6: v_ProductsForSale
CREATE OR ALTER VIEW v_ProductsForSale AS
SELECT 
    p.ProductId,
    c.CategoryName,
    p.Barcode,
    p.ProductName,
    p.SalePrice,
    p.QtyInStock,
    p.QtyInStock AS QuantityInStock,
    p.Photo,
    p.Status
FROM tblProduct p
LEFT JOIN tblCategory c ON p.CategoryId = c.CategoryId;
GO

/*
==============================================================================
 4. CREATE STORED PROCEDURES
==============================================================================
*/

USE Oke_db;
GO

-- Category Procedures
CREATE OR ALTER PROCEDURE InsertCategory
    @CategoryName NVARCHAR(100),
    @Description NVARCHAR(255) = NULL,
    @Status NVARCHAR(50) = 'Active',
    @Create_At DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tblCategory (CategoryName, Description, Status, Create_At)
    VALUES (@CategoryName, @Description, @Status, ISNULL(@Create_At, GETDATE()));
END
GO

CREATE OR ALTER PROCEDURE UpdateCategory
    @CategoryId INT,
    @CategoryName NVARCHAR(100),
    @Description NVARCHAR(255) = NULL,
    @Status NVARCHAR(50) = 'Active'
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tblCategory
    SET CategoryName = @CategoryName,
        Description = @Description,
        Status = @Status,
        Update_At = GETDATE()
    WHERE CategoryId = @CategoryId;
END
GO

CREATE OR ALTER PROCEDURE DeleteCategory
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tblCategory WHERE CategoryId = @CategoryId;
END
GO

-- Product Procedures
CREATE OR ALTER PROCEDURE InsertProduct
    @CategoryId INT,
    @Barcode NVARCHAR(100) = NULL,
    @ProductName NVARCHAR(200),
    @SupplierId INT,
    @UnitPrice FLOAT = 0,
    @SalePrice FLOAT = 0,
    @QtyinStock INT = 0,
    @ExpireDate DATE = NULL,
    @Status NVARCHAR(50) = 'Yes',
    @Photo VARBINARY(MAX) = NULL,
    @UserCreate INT = NULL,
    @Create_At DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    -- Safeguard: If @UserCreate is <= 0 or not found in tblUsers, assign default admin
    IF (@UserCreate <= 0 OR NOT EXISTS (SELECT 1 FROM tblUsers WHERE UserId = @UserCreate))
    BEGIN
        SET @UserCreate = (SELECT TOP 1 UserId FROM tblUsers ORDER BY UserId ASC);
    END

    INSERT INTO tblProduct (
        CategoryId, Barcode, ProductName, SupplierId, 
        UnitPrice, SalePrice, QtyInStock, ExpireDate, 
        Status, Photo, UserCreate, Create_At
    )
    VALUES (
        @CategoryId, @Barcode, @ProductName, @SupplierId, 
        @UnitPrice, @SalePrice, @QtyinStock, @ExpireDate, 
        @Status, @Photo, @UserCreate, ISNULL(@Create_At, GETDATE())
    );
END
GO

CREATE OR ALTER PROCEDURE UpdateProduct
    @ProductId INT,
    @CategoryId INT,
    @Barcode NVARCHAR(100) = NULL,
    @ProductName NVARCHAR(200),
    @SupplierId INT,
    @UnitPrice FLOAT = 0,
    @SalePrice FLOAT = 0,
    @QtyinStock INT = 0,
    @ExpireDate DATE = NULL,
    @Status NVARCHAR(50) = 'Yes',
    @Photo VARBINARY(MAX) = NULL,
    @UserUpdate INT = NULL,
    @Update_At DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    -- Safeguard: If @UserUpdate is <= 0 or not found in tblUsers, assign default admin
    IF (@UserUpdate <= 0 OR NOT EXISTS (SELECT 1 FROM tblUsers WHERE UserId = @UserUpdate))
    BEGIN
        SET @UserUpdate = (SELECT TOP 1 UserId FROM tblUsers ORDER BY UserId ASC);
    END

    UPDATE tblProduct
    SET CategoryId = @CategoryId,
        Barcode = @Barcode,
        ProductName = @ProductName,
        SupplierId = @SupplierId,
        UnitPrice = @UnitPrice,
        SalePrice = @SalePrice,
        QtyInStock = @QtyinStock,
        ExpireDate = @ExpireDate,
        Status = @Status,
        Photo = @Photo,
        UserUpdate = @UserUpdate,
        Update_At = ISNULL(@Update_At, GETDATE())
    WHERE ProductId = @ProductId;
END
GO

CREATE OR ALTER PROCEDURE DeleteProduct
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tblProduct WHERE ProductId = @ProductId;
END
GO

-- Supplier Procedures
CREATE OR ALTER PROCEDURE InsertSupplier
    @CompanyName NVARCHAR(150),
    @SupplierName NVARCHAR(150),
    @PhoneNumber NVARCHAR(50) = NULL,
    @Address NVARCHAR(255) = NULL,
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    -- Safeguard: If @UserId is <= 0 or not found in tblUsers, assign default admin or NULL
    IF (@UserId <= 0 OR NOT EXISTS (SELECT 1 FROM tblUsers WHERE UserId = @UserId))
    BEGIN
        SET @UserId = (SELECT TOP 1 UserId FROM tblUsers ORDER BY UserId ASC);
    END

    INSERT INTO tblSupplier (CompanyName, SupplierName, PhoneNumber, Address, UserId)
    VALUES (@CompanyName, @SupplierName, @PhoneNumber, @Address, @UserId);
END
GO

CREATE OR ALTER PROCEDURE UpdateSupplier
    @SupplierId INT,
    @CompanyName NVARCHAR(150),
    @SupplierName NVARCHAR(150),
    @PhoneNumber NVARCHAR(50) = NULL,
    @Address NVARCHAR(255) = NULL,
    @UpdateBy INT = NULL,
    @Update_At DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    -- Safeguard: If @UpdateBy is <= 0 or not found in tblUsers, assign default admin or NULL
    IF (@UpdateBy <= 0 OR NOT EXISTS (SELECT 1 FROM tblUsers WHERE UserId = @UpdateBy))
    BEGIN
        SET @UpdateBy = (SELECT TOP 1 UserId FROM tblUsers ORDER BY UserId ASC);
    END

    UPDATE tblSupplier
    SET CompanyName = @CompanyName,
        SupplierName = @SupplierName,
        PhoneNumber = @PhoneNumber,
        Address = @Address,
        UpdateBy = @UpdateBy,
        Update_At = ISNULL(@Update_At, GETDATE())
    WHERE SupplierId = @SupplierId;
END
GO

CREATE OR ALTER PROCEDURE DeleteSupplier
    @SupplierId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tblSupplier WHERE SupplierId = @SupplierId;
END
GO

-- Purchase Procedures
CREATE OR ALTER PROCEDURE InsertPurchase
    @ProductId INT,
    @UnitPrice DECIMAL(18,2),
    @Quantity INT,
    @TotalPrice DECIMAL(18,2),
    @PurchaseDate DATE = NULL,
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    -- Safeguard: If @UserId is <= 0 or not found in tblUsers, assign default admin
    IF (@UserId <= 0 OR NOT EXISTS (SELECT 1 FROM tblUsers WHERE UserId = @UserId))
    BEGIN
        SET @UserId = (SELECT TOP 1 UserId FROM tblUsers ORDER BY UserId ASC);
    END

    INSERT INTO tblPurchase (ProductId, UnitPrice, Quantity, TotalPrice, PurchaseDate, UserId)
    VALUES (@ProductId, @UnitPrice, @Quantity, @TotalPrice, ISNULL(@PurchaseDate, CAST(GETDATE() AS DATE)), @UserId);

    -- Automatically update stock quantity in tblProduct
    UPDATE tblProduct
    SET QtyInStock = QtyInStock + @Quantity,
        UnitPrice = @UnitPrice
    WHERE ProductId = @ProductId;
END
GO

CREATE OR ALTER PROCEDURE UpdatePurchase
    @PurchaseId INT,
    @ProductId INT,
    @UnitPrice DECIMAL(18,2),
    @Quantity INT,
    @TotalPrice DECIMAL(18,2),
    @PurchaseDate DATE = NULL,
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    -- Safeguard: If @UserId is <= 0 or not found in tblUsers, assign default admin
    IF (@UserId <= 0 OR NOT EXISTS (SELECT 1 FROM tblUsers WHERE UserId = @UserId))
    BEGIN
        SET @UserId = (SELECT TOP 1 UserId FROM tblUsers ORDER BY UserId ASC);
    END

    DECLARE @OldQuantity INT = 0;
    DECLARE @OldProductId INT = 0;

    SELECT @OldQuantity = Quantity, @OldProductId = ProductId 
    FROM tblPurchase 
    WHERE PurchaseId = @PurchaseId;

    UPDATE tblPurchase
    SET ProductId = @ProductId,
        UnitPrice = @UnitPrice,
        Quantity = @Quantity,
        TotalPrice = @TotalPrice,
        PurchaseDate = ISNULL(@PurchaseDate, CAST(GETDATE() AS DATE)),
        UserId = @UserId
    WHERE PurchaseId = @PurchaseId;

    -- Adjust stock difference
    IF (@OldProductId = @ProductId)
    BEGIN
        UPDATE tblProduct 
        SET QtyInStock = QtyInStock - @OldQuantity + @Quantity 
        WHERE ProductId = @ProductId;
    END
END
GO

CREATE OR ALTER PROCEDURE DeletePurchase
    @PurchaseId INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @OldQuantity INT = 0;
    DECLARE @OldProductId INT = 0;

    SELECT @OldQuantity = Quantity, @OldProductId = ProductId 
    FROM tblPurchase 
    WHERE PurchaseId = @PurchaseId;

    DELETE FROM tblPurchase WHERE PurchaseId = @PurchaseId;

    -- Revert stock deduction
    UPDATE tblProduct 
    SET QtyInStock = CASE WHEN QtyInStock >= @OldQuantity THEN QtyInStock - @OldQuantity ELSE 0 END
    WHERE ProductId = @OldProductId;
END
GO

-- Receipt Procedures
CREATE OR ALTER PROCEDURE InsertReceipt
    @ReceiptNumber NVARCHAR(50),
    @TotalAmount DECIMAL(18,2),
    @Payment DECIMAL(18,2),
    @Change DECIMAL(18,2),
    @SaleDate DATETIME = NULL,
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF (@UserId <= 0 OR NOT EXISTS (SELECT 1 FROM tblUsers WHERE UserId = @UserId))
    BEGIN
        SET @UserId = (SELECT TOP 1 UserId FROM tblUsers ORDER BY UserId ASC);
    END

    INSERT INTO tblReceipt (ReceiptNumber, TotalAmount, Payment, Change, SaleDate, UserId)
    VALUES (@ReceiptNumber, @TotalAmount, @Payment, @Change, ISNULL(@SaleDate, GETDATE()), @UserId);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewReceiptId;
END
GO

-- Sale Procedures
CREATE OR ALTER PROCEDURE InsertSale
    @ReceiptId INT = NULL,
    @ProductId INT,
    @Price DECIMAL(18,2),
    @Qty INT,
    @TotalPrice DECIMAL(18,2),
    @SaleDate DATE = NULL,
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    -- Safeguard: If @UserId is <= 0 or not found in tblUsers, assign default admin
    IF (@UserId <= 0 OR NOT EXISTS (SELECT 1 FROM tblUsers WHERE UserId = @UserId))
    BEGIN
        SET @UserId = (SELECT TOP 1 UserId FROM tblUsers ORDER BY UserId ASC);
    END

    INSERT INTO tblSale (ReceiptId, ProductId, Price, Qty, TotalPrice, SaleDate, UserId)
    VALUES (@ReceiptId, @ProductId, @Price, @Qty, @TotalPrice, ISNULL(@SaleDate, CAST(GETDATE() AS DATE)), @UserId);

    -- Reduce stock quantity in tblProduct
    UPDATE tblProduct
    SET QtyInStock = CASE WHEN QtyInStock >= @Qty THEN QtyInStock - @Qty ELSE 0 END
    WHERE ProductId = @ProductId;
END
GO

-- Setting Procedures
CREATE OR ALTER PROCEDURE InsertSetting
    @CompanyName NVARCHAR(150),
    @CompanyLogo VARBINARY(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tblSetting (CompanyName, CompanyLogo)
    VALUES (@CompanyName, @CompanyLogo);
END
GO

CREATE OR ALTER PROCEDURE UpdateSetting
    @SettingId INT,
    @CompanyName NVARCHAR(150),
    @CompanyLogo VARBINARY(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tblSetting
    SET CompanyName = @CompanyName,
        CompanyLogo = @CompanyLogo
    WHERE SettingId = @SettingId;
END
GO

-- User Procedures
CREATE OR ALTER PROCEDURE InsertUser
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100),
    @UserType NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tblUsers (UserName, Password, UserType)
    VALUES (@UserName, @Password, @UserType);
END
GO

CREATE OR ALTER PROCEDURE UpdateUsers
    @UserId INT,
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100),
    @UserType NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tblUsers
    SET UserName = @UserName,
        Password = @Password,
        UserType = @UserType
    WHERE UserId = @UserId;
END
GO

CREATE OR ALTER PROCEDURE DeleteUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tblUsers WHERE UserId = @UserId;
END
GO

/*
==============================================================================
 5. SEED INITIAL DEFAULT DATA
==============================================================================
*/

USE Oke_db;
GO

-- Default Users for Login
IF NOT EXISTS (SELECT * FROM tblUsers WHERE UserName = 'admin')
BEGIN
    INSERT INTO tblUsers (UserName, Password, UserType)
    VALUES ('admin', '123', 'Super_Admin');
    PRINT 'Default Super_Admin account created: admin / 123';
END
GO

IF NOT EXISTS (SELECT * FROM tblUsers WHERE UserName = 'manager')
BEGIN
    INSERT INTO tblUsers (UserName, Password, UserType)
    VALUES ('manager', '123', 'Admin');
    PRINT 'Default Admin account created: manager / 123';
END
GO

IF NOT EXISTS (SELECT * FROM tblUsers WHERE UserName = 'user')
BEGIN
    INSERT INTO tblUsers (UserName, Password, UserType)
    VALUES ('user', '123', 'User');
    PRINT 'Default User account created: user / 123';
END
GO

-- Default Company Setting (SettingId = 1)
IF NOT EXISTS (SELECT * FROM tblSetting WHERE SettingId = 1)
BEGIN
    SET IDENTITY_INSERT tblSetting ON;
    INSERT INTO tblSetting (SettingId, CompanyName, CompanyLogo)
    VALUES (1, 'OKE MART', NULL);
    SET IDENTITY_INSERT tblSetting OFF;
    PRINT 'Default Company Setting created: SettingId = 1, CompanyName = OKE MART';
END
GO

PRINT '==============================================================================';
PRINT 'OKE_MART DATABASE SETUP COMPLETED SUCCESSFULLY!';
PRINT '==============================================================================';
