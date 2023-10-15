IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Order') AND type in (N'U'))
BEGIN
    CREATE TABLE dbo.[Order] (
        Id BIGINT IDENTITY(1,1) NOT NULL,
		Code UNIQUEIDENTIFIER  NOT NULL,
		ProductId BIGINT NOT NULL,
        OrderStatus INT NOT NULL,
        CreatedAt DATETIME NULL,
        StockValidatedAt DATETIME NULL,
        PaymentValidatedAt DATETIME NULL,
        ConfirmedAt DATETIME NULL,
        ClosedAt DATETIME NULL,
        CONSTRAINT operationType_PK PRIMARY KEY (Id)
    );
END