﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;


namespace Shiny.Generators.Tests
{
    public class AssemblyGenerator
    {
        readonly List<MetadataReference> references = new List<MetadataReference>();
        readonly List<SyntaxTree> sources = new List<SyntaxTree>();


        public void AddSource(string sourceText)
        {
            var source = SourceText.From(sourceText, Encoding.UTF8);
            var tree = CSharpSyntaxTree.ParseText(source);
            this.sources.Add(tree);
        }


        public void AddReference(string assemblyName, bool autoAddAbstraction = true)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName);
            var ass = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(x => x.GetName().Name.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

            if (ass == null)
                throw new ArgumentException($"Assembly '{assemblyName}' not found at '{path}'");

            if (autoAddAbstraction)
                this.AddReference(assemblyName + ".Abstractions", false);

            var reference = MetadataReference.CreateFromFile(ass.Location);
            this.references.Add(reference);
        }


        public CSharpCompilation Create(string assemblyName)
        {
            this.AddReference("Shiny.Core", true);
            var localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName + ".dll");
            return CSharpCompilation
                .Create(localPath)
                .WithReferences(this.references)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddSyntaxTrees(this.sources);
        }
    }
}