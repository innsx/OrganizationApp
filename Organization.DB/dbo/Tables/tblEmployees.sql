CREATE TABLE [dbo].[tblEmployees]
(
    [Id]            VARCHAR(22) NOT NULL,
    [PagingOrder]   INT IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR(50) NOT NULL,
    [Age]           INT NOT NULL,
    [Position]      VARCHAR(50) NOT NULL,
    [CompanyId]     VARCHAR(22) NOT NULL,
    [Salary]        DECIMAL(18, 2) NOT NULL,
    [CreatedOn]     DateTime NULL Default GetDate(),
    [ModifiedOn]    DateTime NULL Default GetDate(),
    [IsDeleted]     BIT DEFAULT ((0)) NOT NULL,

    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employees_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[tblCompanies]([Id]),
    CONSTRAINT [UK_Employees_PagingOrder] UNIQUE NONCLUSTERED ([PagingOrder] ASC),
    CONSTRAINT [UK_Employees_Name] UNIQUE NONCLUSTERED ([Name]),
    INDEX [IX_Employees_Name] NONCLUSTERED ([Name]),
    INDEX [IX_Employees_IsDeleted] NONCLUSTERED ([IsDeleted]),
    INDEX [IX_Employees_CreatedOn] NONCLUSTERED ([CreatedOn])
)
