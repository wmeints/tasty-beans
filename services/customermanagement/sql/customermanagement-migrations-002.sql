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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220227094632_CreateSchema')
BEGIN
    CREATE TABLE [Customers] (
        [Id] uniqueidentifier NOT NULL,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [InvoiceAddress_Street] nvarchar(100) NOT NULL,
        [InvoiceAddress_HouseNumber] nvarchar(20) NOT NULL,
        [InvoiceAddress_PostalCode] nvarchar(20) NOT NULL,
        [InvoiceAddress_City] nvarchar(100) NOT NULL,
        [InvoiceAddress_CountryCode] nvarchar(10) NOT NULL,
        [ShippingAddress_Street] nvarchar(100) NOT NULL,
        [ShippingAddress_HouseNumber] nvarchar(20) NOT NULL,
        [ShippingAddress_PostalCode] nvarchar(20) NOT NULL,
        [ShippingAddress_City] nvarchar(100) NOT NULL,
        [ShippingAddress_CountryCode] nvarchar(10) NOT NULL,
        [EmailAddress] nvarchar(500) NOT NULL,
        [TelephoneNumber] nvarchar(13) NOT NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220227094632_CreateSchema')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220227094632_CreateSchema', N'6.0.3');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220410083029_AddRowVersionProperty')
BEGIN
    ALTER TABLE [Customers] ADD [Version] rowversion NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220410083029_AddRowVersionProperty')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220410083029_AddRowVersionProperty', N'6.0.3');
END;
GO

COMMIT;
GO

