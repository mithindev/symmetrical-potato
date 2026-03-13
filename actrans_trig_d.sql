
ALTER TRIGGER dbo.trg_actrans_audit_delete
ON fiscusdb.dbo.actrans
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Ignore changes from the web application
    IF APP_NAME() LIKE '%.Net SqlClient Data Provider%' RETURN;

    DECLARE @User NVARCHAR(100) = SUSER_SNAME();

    INSERT INTO fiscusdb.dbo.AuditTrail
    (
        TableName,
        RecordId,
        ColumnName,
        OldValue,
        NewValue,
        ActionType,
        CreatedBy,
        ChangedBy
    )
    SELECT
        'actrans',
        d.tId,
        'Full Record',
        (SELECT * FROM deleted d2 WHERE d2.tId = d.tId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        'DELETE',
        d.sesusr,
        @User
    FROM deleted d;

END;
