using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Razor.Directives;
using Microsoft.AspNet.Razor.Chunks;
using Microsoft.AspNet.Razor.CodeGenerators;
using Microsoft.AspNet.Razor.Parser;

namespace Anderman.RazorMinify
{
    public class MyMvcRazorHost : IMvcRazorHost
    {
        private readonly MvcRazorHost _host;
        //public MyMvcRazorHost(string root)
        //{
        //    _host = new MvcRazorHost(root);
        //}

        public MyMvcRazorHost(IChunkTreeCache chunkTreeCache)
        {
            _chunkTreeCache = chunkTreeCache;
            _host = new MvcRazorHost(chunkTreeCache);
        }

        public string DefaultNamespace => _host.DefaultNamespace;

        public string MainClassNamePrefix => _host.MainClassNamePrefix;

        public GeneratorResults GenerateCode(string rootRelativePath, Stream inputStream)
        {
            // Adding a prefix so that the main view class can be easily identified.
            var className = MainClassNamePrefix + ParserHelpers.SanitizeClassName(rootRelativePath);
            var engine = new MyRazorTemplateEngine(this,_host);
            return engine.GenerateCode(inputStream, className, DefaultNamespace, rootRelativePath);
        }
        public CodeGenerator DecorateCodeGenerator(CodeGenerator incomingGenerator, CodeGeneratorContext context)
        {

            var inheritedChunkTrees = GetInheritedChunkTrees(context.SourceFile);

            ChunkInheritanceUtility.MergeInheritedChunkTrees(
                context.ChunkTreeBuilder.ChunkTree,
                inheritedChunkTrees,
                _host.DefaultModel);

            return new MyCSharpCodeGenerator(
                context,
                _host.DefaultModel,
                _host.InjectAttribute,
                new GeneratedTagHelperAttributeContext
                {
                    ModelExpressionTypeName = _host.ModelExpressionType,
                    CreateModelExpressionMethodName = _host.CreateModelExpressionMethod
                });
        }
        private ChunkInheritanceUtility _chunkInheritanceUtility;
        private readonly IChunkTreeCache _chunkTreeCache;

        internal ChunkInheritanceUtility ChunkInheritanceUtility
        {
            get
            {
                if (_chunkInheritanceUtility == null)
                {
                    // This needs to be lazily evaluated to support DefaultInheritedChunks being virtual.
                    _chunkInheritanceUtility = new ChunkInheritanceUtility(_host, _chunkTreeCache, _host.DefaultInheritedChunks);
                }

                return _chunkInheritanceUtility;
            }
            set
            {
                _chunkInheritanceUtility = value;
            }
        }

        private IReadOnlyList<ChunkTree> GetInheritedChunkTrees(string sourceFileName)
        {
            var inheritedChunkTrees = _host.GetInheritedChunkTreeResults(sourceFileName)
                .Select(result => result.ChunkTree)
                .ToList();

            return inheritedChunkTrees;
        }

    }
}