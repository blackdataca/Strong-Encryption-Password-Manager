#  This is Not a fully functional branch

Work in progress. Things will change. 

Defend your passwords with MyID

![](https://github.com/blackdataca/myid/blob/master/mainscreen.PNG)

## Installation

Compile with Visual Studio 2022


![](https://github.com/blackdataca/Strong-Encryption-Password-Manager/blob/MyID-on-Mac/Copy2Apps.png?raw=true)

## Industry standard algorithm

Simple password manager with absolutely verifiable security strength. 


## Separated data files and private key

By default, MyID stores encrypted data without private key in your My Documents folder. The encrypted data files can be safely relocated to any other public location such as Dropbox, Google Drive, OneDrive, etc.

Private key is encrypted and stored securely inside your computer's key store. Without private key, nobody can decrypt the data. You can export the private key to a safe location for backup.


## Convenient access password

As an extra layer of protection, use an easy to remember access password to unlock your password data. 


## Fully open-source for enterprise and private use

MIT license welcome anyone to use the software, verify and make improvements to make internet-world safer.



## MyIdWeb - User Secret Management System

MyIdWeb on ASP.NET + SQL Server

>Development environment, do NOT use

### Introduction
Use strongest standard encryption to store everyone's secrets in a safe place

### Create user
1. Create asymmetric Public Key and Private Key
2. Save Public Key in -> users.public_key
3. Symmetric encrypt Private Key with user's password + users.uuid as salt -> users.private_key(encrypted)

* Cannot reset password
* No need to collect email address

### User change password
1. Symmetric decrypt Private Key with old password <- users.private_key(encrypted)
2. Symmetric re-encrypt Privte Key with new password -> users.private_key(encrypted)

### Create secret
1. Generate a new Secret Key
2. Symmetric encrypt secret payload (site, username, password, memo and files) with Secret Key
3. Asymmetric encrypt Secret Key with users.public_key -> secrets_users.secret_key(encrypted)

### Read secret
1. Symmetric decrypt Private Key from users.private_key(encrypted) with user's password + users.uuid as salt
2. Asymmetric decrypt Secret Key from secrets_users.secret_key(encrypted) with Private Key
3. Symmetric decrypt secret payload with Secret Key 

### Write secret
1. Get Secret Key by the first two steps in above [Read secret](#read-secret)
2. Symmetric encrypt secret (site, username, password, memo and files) with Secret Key

### Share secret with Bob
1. Get Secret Key by the first two steps in above [Read secret](#read-secret)
2. Generate a temporary user with asymmetric Public Key -> users.public_key, and Private Key 
3. Symmetric encrypt Private Key with temporary password -> users.private_key(encrypted)
4. Send secret id, temporary user id and temporary password to Bob
5. Bob creates account or login
6. Get Secret Key by the first two steps in above [Read secret](#read-secret) using temporary user
7. Asymmetric encrypt Secret Key with Bob's users.public_key -> secrets_users.secret_key(encrypted)
8. Remove temporary user
9. Bob follows above [read secret](#read-secret)


### Revoke shared secret
1. Secret owner removes other user from secrets_users record
2. Other user removes themselves from secrets_users record

### Delete Secret
1. Secret owner delete secrets record.
2. Secrets_users records are deleted

