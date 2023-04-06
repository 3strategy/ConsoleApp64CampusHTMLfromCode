using HtmlSyntaxHighlighterDotNet; //Nuget package, also available as git https://github.com/smack0007/HtmlSyntaxHighlighterDotNet

namespace ConsoleApp64CampusHTMLfromCode
{
  internal class Program
  {
    static void Main(string[] args  )
    {
      string text;
      if (args.Length == 0)
      {
        Console.WriteLine("Using an HTML Example.\nplease drag and drop a file or use\nright click -> SendTo");
        //use default file path if one is missing
        text = $@"
static void Main(string[] args)
{{
  for (int i = 0; i < 10; i++)
  {{
    Console.WriteLine($""i is {{i}}"");
  }}
  Console.WriteLine(""Done!"");
}}";
        args= new string[] { @"C:\temp\test.cs"};
      }

      else
        text = File.ReadAllText(args[0] );
      //convert to html
      var syntaxHtml = HtmlSyntaxHighlighter.TransformCSharp(text);
      //string text1 = ConvertToHtml(text);
      //save with html extension.
      string path = Path.GetDirectoryName(args[0])
         + Path.DirectorySeparatorChar
         + Path.GetFileNameWithoutExtension(args[0]);
      string pathEmbeded = path + ".Embeded.html";
      string pathConverted = path +".Converted.html";// Path.GetExtension(args[0]);
      

      var fullHtml =
$@"<!DOCTYPE html>
<html>
    <head>
        <style>
{HtmlSyntaxHighlighter.GetCssStyles()}
        </style>
    </head>
    <body>
        <pre>
            <code>{syntaxHtml}</code>  
        </pre>
    </body>
</html>";

      var embeddedHtml =
$@"  <pre>
    <code>{syntaxHtml}</code>  
  </pre>";

      File.WriteAllText(pathConverted, fullHtml);
      File.WriteAllText(pathEmbeded, embeddedHtml);
    }
  }
}