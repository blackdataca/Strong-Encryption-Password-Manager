CREATE TABLE [dbo].[secrets]
(
	[id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newsequentialid(), 
    [record_id] VARCHAR(23) NULL, 
    [payload] TEXT NOT NULL, 
    [created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [modified] DATETIME NULL, 
    [synced] DATETIME NULL, 
    [deleted] DATETIME NULL, 
    
)

GO


CREATE INDEX [IX_secrets_record_id] ON [dbo].[secrets] ([record_id])
