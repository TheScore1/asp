using System.Collections.ObjectModel;
using System.Reflection;

namespace Core.Helpers;

public static class AssemblyHelper
{
    private static readonly IReadOnlyList<string> DefaultAssemblies =
        new ReadOnlyCollection<string>(["System", "Microsoft"]);

    public static IEnumerable<Assembly> GetAllAssembliesWithoutDefaultAssemblies()
    {
        var usedAssemblies = new HashSet<string>();
        var stack = new Stack<Assembly>();

        stack.Push(Assembly.GetEntryAssembly()!);

        do
        {
            var asm = stack.Pop();

            yield return asm;

            foreach (var reference in asm.GetReferencedAssemblies())
                if (!usedAssemblies.Contains(reference.FullName) &&
                    DefaultAssemblies.All(a => !reference.FullName.StartsWith(a)))
                {
                    stack.Push(Assembly.Load(reference));
                    usedAssemblies.Add(reference.FullName);
                }
        } while (stack.Count > 0);
    }
}