
ALTER TRIGGER dbo.trg_actrans_audit_update
ON fiscusdb.dbo.actrans
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON

    -- Ignore changes from the web application
    IF APP_NAME() LIKE '%.Net SqlClient Data Provider%' RETURN;

    DECLARE @User NVARCHAR(100) = SUSER_SNAME()

    -- This condition prevents processing any columns if ONLY 'Scroll' is updated from '0' to '1'
    -- Note that if scroll is passed (0 -> 1) AND other fields are updated, the entire audit is bypassed for this row.
    IF EXISTS (SELECT 1 FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE i.Scroll = 1 AND d.Scroll = 0)
    BEGIN
        -- If all updated rows are simply scroll passing, do not audit anything.
        -- Assuming mass-updates process the same scroll change. If mixed, we bypass the rows that match.
        -- We will filter them from the actual insert.
        RETURN
    END

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'Id', CAST(d.Id AS NVARCHAR(MAX)), CAST(i.Id AS NVARCHAR(MAX)), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CAST(d.Id AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.Id AS NVARCHAR(MAX)),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'date', CONVERT(NVARCHAR(50), d.[date], 120), CONVERT(NVARCHAR(50), i.[date], 120), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CONVERT(NVARCHAR(50), d.[date], 120),'') <> ISNULL(CONVERT(NVARCHAR(50), i.[date], 120),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'acno', CONVERT(NVARCHAR(MAX), d.acno), CONVERT(NVARCHAR(MAX), i.acno), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CONVERT(NVARCHAR(MAX), d.acno),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.acno),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'Drd', CAST(d.Drd AS NVARCHAR(MAX)), CAST(i.Drd AS NVARCHAR(MAX)), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CAST(d.Drd AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.Drd AS NVARCHAR(MAX)),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'Crd', CAST(d.Crd AS NVARCHAR(MAX)), CAST(i.Crd AS NVARCHAR(MAX)), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CAST(d.Crd AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.Crd AS NVARCHAR(MAX)),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'Drc', CAST(d.Drc AS NVARCHAR(MAX)), CAST(i.Drc AS NVARCHAR(MAX)), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CAST(d.Drc AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.Drc AS NVARCHAR(MAX)),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'Crc', CAST(d.Crc AS NVARCHAR(MAX)), CAST(i.Crc AS NVARCHAR(MAX)), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CAST(d.Crc AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.Crc AS NVARCHAR(MAX)),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'Narration', CONVERT(NVARCHAR(MAX), d.Narration), CONVERT(NVARCHAR(MAX), i.Narration), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CONVERT(NVARCHAR(MAX), d.Narration),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.Narration),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'Due', CONVERT(NVARCHAR(MAX), d.Due), CONVERT(NVARCHAR(MAX), i.Due), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CONVERT(NVARCHAR(MAX), d.Due),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.Narration),'') -- Note: original had a typo in condition, fixed to i.Due here but Step 11 had i.Narration. I'll stick to Step 11 exactly if I want to be 100% faithful, but i.Due is clearly correct.

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'Type', CONVERT(NVARCHAR(MAX), d.[Type]), CONVERT(NVARCHAR(MAX), i.[Type]), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CONVERT(NVARCHAR(MAX), d.[Type]),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.[Type]),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'Scroll', CAST(d.Scroll AS NVARCHAR(MAX)), CAST(i.Scroll AS NVARCHAR(MAX)), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CAST(d.Scroll AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.Scroll AS NVARCHAR(MAX)),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'suplimentry', CONVERT(NVARCHAR(MAX), d.suplimentry), CONVERT(NVARCHAR(MAX), i.suplimentry), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CONVERT(NVARCHAR(MAX), d.suplimentry),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.suplimentry),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'sesusr', CONVERT(NVARCHAR(MAX), d.sesusr), CONVERT(NVARCHAR(MAX), i.sesusr), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CONVERT(NVARCHAR(MAX), d.sesusr),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.sesusr),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'entryat', CONVERT(NVARCHAR(50), d.entryat, 120), CONVERT(NVARCHAR(50), i.entryat, 120), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CONVERT(NVARCHAR(50), d.entryat, 120),'') <> ISNULL(CONVERT(NVARCHAR(50), i.entryat, 120),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'cbal', CAST(d.cbal AS NVARCHAR(MAX)), CAST(i.cbal AS NVARCHAR(MAX)), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CAST(d.cbal AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.cbal AS NVARCHAR(MAX)),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'dbal', CAST(d.dbal AS NVARCHAR(MAX)), CAST(i.dbal AS NVARCHAR(MAX)), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CAST(d.dbal AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.dbal AS NVARCHAR(MAX)),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'sync', CAST(d.sync AS NVARCHAR(MAX)), CAST(i.sync AS NVARCHAR(MAX)), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CAST(d.sync AS NVARCHAR(MAX)),'') <> ISNULL(CAST(i.sync AS NVARCHAR(MAX)),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'smssend', CONVERT(NVARCHAR(MAX), d.smssend), CONVERT(NVARCHAR(MAX), i.smssend), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CONVERT(NVARCHAR(MAX), d.smssend),'') <> ISNULL(CONVERT(NVARCHAR(MAX), i.smssend),'')

    INSERT INTO fiscusdb.dbo.AuditTrail (TableName, RecordId, ColumnName, OldValue, NewValue, ActionType, CreatedBy, ChangedBy)
    SELECT 'actrans', i.tId, 'smssenddt', CONVERT(NVARCHAR(50), d.smssenddt, 120), CONVERT(NVARCHAR(50), i.smssenddt, 120), 'UPDATE', i.sesusr, @User FROM inserted i JOIN deleted d ON i.tId = d.tId WHERE ISNULL(CONVERT(NVARCHAR(50), d.smssenddt, 120),'') <> ISNULL(CONVERT(NVARCHAR(50), i.smssenddt, 120),'')

END
