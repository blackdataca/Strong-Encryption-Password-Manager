CREATE TABLE [dbo].[secrets_users]
(
	[user_id] NVARCHAR(450) NOT NULL, 
    [secret_id] UNIQUEIDENTIFIER NOT NULL, 
    [secret_key] VARCHAR(2048) NOT NULL, 
    [is_owner] BIT NULL, 
    CONSTRAINT [PK_secrets_users] PRIMARY KEY ([user_id], [secret_id]), 
    CONSTRAINT [FK_secrets_users_users] FOREIGN KEY ([user_id]) REFERENCES [users]([id]), 
    CONSTRAINT [FK_secrets_users_secrets] FOREIGN KEY ([secret_id]) REFERENCES [secrets]([id])
)
