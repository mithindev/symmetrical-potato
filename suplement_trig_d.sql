
ALTER TRIGGER dbo.trg_suplement_audit_delete
ON fiscusdb.dbo.suplement
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
        'suplement',
        d.tId,
        'Full Record',
        (SELECT * FROM deleted d2 WHERE d2.tId = d.tId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        'DELETE',
        a.sesusr,      -- CreatedBy from actrans
        @User
    FROM deleted d
    LEFT JOIN fiscusdb.dbo.actrans a ON a.Id = d.transid;

END;
