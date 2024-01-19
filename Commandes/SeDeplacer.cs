namespace ContactManager.Commandes;

public class SeDeplacer : Commande
{
    public override string Description => "Se déplacer dans l'arborescence des dossiers et fichiers";

    public SeDeplacer() : base("cd", "deplacer", "move") { }

    public override void Execute(Terminal t, CommandeArg args)
    {
        if (args.Count < 2)
        {
            t.Erreur("Syntaxe : ");
            t.Erreur(args.NomCommande + " ../DossierParent");
            t.Erreur(args.NomCommande + " ./MonDossierRelatif");
            t.Erreur(args.NomCommande + " ~/DepuisLaRacine");
            return;
        }

        DbElement path = t.Actif;
        args.Pop();

        while (args.Count != 0)
        {
            string actuel = args.Pop();

            switch (actuel)
            {
                case "..":
                    {
                        if (path.Parents == null)
                        {
                            t.Erreur("L'élément " + path.NomElement + " n'a pas de parent."); return;
                        }
                        path = path.Parents;
                    }
                    break;
                case ".": path = t.Actif; break;
                case "~": path = t.Racine; break;
                default:
                    {
                        if (path is Dossier d)
                        {
                            var enfant = d.Trouver(actuel);
                            if (enfant == null)
                            {
                                t.Erreur("Fichier ou dossier " + actuel + " introuvable"); return;
                            }
                            path = enfant;
                        }
                        else
                        {
                            args.Clear();
                        }
                    }
                    break;
            }
        }
        t.Actif = path;
    }
}
