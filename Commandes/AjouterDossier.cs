namespace ContactManager.Commandes;

public class AjouterDossier : Commande
{
    public override string Description => "Ajoute un dossier dans le dossier actuel et se déplace dedans";

    public AjouterDossier() : base("mkdir", "ajouterdossier", "dossier") { }

    public override void Execute(Terminal t, CommandeArg args)
    {
        if(args.Count < 2) 
        {
            t.Erreur("Syntaxe : ");
            t.Erreur(args.NomCommande + " NomDuDossier");
            t.Erreur(args.NomCommande + " NomDuDossier/SousDossier1/SousDossier2/SousDossierN");
            return;
        }

        t.Actif = t.Actif.DossierSuperieur();


        args.Pop();
        while(args.Count != 0) 
        {
            if (t.Actif is Dossier d)
            {
                var sousDossierNom = args.Pop();
                if (d.Contient(sousDossierNom)) 
                {
                    t.Erreur("L'élément " + sousDossierNom + " existe déjà");
                    return;
                }
                var sousDossier = new Dossier(d, sousDossierNom);
                d.Enfants.Add(sousDossier);
                t.Actif = sousDossier;
                t.Succès("Le dossier " + sousDossierNom + " a bien été crée");
            }
            else
            {
                throw new Exception("ça devrait pas arriver");
            }
        }
    }
}