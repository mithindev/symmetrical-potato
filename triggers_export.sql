
CREATE TRIGGER dbo.trg_actrans_audit_delete
ON fiscusdb.dbo.actrans
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @User NVARCHAR(100) = SUSER_SNAME();

    INSERT INTO fiscusdb.dbo.AuditTrail
    (
        TableName,
        RecordId

CREATE TRIGGER dbo.trg_suplement_audit_delete
ON fiscusdb.dbo.suplement
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @User NVARCHAR(100) = SUSER_SNAME();

    INSERT INTO fiscusdb.dbo.AuditTrail
    (
        TableName,
        Reco

CREATE TRIGGER dbo.trg_suplement_audit_update
ON fiscusdb.dbo.suplement
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @User NVARCHAR(100) = SUSER_SNAME();

    INSERT INTO fiscusdb.dbo.AuditTrail
    (
        TableName,
        Reco

CREATE TRIGGER dbo.trg_actrans_audit_update
ON fiscusdb.dbo.actrans
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @User NVARCHAR(100) = SUSER_SNAME();

    INSERT INTO fiscusdb.dbo.AuditTrail
    (
        TableName,
        RecordId
