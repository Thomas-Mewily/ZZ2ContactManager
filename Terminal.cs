using ContactManager.Commandes;
using System;
using System.Collections;

namespace ContactManager;

public class Terminal : IEnumerable<Commande>
{
    public enum Etat { Attente, EnCours, Fin };

    public Etat MonEtat { get; set; }
    public List<Commande> Commandes = new();

    public Dossier Racine;
    public DbElement Actif;

    public Terminal() 
    {
        Racine = Dossier.Racine();
        Actif = Racine;

        ImporterCommandes();
        MonEtat = Etat.Attente;
    }

    private void ImporterCommandes() 
    {
        Commandes = new Espion().GetAllTIn<Commande>(GetType().Assembly, "ContactManager.Commandes");
    }

    public void Erreur(string erreur) 
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(erreur);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void Succès(string succes)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(succes);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void ExecuteUneCommande() 
    {
        MonEtat = Etat.Attente;

        CommandeArg args;

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("> ");
            Console.ForegroundColor = ConsoleColor.White;
            string s = Console.ReadLine()!;

            args = new(s.Split(new string[] { " ", "/" }, StringSplitOptions.TrimEntries).Where(t=>!string.IsNullOrWhiteSpace(t)).ToList());

            if (args.Count == 0)
            {
                Erreur("Aucune commande rentrée. Essaies la commande Aide\n");
                continue;
            }

            break;
        }

        MonEtat = Etat.EnCours;
        Commande? c = null;
        foreach (var cmd in Commandes)
        {
            if (cmd.NomCorrespond(args)) 
            {
                c = cmd;
                break;
            }
        }

        if(c != null) 
        {
            c.Execute(this, args);
            Console.WriteLine();
        }
        else 
        {
            Erreur("Commande Inconnu. Essaies la commande Aide");
        }

        if (MonEtat == Etat.EnCours) { MonEtat = Etat.Attente; }
    }

    public void Run() 
    {
        MonEtat = Etat.EnCours;
        while (MonEtat != Etat.Fin) 
        {
            ExecuteUneCommande();
        }
    }

    public IEnumerator<Commande> GetEnumerator() => ((IEnumerable<Commande>)Commandes).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Commandes).GetEnumerator();
}