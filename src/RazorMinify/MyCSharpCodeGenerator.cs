using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Razor.CodeGenerators;
using Microsoft.AspNet.Razor.CodeGenerators.Visitors;

namespace Anderman.RazorMinify
{
    public class MyCSharpCodeGenerator : MvcCSharpCodeGenerator
    {

        protected override CSharpCodeVisitor CreateCSharpCodeVisitor(CSharpCodeWriter writer, CodeGeneratorContext context)
        {
            return new MyCSharpCodeVisitor(writer, context);
        }

        public MyCSharpCodeGenerator(CodeGeneratorContext context, string defaultModel, string injectAttribute, GeneratedTagHelperAttributeContext tagHelperAttributeContext) : base(context, defaultModel, injectAttribute, tagHelperAttributeContext)
        {
        }
    }
}