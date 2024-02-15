CREATE TABLE [dbo].[logs]
(
	[id] INT NOT NULL PRIMARY KEY identity, 
    [user_id] NVARCHAR(450) NULL, 
    [created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [description] NVARCHAR(200) NULL, 
    [extra] NVARCHAR(200) NULL, 
    [remote_address] VARCHAR(50) NULL, 
    CONSTRAINT [FK_logs_users] FOREIGN KEY ([user_id]) REFERENCES [users]([id])
)
