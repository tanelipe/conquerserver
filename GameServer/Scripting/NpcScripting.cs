using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
namespace GameServer.Scripting
{
    public class NpcScripting
    {
        private Dictionary<string, CompilerResults> CompiledScripts;
        private CompilerParameters Parameters;
        private CodeDomProvider CodeProvider;
       
        public NpcScripting()
        {
            CompiledScripts = new Dictionary<string, CompilerResults>();
            CodeProvider = CodeDomProvider.CreateProvider("CSharp");
            Parameters = new CompilerParameters();
            Parameters.GenerateExecutable = false;
            Parameters.GenerateInMemory = true;

            var assemblies = AppDomain.CurrentDomain
                                .GetAssemblies()
                                .Where(assembly => !assembly.IsDynamic)
                                .Select(assembly => assembly.Location);
            Parameters.ReferencedAssemblies.AddRange(assemblies.ToArray());
        }
        public bool Compile(uint ID, out string ScriptFile)
        {
            string ScriptDirectory = "..\\..\\Npc Scripts\\";
            ScriptFile = ID.ToString() + ".cs";
            if (!File.Exists(ScriptDirectory + ScriptFile))
                ScriptFile = "Default.cs";

            if (!CompiledScripts.ContainsKey(ScriptFile))
            {
                CompilerResults Results = CodeProvider.CompileAssemblyFromFile(Parameters, ScriptDirectory + ScriptFile);
                if (Results.Errors.Count == 0)
                {
                    CompiledScripts.ThreadSafeAdd(ScriptFile, Results);
                }
            }
            return CompiledScripts.ContainsKey(ScriptFile);
        }

        public void Initialize(GameClient Client, uint ID)
        {
            string ScriptFile = "";
            if (Compile(ID, out ScriptFile))
            {
                Client.ActiveNPC = ID;

                CompilerResults Results = CompiledScripts[ScriptFile];
                object ClassInstance = Results.CompiledAssembly.CreateInstance("NpcScriptEngine.NpcDialog");
                if (ClassInstance != null)
                {
                    MethodInfo Method = ClassInstance.GetType().GetMethod("Initialize");
                    if (Method != null)
                    {
                        Method.Invoke(ClassInstance, new object[] { Client });
                    }
                }
            }
        }
        public void Process(GameClient Client, byte OptionID, string Input)
        {
            string ScriptFile = "";
            if (Compile(Client.ActiveNPC, out ScriptFile))
            {
                CompilerResults Results = CompiledScripts[ScriptFile];
                object ClassInstance = Results.CompiledAssembly.CreateInstance("NpcScriptEngine.NpcDialog");
                if (ClassInstance != null)
                {
                    MethodInfo Method = ClassInstance.GetType().GetMethod("Process");
                    if (Method != null)
                    {
                        Method.Invoke(ClassInstance, new object[] { Client, OptionID, Input });
                    }
                }
            }
        }
        public void Clear()
        {
            CompiledScripts.Clear();
        }
    }
}