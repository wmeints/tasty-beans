IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220421114835_CreateInitialSchema')
BEGIN
    CREATE TABLE [Customers] (
        [Id] uniqueidentifier NOT NULL,
        [FirstName] nvarchar(150) NOT NULL,
        [LastName] nvarchar(150) NOT NULL,
        [EmailAddress] nvarchar(500) NOT NULL,
        [ShippingAddress_Street] nvarchar(100) NOT NULL,
        [ShippingAddress_HouseNumber] nvarchar(20) NOT NULL,
        [ShippingAddress_PostalCode] nvarchar(20) NOT NULL,
        [ShippingAddress_City] nvarchar(100) NOT NULL,
        [ShippingAddress_CountryCode] nvarchar(10) NOT NULL,
        [Version] rowversion NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220421114835_CreateInitialSchema')
BEGIN
    CREATE TABLE [Products] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(500) NOT NULL,
        [Version] rowversion NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220421114835_CreateInitialSchema')
BEGIN
    CREATE TABLE [ShippingOrders] (
        [Id] uniqueidentifier NOT NULL,
        [CustomerId] uniqueidentifier NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Street] nvarchar(max) NOT NULL,
        [HouseNumber] nvarchar(max) NOT NULL,
        [PostalCode] nvarchar(max) NOT NULL,
        [City] nvarchar(max) NOT NULL,
        [CountryCode] nvarchar(max) NOT NULL,
        [Status] int NOT NULL,
        [Version] rowversion NULL,
        CONSTRAINT [PK_ShippingOrders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ShippingOrders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220421114835_CreateInitialSchema')
BEGIN
    CREATE TABLE [OrderItem] (
        [ShippingOrderId] uniqueidentifier NOT NULL,
        [Id] int NOT NULL IDENTITY,
        [ProductId] uniqueidentifier NOT NULL,
        [Description] nvarchar(250) NOT NULL,
        [Amount] int NOT NULL,
        CONSTRAINT [PK_OrderItem] PRIMARY KEY ([ShippingOrderId], [Id]),
        CONSTRAINT [FK_OrderItem_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderItem_ShippingOrders_ShippingOrderId] FOREIGN KEY ([ShippingOrderId]) REFERENCES [ShippingOrders] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220421114835_CreateInitialSchema')
BEGIN
    CREATE INDEX [IX_OrderItem_ProductId] ON [OrderItem] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220421114835_CreateInitialSchema')
BEGIN
    CREATE INDEX [IX_ShippingOrders_CustomerId] ON [ShippingOrders] ([CustomerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220421114835_CreateInitialSchema')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220421114835_CreateInitialSchema', N'6.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220421123539_AddMissingFields')
BEGIN
    ALTER TABLE [ShippingOrders] ADD [LastUpdatedDate] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220421123539_AddMissingFields')
BEGIN
    ALTER TABLE [ShippingOrders] ADD [OrderDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220421123539_AddMissingFields')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220421123539_AddMissingFields', N'6.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220423065330_RemoveCustomerFields')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ShippingOrders]') AND [c].[name] = N'City');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ShippingOrders] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [ShippingOrders] DROP COLUMN [City];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220423065330_RemoveCustomerFields')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ShippingOrders]') AND [c].[name] = N'CountryCode');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [ShippingOrders] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [ShippingOrders] DROP COLUMN [CountryCode];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220423065330_RemoveCustomerFields')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ShippingOrders]') AND [c].[name] = N'FirstName');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [ShippingOrders] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [ShippingOrders] DROP COLUMN [FirstName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220423065330_RemoveCustomerFields')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ShippingOrders]') AND [c].[name] = N'HouseNumber');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [ShippingOrders] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [ShippingOrders] DROP COLUMN [HouseNumber];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220423065330_RemoveCustomerFields')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ShippingOrders]') AND [c].[name] = N'LastName');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [ShippingOrders] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [ShippingOrders] DROP COLUMN [LastName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220423065330_RemoveCustomerFields')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ShippingOrders]') AND [c].[name] = N'PostalCode');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [ShippingOrders] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [ShippingOrders] DROP COLUMN [PostalCode];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220423065330_RemoveCustomerFields')
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ShippingOrders]') AND [c].[name] = N'Street');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [ShippingOrders] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [ShippingOrders] DROP COLUMN [Street];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220423065330_RemoveCustomerFields')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220423065330_RemoveCustomerFields', N'6.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220430124714_RemoveDescriptionField')
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'Description');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [OrderItem] DROP COLUMN [Description];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220430124714_RemoveDescriptionField')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220430124714_RemoveDescriptionField', N'6.0.4');
END;
GO

COMMIT;
GO

