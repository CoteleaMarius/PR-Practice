using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

class EmailClient
{
    private string username = "marius.cotelea1003@gmail.com";
    private string password = "khlpuhpaqcyjcoxw";

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        return email.Contains("@");
    }

    public void GetEmailsPop3(int maxEmails = 5)
    {
        using (var client = new Pop3Client())
        {
            client.Connect("pop.gmail.com", 995, SecureSocketOptions.SslOnConnect);
            client.Authenticate(username, password);

            int messageCount = client.Count;
            Console.WriteLine($"Total messages: {messageCount}");

            for (int i = 0; i < messageCount && i < maxEmails; i++)
            {
                var message = client.GetMessage(i);
                Console.WriteLine($"Message {i + 1}:");
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.From}");
            }

            client.Disconnect(true);
        }
    }

    public void GetEmailsImap(int maxEmails = 5)
    {
        using (var client = new ImapClient())
        {
            client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
            client.Authenticate(username, password);

            var inbox = client.Inbox;
            inbox.Open(MailKit.FolderAccess.ReadOnly);

            Console.WriteLine($"Total messages: {inbox.Count}");

            for (int i = 0; i < inbox.Count && i < maxEmails; i++)
            {
                var message = inbox.GetMessage(i);
                Console.WriteLine($"Message {i + 1}:");
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.From}");
            }

            client.Disconnect(true);
        }
    }

    public void DownloadEmailWithAttachments(int messageIndex)
    {
        using (var client = new ImapClient())
        {
            client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
            client.Authenticate(username, password);

            var inbox = client.Inbox;
            inbox.Open(MailKit.FolderAccess.ReadOnly);

            if (messageIndex < 0 || messageIndex >= inbox.Count)
            {
                Console.WriteLine("Invalid message index!");
                return;
            }

            var message = inbox.GetMessage(messageIndex);
            Console.WriteLine($"Downloading attachments for message: {message.Subject}");

            foreach (var attachment in message.Attachments)
            {
                if (attachment is MimePart part)
                {
                    string fileName = part.FileName;
                    using (var stream = File.Create(fileName))
                    {
                        part.Content.DecodeTo(stream);
                    }
                    Console.WriteLine($"Attachment saved: {fileName}");
                }
            }

            if (!message.Attachments.Any())
            {
                Console.WriteLine("Message has no attachments.");
            }

            client.Disconnect(true);
        }
    }

    public void SendEmail(string to, string subject, string body, string senderName, string attachmentPath = "", string replyTo = "")
    {
        if (!IsValidEmail(to))
        {
            Console.WriteLine("Invalid recipient email format!");
            return;
        }

        if (!string.IsNullOrEmpty(replyTo) && !IsValidEmail(replyTo))
        {
            Console.WriteLine("Invalid Reply-To email format!");
            return;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, username));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        if (!string.IsNullOrEmpty(replyTo))
        {
            message.ReplyTo.Add(new MailboxAddress("", replyTo));
        }

        var builder = new BodyBuilder { TextBody = body };
        if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
        {
            builder.Attachments.Add(attachmentPath);
        }
        else if (!string.IsNullOrEmpty(attachmentPath))
        {
            Console.WriteLine("Not found. Sending without attachment.");
        }

        message.Body = builder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate(username, password);
            client.Send(message);
            client.Disconnect(true);
            Console.WriteLine("Email sent successfully!");
        }
    }

    static void Main(string[] args)
    {
        var emailClient = new EmailClient();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Show emails (POP3)");
            Console.WriteLine("2. Show emails (IMAP)");
            Console.WriteLine("3. Download attachments from an email");
            Console.WriteLine("4. Send email");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option (1-5): ");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("How many emails to show? ");
                        if (int.TryParse(Console.ReadLine(), out int pop3Count) && pop3Count > 0)
                        {
                            emailClient.GetEmailsPop3(pop3Count);
                        }
                        else
                        {
                            Console.WriteLine("Showing 5 emails by default.");
                            emailClient.GetEmailsPop3();
                        }
                        break;

                    case "2":
                        Console.Write("How many emails to show? ");
                        if (int.TryParse(Console.ReadLine(), out int imapCount) && imapCount > 0)
                        {
                            emailClient.GetEmailsImap(imapCount);
                        }
                        else
                        {
                            Console.WriteLine("Showing 5 emails by default.");
                            emailClient.GetEmailsImap();
                        }
                        break;

                    case "3":
                        Console.Write("Enter message index to download attachments: ");
                        if (int.TryParse(Console.ReadLine(), out int messageIndex))
                        {
                            emailClient.DownloadEmailWithAttachments(messageIndex);
                        }
                        else
                        {
                            Console.WriteLine("Invalid index!");
                        }
                        break;

                    case "4":
                        Console.Write("Sender name: ");
                        string senderName = Console.ReadLine();
                        Console.Write("Recipient: ");
                        string to = Console.ReadLine();
                        Console.Write("Subject: ");
                        string subject = Console.ReadLine();
                        Console.Write("Message: ");
                        string body = Console.ReadLine();
                        Console.Write("Attachment file path (leave blank for none): ");
                        string attachmentPath = Console.ReadLine();
                        Console.Write("Reply-To email (leave blank for none): ");
                        string replyTo = Console.ReadLine();

                        if (!string.IsNullOrEmpty(to) && !string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(senderName))
                        {
                            emailClient.SendEmail(to, subject, body, senderName, attachmentPath, replyTo);
                        }
                        else
                        {
                            Console.WriteLine("Sender name, recipient, and subject are required!");
                        }
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Invalid option! Please select between 1 and 5.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}