
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using System.Xml.Serialization;
using static ContactManager.Contacte;

namespace ContactManager.Commandes;

public class Aide : Commande
{
    public override string Description => "Affiche toute les commandes";

    public Aide() : base("help", "aide") { }

    public override void Execute(Terminal t, CommandeArg args)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Aide :");
        Console.ForegroundColor = ConsoleColor.White;
        
        foreach (var cmd in t) 
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" - ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(string.Join(", ", cmd.NomDesCommandes));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(cmd.Description);
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}

