CREATE TABLE [dbo].[users]
(
	[id] NVARCHAR(450) NOT NULL PRIMARY KEY, 
    [name] VARCHAR(50) NOT NULL, 
    [public_key] VARCHAR(2048) NOT NULL, 
    [private_key] VARCHAR(4096) NOT NULL, 
    [security_stamp] NVARCHAR(MAX) NOT NULL,
    [status] BIT NOT NULL DEFAULT 0, 
    [perference] INT NULL, 
    [idle_timeout] INT NULL, 
    [last_active] DATETIME NULL, 
    [created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [modified] DATETIME NULL, 
    [expiry] DATETIME NULL, 
    CONSTRAINT [AK_users_name] UNIQUE ([name])
)
