using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Globalization;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
namespace GameServer.Scripting
{
    partial class NpcScript
    {

    }



    public class NpcScripting
    {
   
        public void Compile(GameClient Client, uint ID)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;

            var assemblies = AppDomain.CurrentDomain
                                .GetAssemblies()
                                .Where(assembly => !assembly.IsDynamic)
                                .Select(assembly => assembly.Location);
            parameters.ReferencedAssemblies.AddRange(assemblies.ToArray());

            string ScriptFile = "..\\..\\Npc Scripts\\";
            if (!File.Exists(ScriptFile + ID.ToString() + ".cs"))
                ScriptFile = ScriptFile + "Default.cs";
            else
                ScriptFile = ScriptFile + ID.ToString() + ".cs";

            CompilerResults results = provider.CompileAssemblyFromFile(parameters, ScriptFile);
            if (results.Errors.Count == 0)
            {     
                var instance = results.CompiledAssembly.CreateInstance("NpcScriptEngine.NpcDialog");
                if (instance != null)
                {
                    Type type = instance.GetType();
                    type.GetMethod("Process").Invoke(instance, new object[] { Client, ID, 0, "" });
                }
            }
            else
            {
                foreach (CompilerError error in results.Errors)
                {
                    Console.WriteLine(error.ToString());
                    Console.WriteLine();
                }
            }

        }
    }
}
