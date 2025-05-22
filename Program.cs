using System.CommandLine;

var rootCommand = new RootCommand("Sample app for System.CommandLine");
var option = new Option<string>(
    "--name",
    description: "The name of the person to greet",
    getDefaultValue: () => "World");
rootCommand.AddOption(option);
rootCommand.SetHandler((name) =>
{
    Console.WriteLine($"Hello, {name}!");
}, option);

await rootCommand.InvokeAsync(args);
