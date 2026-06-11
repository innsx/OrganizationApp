CREATE TABLE [dbo].[tblEmployees]
(
    [Id]            VARCHAR(22) NOT NULL,
    [PagingOrder]   INT IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR(50) NOT NULL,
    [Age]           INT NOT NULL,
    [Position]      VARCHAR(50) NOT NULL,
    [CompanyId]     VARCHAR(22) NOT NULL,
    [Salary]        DECIMAL(18, 2) NOT NULL,
    [CreatedOn]     DateTime NULL Default GetDateTime(),
    [ModifiedOn]    DateTime NULL Default GetDateTime(),
    [IsDeleted]     BIT DEFAULT ((0)) NOT NULL,

    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employees_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[tblCompanies]([Id]),
    CONSTRAINT [UK_Employees_PagingOrder] UNIQUE NONCLUSTERED ([PagingOrder] ASC),
    INDEX [IX_Employees_Name] NONCLUSTERED ([Name]),
)
