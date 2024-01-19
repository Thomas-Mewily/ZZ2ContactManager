using System;
using System.Reflection;
using System.Text;

namespace ContactManager;

/// <summary>
/// Made by Thomas T.
/// </summary>
public class Espion
{
    public Espion() { }

    public List<Type> GetAllTypeIn(Assembly assembly, string prefix)
    {
        List<Type> result = new();
        var types = assembly.GetTypes().Where(t => t.FullName!.StartsWith(GetType()!.Namespace!));
        bool Continue;
        do
        {
            Continue = false;
            foreach (var t in types)
            {
                try
                {
                    if (t.IsAbstract == false && t.IsGenericType == false && t.FullName!.StartsWith(prefix))
                    {
                        result.Add(t);
                    }
                }
                catch
                {

                }
            }
        } while (Continue);
        return result;
    }

    public List<T> GetAllTIn<T>(Assembly assembly, string prefix) 
    {
        List<T> instances = new();
        var l = GetAllTypeIn(assembly, prefix);
        foreach (var type in l)
        {
            try
            {
                T? obj = CreateInstanceOrException<T>(type) ?? throw new Exception(type.ToString() + " l'instanciation a été null");
                instances.Add(obj);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("Importation de la commande ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(type.FullName + "...");
            }
            catch { }
        }
        Console.WriteLine();
        return instances;
    }

    public T? CreateInstanceOrException<T>(Type type) => (T?)Activator.CreateInstance(type);
}
