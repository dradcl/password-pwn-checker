# password-pwn-checker
Check if your password has been the victim of a breach. Implemented in C#  
Based on https://haveibeenpwned.com/API/v2  
![Alt text](https://media.discordapp.net/attachments/630277945507708928/838962365289463868/unknown.png "")

# How it works  
Simply, it hashes an entered password with SHA1 and with the first 5 characters of that hash it searches the database. This program will use the database results to match your hash to see if you have been pwned. Read more [here](https://haveibeenpwned.com/API/v2#PwnedPasswords)
