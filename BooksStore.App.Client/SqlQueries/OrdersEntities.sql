IF OBJECT_ID('dbo.CreateOrderTables', 'P') IS NOT NULL
    DROP PROCEDURE dbo.CreateOrderTables
GO
CREATE PROCEDURE CreateOrderTables AS
BEGIN
    CREATE TABLE dbo.Customers
    (
        Id      BIGINT IDENTITY (1,1)
            CONSTRAINT PK_Customers PRIMARY KEY,
        Name    NVARCHAR(100),
        Address NVARCHAR(200),
        State   NVARCHAR(100),
        ZipCode INT,
        Created DATETIME NOT NULL
            CONSTRAINT DFT_Customers_created DEFAULT (GETDATE()),
        Updated DATETIME NULL
    )

    CREATE TABLE dbo.Payments
    (
        Id           BIGINT IDENTITY (1,1)
            CONSTRAINT PK_Payments PRIMARY KEY,
        CardNumber   NVARCHAR(19),
        CardExpiry   NVARCHAR(5),
        SecurityCode INT,
        Total        DECIMAL(8, 2),
        AuthCode     NVARCHAR(20),
        Created      DATETIME NOT NULL
            CONSTRAINT DFT_Payments_created DEFAULT (GETDATE()),
        Updated      DATETIME NULL
    )

    CREATE TABLE dbo.Orders
    (
        Id         BIGINT IDENTITY (1,1)
            CONSTRAINT PK_Orders PRIMARY KEY,
        Shipped    BIT,
        CustomerId BIGINT
            CONSTRAINT FK_Orders_Customers_Id
                REFERENCES Customers (Id)
                ON DELETE SET NULL,
        PaymentId  BIGINT
            CONSTRAINT FK_Orders_Payment_Id
                REFERENCES Payments (Id)
                ON DELETE SET NULL,
        Created    DATETIME NOT NULL
            CONSTRAINT DFT_Orders_created DEFAULT (GETDATE()),
        Updated    DATETIME NULL
    )

    CREATE TABLE dbo.CartLines
    (
        Id         BIGINT IDENTITY (1,1)
            CONSTRAINT PK_CartLines PRIMARY KEY,
        BookId    BIGINT,
        ItemName     NVARCHAR(400),
        Price        DECIMAL(8, 2),
        Quantity INT,
        OrderId BIGINT
            CONSTRAINT FK_CartLines_Orders_Id
                REFERENCES Customers (Id)
                ON DELETE SET NULL,
        Created    DATETIME NOT NULL
            CONSTRAINT DFT_CartLines_created DEFAULT (GETDATE()),
        Updated    DATETIME NULL
    )
END
GO

IF OBJECT_ID('dbo.DeleteOrderTables', 'P') IS NOT NULL
    DROP PROCEDURE dbo.DeleteOrderTables
GO
CREATE PROCEDURE DeleteOrderTables AS
BEGIN
    ALTER TABLE dbo.Customers
        DROP CONSTRAINT DFT_Customers_created;
    ALTER TABLE dbo.Payments
        DROP CONSTRAINT DFT_Payments_created;
    ALTER TABLE dbo.Orders
        DROP CONSTRAINT DFT_Orders_created,
            FK_Orders_Customers_Id, FK_Orders_Payment_Id;
    ALTER TABLE dbo.CartLines
        DROP CONSTRAINT FK_CartLines_Orders_Id,
            DFT_CartLines_created

    DROP TABLE dbo.Customers;
    DROP TABLE dbo.Payments;
    DROP TABLE dbo.Orders;
    DROP TABLE dbo.CartLines;
END