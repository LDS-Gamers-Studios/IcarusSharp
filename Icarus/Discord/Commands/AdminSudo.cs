using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

using System.Reflection;
using System.Runtime.Loader;

namespace Icarus.Discord.Commands
{
    public partial class Admin
    {
        [DiscordEventHandler("MessageCreated")]
        public static async Task MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Message.Content is null || !e.Message.Content.StartsWith("!sudo"))
            {
                return;
            }

            var sudoPermitted = DiscordBotService.Configuration.GetSection("discord:sudoPermitted").Get<string[]>();
            if (!sudoPermitted.Contains(e.Author.Id.ToString()))
            {
                return;
            }

            var code = e.Message.Content["!sudo".Length..].Trim();
            var initialCode = code = code.StartsWith("```cs") ? code.Substring("```cs".Length, code.Length - "```cs```".Length) : code;
            var template = File.ReadAllText("SudoTemplate.cs");
            code = template.Replace("{input}", code);


            var compileStartedAt = DateTime.Now;
            var compiled = Compiler.Compile(code, out var diagnostics, out var success);
            var compileTime = (DateTime.Now - compileStartedAt).TotalMilliseconds;

            if (!success)
            {
                var embedError = e.Message.IcarusEmbed()
                    .WithColor(DiscordColor.Red)
                    .WithTitle("Compilation Failed")
                    .AddField("Compile Time", compileTime + "ms", true);

                foreach (var d in diagnostics)
                {
                    if (d.Severity < DiagnosticSeverity.Error) { continue; }
                    embedError.AddField(d.Severity.ToString() + " - " + d.Id, d.ToString(), true);
                }

                await e.Message.RespondAsync(embedError);
                return;
            }

            var embed = e.Message.IcarusEmbed()
                .WithTitle("Running")
                .AddField("Input Code", $"```cs\n{initialCode}\n```", false)
                .AddField("Compile Time", compileTime + "ms", true)
                .WithColor(DiscordColor.White);
            var msg = await e.Channel.SendMessageAsync(embed);

            var timeStarted = DateTime.Now;
            double? runtime = null;

            try
            {
                var asm = Compiler.GetAssembly(compiled);
                var type = asm.GetType("Icarus.SudoTemplate");
                var method = type.GetMethod("Sudo");
                var constructor = type.GetConstructor(Array.Empty<Type>());
                var instance = constructor.Invoke(Array.Empty<object>());
                type.GetProperty("Msg").SetValue(instance, e.Message, null);

                timeStarted = DateTime.Now;
                await (Task)method.Invoke(instance, null);
                var result = type.GetProperty("Out").GetValue(instance, null);
                runtime = (DateTime.Now - timeStarted).TotalMilliseconds;

                embed = embed.WithColor(DiscordColor.Green)
                    .WithTitle("Success")
                    .AddField("Runtime", runtime + "ms", true);
                if (result is not null)
                {
                    embed = embed.WithDescription(result.ToString());
                }

                await msg.ModifyAsync(embed.Build());
            }
            catch (Exception ex)
            {
                runtime ??= (DateTime.Now - timeStarted).TotalMilliseconds;
                embed = embed.WithColor(DiscordColor.Red)
                    .WithDescription(ex.ToString())
                    .WithTitle("Failure")
                    .AddField("Runtime", runtime + "ms", true);

                await msg.ModifyAsync(embed.Build());
            }
        }
    }

    public static class Compiler
    {
        public static byte[] Compile(string code, out Diagnostic[] diagnostics, out bool success)
        {
            diagnostics = null;

            using var peStream = new MemoryStream();
            var result = GenerateCode(code).Emit(peStream);
            success = result.Success;
            diagnostics = result.Diagnostics.ToArray();

            if (!result.Success)
            {
                return null;
            }

            peStream.Seek(0, SeekOrigin.Begin);

            return peStream.ToArray();
        }

        private static CSharpCompilation GenerateCode(string sourceCode)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp7_3);

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);

            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && a.Location is not null && a.Location != "")
                .Select(a => MetadataReference.CreateFromFile(a.Location));

            return CSharpCompilation.Create("compiledEvaluation.dll", new[] { parsedSyntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication,
                optimizationLevel: OptimizationLevel.Release,
                assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
        }

        public static Assembly GetAssembly(byte[] compiledAssembly)
        {
            using var asm = new MemoryStream(compiledAssembly);
            var assemblyLoadContext = new AssemblyLoadContext("compiledEvaluation.dll", true);
            return assemblyLoadContext.LoadFromStream(asm);
        }
    }
}
