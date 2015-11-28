using System;
using System.Diagnostics;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Razor.Chunks;
using Microsoft.AspNet.Razor.CodeGenerators;
using Microsoft.AspNet.Razor.CodeGenerators.Visitors;


namespace Anderman.RazorMinify
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".

    public class MyCSharpCodeVisitor : CSharpCodeVisitor
    {
        public MyCSharpCodeVisitor(CSharpCodeWriter writer, CodeGeneratorContext context) : base(writer, context)
        {
            Writer = writer;
            Context = context;
        }

        protected CSharpCodeWriter Writer { get; private set; }
        protected CodeGeneratorContext Context { get; private set; }

        protected override void Visit(LiteralChunk chunk)
        {
            chunk.Text = chunk.Text.Replace("\t", " ").Replace("\r", " ").Replace("\n", " ").Replace("        ", " ").Replace("    ", " ").Replace("  ", " ");

            if (Context.Host.DesignTimeMode || string.IsNullOrEmpty(chunk.Text))
            {
                // Skip generating the chunk if we're in design time or if the chunk is empty.
                return;
            }

            if (Context.Host.EnableInstrumentation)
            {
                Writer.WriteStartInstrumentationContext(Context, chunk.Association, true);
            }

            if (Context.ExpressionRenderingMode == ExpressionRenderingMode.WriteToOutput)
            {
                RenderPreWriteStart();
            }

            Writer.WriteStringLiteral(chunk.Text);

            if (Context.ExpressionRenderingMode == ExpressionRenderingMode.WriteToOutput)
            {
                Writer.WriteEndMethodInvocation();
            }

            if (Context.Host.EnableInstrumentation)
            {
                Writer.WriteEndInstrumentationContext(Context);
            }
        }

        private CSharpCodeWriter RenderPreWriteStart()
        {
            return RenderPreWriteStart(Writer, Context);
        }
    }
}