
using System.Net.Mail;
using static ContactManager.Contacte;

namespace ContactManager.Commandes;

public class AjouterContacte : Commande
{
    public override string Description => "Ajouter un contacte";

    public AjouterContacte() : base("touch", "ajoutercontact", "mkfile") { }

    public override void Execute(Terminal t, CommandeArg args)
    {
        if (args.Count != 6)
        {
            t.Erreur("Syntaxe : ");
            t.Erreur(args.NomCommande + " Nom Prenom Entreprise Email Link={" + string.Join(", ", Enum.GetNames(typeof(Lien))) + "}");
            return;
        }
        args.Pop();


        bool formatEstBon = true;

        string prenom = args.Pop();
        string nom = args.Pop();
        string entreprise = args.Pop();
        string email = args.Pop();


        try
        {
            MailAddress m = new(email);
        }
        catch
        {
            t.Erreur("Adresse mail " + email + " non valide");
            formatEstBon = false;
        }

        string lienStr = args.Pop();

        if (Enum.TryParse<Lien>(lienStr, out Lien lien) == false)
        {
            formatEstBon = false;

            t.Erreur("Type de lien non valide. Les liens sont : " + string.Join(", ", Enum.GetNames(typeof(Lien))));
        }

        if (!formatEstBon)
        {
            return;
        }

        Dossier d = t.Actif.DossierSuperieur();
        t.Actif = d;

        string nomFichier = prenom;

        if (d.Contient(nomFichier))
        {
            t.Erreur("Le fichier " + nomFichier + " existe déjà !"); return;
        }

        Contacte c = new(d, nomFichier);
        d.Enfants.Add(c);

        c.Nom = nom;
        c.Prenom = prenom;
        c.Société = entreprise;
        c.Courriel = email;
        c.TypeDeLien = lien;
        t.Succès("La personne " + prenom + " a bien été ajouté");
    }
}