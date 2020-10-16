IF OBJECT_ID('dbo.CreateStoreTables', 'P') IS NOT NULL
    DROP PROCEDURE dbo.CreateStoreTables
GO
CREATE PROCEDURE CreateStoreTables AS
BEGIN
    CREATE TABLE dbo.Publishers
    (
        Id      BIGINT IDENTITY (1,1)
            CONSTRAINT PK_Publishers PRIMARY KEY,
        Name    NVARCHAR(100),
        Country NVARCHAR(100),
        Created DATETIME NOT NULL
            CONSTRAINT DFT_Publisher_created DEFAULT (GETDATE()),
        Updated DATETIME NULL
    );

    CREATE TABLE dbo.Categories
    (
        Id                 BIGINT IDENTITY (1, 1)
            CONSTRAINT PK_Categories PRIMARY KEY,
        Name               NVARCHAR(100),
        ParentId           BIGINT   NULL,
        ParentAndChildName NVARCHAR(300),
        Created            DATETIME NOT NULL
            CONSTRAINT DFT_Category_created DEFAULT (GETDATE()),
        Updated            DATETIME NULL
    );

    CREATE TABLE dbo.Books
    (
        Id            BIGINT IDENTITY (1,1)
            CONSTRAINT PK_Books PRIMARY KEY,
        Title         NVARCHAR(400),
        Authors       NVARCHAR(100),
        Year          INT           NOT NULL
            CONSTRAINT DFT_year DEFAULT (YEAR(GETDATE())),
        Language      NVARCHAR(50)
            CONSTRAINT DFT_language DEFAULT ('Russian'),
        PageCount     INT           NOT NULL,
        Description   NVARCHAR(MAX),
        PurchasePrice DECIMAL(8, 2) NOT NULL,
        RetailPrice   DECIMAL(8, 2) NOT NULL,
        BookCover     NVARCHAR(MAX),
        CategoryId    BIGINT
            CONSTRAINT FK_Books_Categories_CategoryId
                REFERENCES Categories (Id)
                ON DELETE SET NULL,
        PublisherId   BIGINT
            CONSTRAINT FK_Books_Publishers_PublisherId
                REFERENCES Publishers (Id)
                ON DELETE SET NULL,
        Created       DATETIME      NOT NULL
            CONSTRAINT DFT_Book_created DEFAULT (GETDATE()),
        Updated       DATETIME      NULL
    )
END
GO

IF OBJECT_ID('dbo.DeleteStoreTables', 'P') IS NOT NULL
    DROP PROCEDURE dbo.DeleteStoreTables
GO
CREATE PROCEDURE DeleteStoreTables AS
BEGIN
    ALTER TABLE dbo.Publishers
        DROP CONSTRAINT DFT_Publisher_created;
    ALTER TABLE dbo.Categories
        DROP CONSTRAINT DFT_Category_created,
            FK_Categories_Categories_ParentId;
    ALTER TABLE dbo.Books
        DROP CONSTRAINT DFT_year,
            DFT_language,
            FK_Books_Categories_CategoryId,
            FK_Books_Publishers_PublisherId,
            DFT_Book_created

    DROP TABLE dbo.Publishers;
    DROP TABLE dbo.Categories;
    DROP TABLE dbo.Books;
END
GO

IF OBJECT_ID('dbo.ClearStoreTables', 'P') IS NOT NULL
    DROP PROCEDURE dbo.ClearStoreTables
GO
CREATE PROCEDURE ClearStoreTables AS
BEGIN
    TRUNCATE TABLE dbo.Books;

    ALTER TABLE dbo.Books
        DROP CONSTRAINT FK_Books_Categories_CategoryId,
            FK_Books_Publishers_PublisherId;

    TRUNCATE TABLE dbo.Categories;
    TRUNCATE TABLE dbo.Publishers;

    ALTER TABLE dbo.Books
        ADD
            CONSTRAINT FK_Books_Categories_CategoryId FOREIGN KEY (CategoryId)
                REFERENCES dbo.Categories (Id)
                ON DELETE SET NULL,
            CONSTRAINT FK_Books_Publishers_PublisherId FOREIGN KEY (PublisherId)
                REFERENCES Publishers (Id)
                ON DELETE SET NULL
END
GO

IF OBJECT_ID('dbo.FillStoreEntitiesWithTestData', 'P') IS NOT NULL
    DROP PROCEDURE dbo.FillStoreEntitiesWithTestData
GO
CREATE PROCEDURE FillStoreEntitiesWithTestData @BooksCount DECIMAL AS
BEGIN
    SET NOCOUNT ON
    DECLARE @i INT = 1;
    DECLARE @categoryId BIGINT;
    DECLARE @subCategoryId BIGINT;
    DECLARE @publisherId BIGINT;
    DECLARE @booksInPublishersCount INT = 10;
    DECLARE @subCategoriesInCategoriesCount INT = 10;
    DECLARE @categoriesCount INT = @BooksCount / @booksInPublishersCount / @subCategoriesInCategoriesCount
    DECLARE @pprice DECIMAL(5, 2);
    DECLARE @rprice DECIMAL(5, 2);
    DECLARE @pageCount INT;
    DECLARE @baseDate DATETIME = DATEADD(DAY, @subCategoriesInCategoriesCount, GETDATE())
    BEGIN TRANSACTION
        WHILE @i <= @categoriesCount
            BEGIN
                -- создать категорию
                INSERT INTO Categories (Name)
                VALUES (CONCAT('Category-', @i));
                SET @categoryId = SCOPE_IDENTITY();

                DECLARE @j INT = 1;
                WHILE @j <= @subCategoriesInCategoriesCount
                    BEGIN
                        -- создать подкатегорию
                        INSERT INTO Categories (Name, ParentId, Created)
                        VALUES (CONCAT('SubCategory-', @i, '-', @j), @categoryId, DATEADD(day, @j, @baseDate));
                        SET @subCategoryId = SCOPE_IDENTITY();
                        -- создать издательство
                        INSERT INTO Publishers (Name, Country, Created)
                        VALUES (CONCAT('Publisher-', @i, '-', @j), 'Russia', DATEADD(day, @j, @baseDate))
                        SET @publisherId = SCOPE_IDENTITY();

                        DECLARE @k INT = 1;
                        WHILE @k <= @booksInPublishersCount
                            BEGIN
                                SET @pprice = RAND() * (500 - 5 + 1);
                                SET @rprice = (RAND() * @pprice) + @pprice;
                                SET @pageCount = RAND() * 1000;
                                INSERT INTO Books (Title, Authors, Language, CategoryId, PublisherId, Year, PageCount,
                                                   PurchasePrice, RetailPrice, Description, Created)
                                VALUES (CONCAT('Book-', @i, '-', @j, '-', @k), CONCAT('Author-', @i, '-', @j, '-', @k),
                                        'Russian',
                                        @subCategoryId, @publisherId, 2020, CAST(@pageCount AS INT), @pprice, @rprice,
                                        CONCAT('BookDescription-', @i, '-', @j, '-', @k), DATEADD(day, @j, @baseDate))
                                SET @k = @k + 1;
                            END
                        SET @j = @j + 1;
                    END
                SET @i = @i + 1;
            END
    COMMIT
END
GO

EXEC CreateStoreTables;
GO

EXEC DeleteStoreTables;
GO

EXEC FillStoreEntitiesWithTestData 500;
GO

EXEC ClearStoreTables
GO



