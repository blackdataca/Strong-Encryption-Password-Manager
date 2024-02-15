CREATE TABLE [dbo].[secrets]
(
	[id] BINARY(16) NOT NULL PRIMARY KEY, 
    [record_id] VARCHAR(23) NOT NULL, 
    [payload] TEXT NOT NULL, 
    [created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [modified] DATETIME NULL, 
    [synced] DATETIME NULL 
)

GO


CREATE INDEX [IX_secrets_record_id] ON [dbo].[secrets] ([record_id])
