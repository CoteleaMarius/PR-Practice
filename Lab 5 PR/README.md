📧 EmailClient Console App (C# + MailKit)
Aplicație de consolă scrisă în C# care permite gestionarea emailurilor prin protocoalele POP3, IMAP și SMTP, utilizând biblioteca MailKit.

⚙️ Funcționalități
✅ Afișarea emailurilor din contul Gmail folosind POP3 sau IMAP

📎 Descărcarea atașamentelor dintr-un email (IMAP)

📨 Trimiterea de emailuri (cu sau fără atașamente și câmp Reply-To)

🔐 Conexiuni securizate cu SSL/TLS (porturi standard)

🧪 Exemplu de utilizare
La pornire, meniul oferă:
text
Copiază
Editează
1. Show emails (POP3)
2. Show emails (IMAP)
3. Download attachments from an email
4. Send email
5. Exit
Exemplu de trimitere email:
yaml
Copiază
Editează
Sender name: Marius
Recipient: john@example.com
Subject: Hello
Message: This is a test email.
Attachment file path: C:\Images\cat.jpg
Reply-To email: contact@example.com
📂 Structura funcțională
Metodă    Descriere
GetEmailsPop3()    Afișează ultimele N emailuri folosind POP3
GetEmailsImap()    Afișează ultimele N emailuri folosind IMAP
DownloadEmailWithAttachments()    Salvează atașamentele de la un email dat (IMAP)
SendEmail()    Trimite un email cu sau fără atașament
IsValidEmail()    Validează formatul adresei de email

🧱 Tehnologii și biblioteci
.NET 6.0+

MailKit

MimeKit

Instalare pachete (folosind .NET CLI):

bash
Copiază
Editează
dotnet add package MailKit
dotnet add package MimeKit
🛡️ Securitate
🔐 Nu folosi credențiale reale hardcodate!
Acest exemplu folosește doar scopuri educaționale. Se recomandă:

Citirea parolelor din fișiere .env sau din variabile de mediu.

Folosirea OAuth2 pentru Gmail în aplicații reale.

Activarea „App Passwords” în contul Google pentru aplicații mai puțin sigure.

▶️ Rulare
Compilare și rulare:

bash
Copiază
Editează
dotnet build
dotnet run
⚠️ Limitări
Suport doar pentru Gmail (cu POP3/IMAP/SMTP configurate în cont).

Nu suportă foldere personalizate sau mutarea emailurilor.

Nu suportă vizualizarea conținutului HTML sau descărcarea inline a imaginilor.

Nu include autentificare OAuth2.

💡 Idei de extindere
Interfață grafică (WPF/WinForms)

Export emailuri în format .eml sau .pdf

Suport pentru sortare, căutare și filtrare după expeditor/data

Logging și sistem de backup
