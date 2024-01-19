namespace ContactManager.Commandes;

public class Sortir : Commande
{
    public override string Description => "Sortir du programme";

    public Sortir() : base("exit", "sortir", "byebye") { }

    public override void Execute(Terminal t, CommandeArg args)
    {
        t.MonEtat = Terminal.Etat.Fin;
        Console.WriteLine("Au revoir");
    }
}