using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Emailer : MonoBehaviour
{
    const string kSenderPassword = "rwfj dfxr thtl mfqy ";
    const string kReceiverEmailAddress = "gunlike2data@gmail.com";
    void Start()
    { 
        SendAnEmail("Test message, holy moly!", "Test : 2");
    }
    public static void SendAnEmail(string message, string header)
    {
        var fromEmail = kReceiverEmailAddress;

        var mail = new MailMessage();
        mail.From = new MailAddress(fromEmail);
        mail.To.Add(kReceiverEmailAddress);

        mail.Subject = header;
        mail.Body = message;

        string host = "gmail";
        var smtpServer = new SmtpClient();
        switch (host)
        {
            case "gmail": 
                smtpServer = new SmtpClient("smtp.gmail.com"); // Gmail smtp client
                smtpServer.Port = 587; // Gmail smtp port
                break;
            case "outlook":
                smtpServer = new SmtpClient("smtp-mail.outlook.com"); // Outlook smtp client
                smtpServer.Port = 587; // Outlook smtp port
                break;
        }
        

        smtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, kSenderPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        smtpServer.Send(mail);
    }
}
