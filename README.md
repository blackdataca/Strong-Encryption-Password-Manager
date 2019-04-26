#  MyID - Secure Password Manager

Defend your passwords in depths with MyID

## Industry standard algorithm

Simple to use password manager with absolutely verifiable security strength. 

## Ai watches best security practices for you

To ensure cyber-safety in tomorrow's changing world,  artificial intelligence watches the net for latest security vulnerabilities. Ai analyzes and discoveries and updates in app adverser algorithm automatically.
```mermaid
graph LR
A(Data) --> B((Encryption))
B --> D{Ai}
D -- Learn --> C(Knownledge)
C -- Update--> D
D -- Advice --> A
```
## Your own tiny-net - no server

MyID syncs encrypted data to the devices in your own tiny blockchain net, which means your data is backup securely and **nobody** can access your data without your private key. 
Your team's devices can also become fault-tolerant nodes of your tiny-net. In the event of device lost, not only the nodes help you have all your password data back but also have the lost data self-destruct. Your data is also recoverable from ransomware damage.

An example of 2 devices help the 3rd device recover data when the device is lost or damaged:

```mermaid
graph TD
    Alice --> John/New=Recover
    Bob --> John/New=Recover
    Alice -x John/Lost=Self-destruct
    Bob -x John/Lost=Self-destruct
```

## Sync Shared passwords with your friends and team members

Never need to send clear password in chat, phone or email again. Sync encrypted passwords with your friends and enemies with confidence. 

```mermaid
sequenceDiagram
Note left of Alice (IT): Ai advices<br>security policy
Alice (IT) ->> Bob (easy user): Change Team Password
Bob (easy user)->>John (micromanager): Do you also approve Alice's change?
John (micromanager)->>Bob (easy user): Approve
Bob (easy user)->>Alice (IT): Accept change
Note right of John (micromanager): Consensus<br>mechanism
Alice (IT) ->> Bob (easy user): Accidential change
Bob (easy user)->>John (micromanager): Do you also approve Alice's change?
John (micromanager)-x Bob (easy user): Not really!
Bob (easy user) -x Alice (IT): Reject!
```




## Fully open-source for enterprise use

MIT license welcome anyone to use the software, verify and make improvements to make internet-world safer.

