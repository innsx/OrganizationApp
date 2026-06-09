CREATE TABLE [dbo].[tblEmployees]
(
    [Id]            VARCHAR(22) NOT NULL,
    [Name]          VARCHAR(50) NOT NULL,
    [Age]           INT NOT NULL,
    [Position]      VARCHAR(50) NOT NULL,
    [CompanyId]     VARCHAR(22) NOT NULL,
    [Salary]        DECIMAL(18, 2) NOT NULL,  
    [IsDeleted]     BIT DEFAULT ((0)) NOT NULL,

    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employees_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[tblCompanies]([Id]),
    INDEX [IX_Employees_Name] NONCLUSTERED ([Name]),
)
