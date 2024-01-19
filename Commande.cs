
namespace ContactManager;

public abstract class Commande 
{
    public List<string> NomDesCommandes;

    public Commande(params string[] NomDesCommandes) : this(NomDesCommandes.ToList()) { }
    public Commande(List<string> nomDesCommande) 
    {
        NomDesCommandes = nomDesCommande.Select(s=>s.ToLowerInvariant()).ToList();  
    }

    public virtual bool NomCorrespond(CommandeArg args) => NomDesCommandes.Contains(args.NomCommande);

    public virtual void Execute(Terminal terminal, CommandeArg args) { }
    public virtual string Description { get => "Commande non documenté"; }
}

public class CommandeArg : List<string>
{
    public string NomCommande => this[0].ToLowerInvariant();

    public string Pop() 
    {  
        if(Count == 0) { return ""; }
        var val = this[0];
        RemoveAt(0);
        return val;
    }

    public CommandeArg(List<string> args) 
    {
        AddRange(args);
    }
}