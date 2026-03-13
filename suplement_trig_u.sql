
ALTER TRIGGER dbo.trg_suplement_audit_update
ON fiscusdb.dbo.suplement
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Ignore changes from the web application
    IF APP_NAME() LIKE '%.Net SqlClient Data Provider%' RETURN;

    DECLARE @User NVARCHAR(100) = SUSER_SNAME();

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'suplement', i.tId, 'date', CONVERT(NVARCHAR(50), d.[date], 120), CONVERT(NVARCHAR(50), i.[date], 120), 'UPDATE', a.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId LEFT JOIN fiscusdb.dbo.actrans a ON a.Id = i.transid WHERE NOT (i.scroll = 1 AND d.scroll = 0) AND ISNULL(CONVERT(NVARCHAR(50), d.[date], 120),'') <> ISNULL(CONVERT(NVARCHAR(50), i.[date], 120),'');

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'suplement', i.tId, 'transid', CAST(d.transid AS NVARCHAR(MAX)), CAST(i.transid AS NVARCHAR(MAX)), 'UPDATE', a.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId LEFT JOIN fiscusdb.dbo.actrans a ON a.Id = i.transid WHERE NOT (i.scroll = 1 AND d.scroll = 0) AND ISNULL(CAST(d.transid AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.transid AS NVARCHAR(MAX)),'');

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'suplement', i.tId, 'achead', CONVERT(NVARCHAR(MAX), d.achead), CONVERT(NVARCHAR(MAX), i.achead), 'UPDATE', a.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId LEFT JOIN fiscusdb.dbo.actrans a ON a.Id = i.transid WHERE NOT (i.scroll = 1 AND d.scroll = 0) AND ISNULL(CONVERT(NVARCHAR(MAX), d.achead),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.achead),'');

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'suplement', i.tId, 'debit', CAST(d.debit AS NVARCHAR(MAX)), CAST(i.debit AS NVARCHAR(MAX)), 'UPDATE', a.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId LEFT JOIN fiscusdb.dbo.actrans a ON a.Id = i.transid WHERE NOT (i.scroll = 1 AND d.scroll = 0) AND ISNULL(CAST(d.debit AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.debit AS NVARCHAR(MAX)),'');

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'suplement', i.tId, 'credit', CAST(d.credit AS NVARCHAR(MAX)), CAST(i.credit AS NVARCHAR(MAX)), 'UPDATE', a.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId LEFT JOIN fiscusdb.dbo.actrans a ON a.Id = i.transid WHERE NOT (i.scroll = 1 AND d.scroll = 0) AND ISNULL(CAST(d.credit AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.credit AS NVARCHAR(MAX)),'');

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'suplement', i.tId, 'acn', CONVERT(NVARCHAR(MAX), d.acn), CONVERT(NVARCHAR(MAX), i.acn), 'UPDATE', a.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId LEFT JOIN fiscusdb.dbo.actrans a ON a.Id = i.transid WHERE NOT (i.scroll = 1 AND d.scroll = 0) AND ISNULL(CONVERT(NVARCHAR(MAX), d.acn),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.acn),'');

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'suplement', i.tId, 'narration', CONVERT(NVARCHAR(MAX), d.narration), CONVERT(NVARCHAR(MAX), i.narration), 'UPDATE', a.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId LEFT JOIN fiscusdb.dbo.actrans a ON a.Id = i.transid WHERE NOT (i.scroll = 1 AND d.scroll = 0) AND ISNULL(CONVERT(NVARCHAR(MAX), d.narration),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.narration),'');

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'suplement', i.tId, 'type', CONVERT(NVARCHAR(MAX), d.[type]), CONVERT(NVARCHAR(MAX), i.[type]), 'UPDATE', a.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId LEFT JOIN fiscusdb.dbo.actrans a ON a.Id = i.transid WHERE NOT (i.scroll = 1 AND d.scroll = 0) AND ISNULL(CONVERT(NVARCHAR(MAX), d.[type]),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.[type]),'');

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'suplement', i.tId, 'scroll', CAST(d.scroll AS NVARCHAR(MAX)), CAST(i.scroll AS NVARCHAR(MAX)), 'UPDATE', a.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId LEFT JOIN fiscusdb.dbo.actrans a ON a.Id = i.transid WHERE NOT (i.scroll = 1 AND d.scroll = 0) AND ISNULL(CAST(d.scroll AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.scroll AS NVARCHAR(MAX)),'');

END;
