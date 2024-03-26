CREATE TABLE [dbo].[secrets]
(
	[id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newsequentialid(), 
    [record_id] VARCHAR(36) NULL, 
    [payload] TEXT NOT NULL, 
    [created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [modified] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [synced] DATETIME NULL, 
    [deleted] BIT NULL , 
    
)

GO


CREATE INDEX [IX_secrets_record_id] ON [dbo].[secrets] ([record_id])
