// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");
Console.WriteLine("Wpisz tekst:");
var text = Console.ReadLine();
if (text is null)
{
    Console.WriteLine("Cos poszlo nie tak!!!");
    return;
}

Console.WriteLine($"Wpisany tekst: \"{text}\"");