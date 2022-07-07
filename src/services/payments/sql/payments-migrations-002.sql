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

CREATE TABLE [PaymentMethods] (
    [Id] uniqueidentifier NOT NULL,
    [CustomerId] uniqueidentifier NOT NULL,
    [CardHolderName] nvarchar(150) NOT NULL,
    [CardNumber] nvarchar(16) NOT NULL,
    [SecurityCode] nvarchar(3) NOT NULL,
    [ExpirationDate] nvarchar(5) NOT NULL,
    [Type] int NOT NULL,
    [Version] rowversion NULL,
    CONSTRAINT [PK_PaymentMethods] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220411061127_AddRowVersion', N'6.0.3');
GO

COMMIT;
GO

