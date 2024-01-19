namespace ContactManager.Commandes;

public class Afficher : Commande
{
    public override string Description => "Afficher la racine";

    public Afficher() : base("ls", "afficher", "display") { }

    public override void Execute(Terminal t, CommandeArg args)
    {
        t.Racine.AfficherRecursif(t);
    }
}
