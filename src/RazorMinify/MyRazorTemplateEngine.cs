using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Razor;
using Microsoft.AspNet.Razor.CodeGenerators;

namespace Anderman.RazorMinify
{
    public class MyRazorTemplateEngine : RazorTemplateEngine
    {
        private MyMvcRazorHost _myHost;

        public MyRazorTemplateEngine(MyMvcRazorHost myHost, MvcRazorHost host) : base(host)
        {
            _myHost = myHost;
        }

        protected override CodeGenerator CreateCodeGenerator(CodeGeneratorContext context)
        {
            return _myHost.DecorateCodeGenerator(Host.CodeLanguage.CreateCodeGenerator(context), context);
        }
    }
}