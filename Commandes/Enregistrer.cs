using System.Xml.Serialization;

namespace ContactManager.Commandes;

public class Enregistrer : Commande
{
    public override string Description => "Enregistrer l'arborescence sur le disque";

    public Enregistrer() : base("save", "enregistrer") { }

    public static string SavePath { get; private set; } = Path.Combine(Directory.GetCurrentDirectory(), "arborescence.db");

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
            XmlSerializer serializer = new(typeof(DbElement), new[] { typeof(DbElement), typeof(Dossier), typeof(Contacte) });
            using StringWriter textWriter = new();
            serializer.Serialize(textWriter, t.Racine);

            var xml = textWriter.ToString();
            textWriter.Close();

            Encoder.EncryptTextToFile(xml, SavePath, cle);
            t.Succès("Fichier enregistré avec cryptage à " + SavePath);
        }
        catch (Exception ex)
        {
            t.Erreur(ex.Message);
            t.Erreur("Erreur lors de l'enregistrement du fichier à " + SavePath);
            return;
        }
    }
}

