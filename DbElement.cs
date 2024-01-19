
using System.Xml.Serialization;

namespace ContactManager;

public abstract class DbElement 
{
    public string     NomElement = "~";
    [XmlIgnore] // Avoid circular reference
    public DbElement? Parents = null;

    public DbElement() { Parents = null; NomElement = ""; }
    public DbElement(DbElement? parents, string nomElement)
    {
        Parents = parents;
        NomElement = nomElement;
    }

    public virtual void AfficherElement(Terminal t) 
    {
        if (ReferenceEquals(this, t.Actif))
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        Console.Write(NomElement);
        Console.ForegroundColor = ConsoleColor.White; 
    }

    public void AfficherRecursif(Terminal t, string indent = "", bool last = true, bool writeIndent = true)
    {
        if (writeIndent)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(indent);
            if (last)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "│ ";
            }
        }

        Console.ForegroundColor = ConsoleColor.White;
        AfficherElement(t);
        AfficherEnfants(t, indent, last, writeIndent);
        Console.ForegroundColor = ConsoleColor.White;
    }

    protected virtual void AfficherEnfants(Terminal t, string indent, bool last = true, bool writeIndent = true) { }

    /// <summary>
    /// On remonte l'arborescence si l'on n'est pas dans un dossier
    /// </summary>
    /// <returns></returns>
    public Dossier DossierSuperieur() 
    {
        DbElement actif = this;
        while (actif is not Dossier)
        {
            actif = actif.Parents!;
        }
        return (Dossier)actif;
    }

    public virtual void FaireLesLiensDeParenté() { }
}

public class Dossier : DbElement
{
    public List<DbElement>  Enfants   = new();

    public DbElement? Trouver(string nom) => Enfants.FirstOrDefault(t => t.NomElement == nom.ToLowerInvariant());
    public bool Contient(string nom) => Trouver(nom) != null;

    public Dossier() : base() { }
    public Dossier(DbElement? parents, string nomDeDossier) : base(parents, nomDeDossier) { }

    public static Dossier Racine() => new(null, "~");

    protected override void AfficherEnfants(Terminal t, string indent, bool last = true, bool writeIndent = true)
    {
        for (int i = 0; i < Enfants.Count; i++)
        {
            Console.WriteLine();
            Enfants[i].AfficherRecursif(t, indent, (i == Enfants.Count - 1), writeIndent);
        }
    }

    public override void FaireLesLiensDeParenté() 
    {
        foreach(var e in Enfants) 
        {
            e.Parents = this;
            e.FaireLesLiensDeParenté();
        }
    }
}


public class Contacte : DbElement
{
    public enum Lien { Ami, Collegue, Relation, Reseau, Inconnu };

    // public pour la sérialisation
    public string _Nom        = "";
    public string _Prenom     = "";
    public string _Courriel   = "";
    public string _Société    = "";
    public Lien   _TypeDeLien = Lien.Inconnu;

    public string Nom      { get => _Nom       ; set { _Nom        = value; UpdateDateModification(); } }
    public string Prenom   { get => _Prenom    ; set { _Prenom     = value; UpdateDateModification(); } }
    public string Courriel { get => _Courriel  ; set { _Courriel   = value; UpdateDateModification(); } }
    public string Société  { get => _Société   ; set { _Société    = value; UpdateDateModification(); } }
    public Lien TypeDeLien { get => _TypeDeLien; set { _TypeDeLien = value; UpdateDateModification(); } }

    private void UpdateDateModification() => DateModification = DateTime.Now;

    public DateTime DateCreation;
    public DateTime DateModification;

    public Contacte() : base() { }
    public Contacte(DbElement? parents, string nomDeContacte) : base(parents, nomDeContacte)
    {
        DateCreation     = DateTime.Now;
        DateModification = DateTime.Now;
    }

    public override void AfficherElement(Terminal t)
    {
        base.AfficherElement(t);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(new string(' ', 9) + Nom + " " + Prenom + " (" + Société + "), " + Courriel + ", link:" + TypeDeLien);
        Console.ForegroundColor = ConsoleColor.White;
    }
}