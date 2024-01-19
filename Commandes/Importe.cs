using System.Xml.Serialization;

namespace ContactManager.Commandes;

public class Importe : Commande
{
    public override string Description => "Importe la sauvegarde depuis le disque";

    public Importe() : base("import", "importe", "fopen", "ouvrir") { }

    public override void Execute(Terminal t, CommandeArg args)
    {
        if (args.Count != 2)
        {
            t.Erreur("Syntaxe : ");
            t.Erreur(args.NomCommande + " <clé de cryptage>");
            return;
        }
        args.Pop();
        var cle = args.Pop();

        try 
        {
            string xmlEncoded = "";
            try 
            {
                xmlEncoded = Encoder.DecryptTextFromFile(Enregistrer.SavePath, cle);
            }
            catch
            {
                t.Erreur("Mot de passe invalide");
                return;
            }

            XmlSerializer serializer = new(typeof(DbElement), new[] { typeof(DbElement), typeof(Dossier), typeof(Contacte) });

            using TextReader reader = new StringReader(xmlEncoded);

            DbElement? racine = (DbElement?)serializer.Deserialize(reader);
            if(racine == null) { t.Erreur("L'objet désérialisé a été null"); return; }
            t.Racine = (Dossier)racine!;
            t.Actif = racine!;
            t.Racine.FaireLesLiensDeParenté();
            t.Succès("L'importation depuis " + Enregistrer.SavePath + " a réussi");
            reader.Close();
        }
        catch (Exception ex)
        {
            t.Erreur("Erreur lors de l'importation du fichier à " + Enregistrer.SavePath);
            t.Erreur(ex.Message);
            return;
        }
    }
}