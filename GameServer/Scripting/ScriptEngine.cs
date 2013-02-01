using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GameServer.Scripting
{
    public class ScriptEngine
    {
        protected CompilerParameters CompilerParameters;
        protected CodeDomProvider CodeProvider;
        protected CompilerResults Results;
        protected string Directory;
        protected bool Compiled;
        protected Dictionary<string, CompilerResults> Scripts;

        public ScriptEngine(string Directory)
        {
            this.Directory = Directory;

            Scripts = new Dictionary<string, CompilerResults>();

            CodeProvider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters = new CompilerParameters();
            CompilerParameters.GenerateExecutable = false;
            CompilerParameters.GenerateInMemory = true;

            var ReferencedAssemblies = AppDomain.CurrentDomain
                                        .GetAssemblies()
                                        .Where(Assembly => !Assembly.IsDynamic)
                                        .Select(Assembly => Assembly.Location);
            CompilerParameters.ReferencedAssemblies.AddRange(ReferencedAssemblies.ToArray());

            Compiled = false;

            Compile("Default.cs");

        }


        public void Compile(string Location)
        {
            string Key = Location;
            Location = Directory + "\\" + Location;
            if (File.Exists(Location))
            {
                CompilerResults Results = InternalCompile(File.ReadAllLines(Location));
                if (Compiled)
                {
                    Scripts.ThreadSafeAdd(Key, Results);
                }
                else
                {
                    foreach (CompilerError Error in Results.Errors)
                    {
                        Console.WriteLine(Error.ToString());
                        Console.WriteLine();
                    }
                }
            }
        }

        private CompilerResults InternalCompile(string[] Source)
        {
            CompilerResults Results = CodeProvider.CompileAssemblyFromSource(CompilerParameters, Source);
            Compiled = Results.Errors.Count == 0;
            return Results;
        }
    }
}