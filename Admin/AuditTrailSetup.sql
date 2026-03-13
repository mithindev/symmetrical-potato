-- Run this script in your fiscusdb database to create the AuditTrail table

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditTrail]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AuditTrail] (
        [AuditId]    INT             IDENTITY (1, 1) NOT NULL,
        [TableName]  NVARCHAR (255)  NOT NULL,
        [RecordId]   NVARCHAR (255)  NOT NULL,
        [ColumnName] NVARCHAR (255)  NULL,
        [CreatedBy]  NVARCHAR (255)  NULL,
        [OldValue]   NVARCHAR (MAX)  NULL,
        [NewValue]   NVARCHAR (MAX)  NULL,
        [ActionType] NVARCHAR (50)   NOT NULL,
        [ChangedBy]  NVARCHAR (255)  NOT NULL,
        [ChangedOn]  DATETIME        DEFAULT (getdate()) NOT NULL,
        PRIMARY KEY CLUSTERED ([AuditId] ASC)
    );
    
    PRINT 'AuditTrail table created successfully.';
END
ELSE
BEGIN
    PRINT 'AuditTrail table already exists.';
END
GO


