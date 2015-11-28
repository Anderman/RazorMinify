using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Razor.CodeGenerators;
using Microsoft.AspNet.Razor.CodeGenerators.Visitors;

namespace Anderman.RazorMinify
{
    public class MyCSharpCodeGenerator : MvcCSharpCodeGenerator
    {
        private GeneratedTagHelperAttributeContext _tagHelperAttributeContext;

        public MyCSharpCodeGenerator(CodeGeneratorContext context, string defaultModel, string injectAttribute, GeneratedTagHelperAttributeContext tagHelperAttributeContext) : base(context, defaultModel, injectAttribute, tagHelperAttributeContext)
        {
            _tagHelperAttributeContext = tagHelperAttributeContext;
        }
        protected override CSharpCodeVisitor CreateCSharpCodeVisitor(CSharpCodeWriter writer, CodeGeneratorContext context)
        {
            var csharpCodeVisitor = new MyCSharpCodeVisitor(writer, context);

            csharpCodeVisitor.TagHelperRenderer.AttributeValueCodeRenderer =
                new MvcTagHelperAttributeValueCodeRenderer(_tagHelperAttributeContext);

            return csharpCodeVisitor;
        }

    }
}