using CsharpToColouredHTML.Core;
using HtmlSyntaxHighlighterDotNet; //Nuget package, also available as git https://github.com/smack0007/HtmlSyntaxHighlighterDotNet
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp64CampusHTMLfromCode
{
  internal class Program
  {
    static void Main(string[] args)
    {
      string text;
      if (args.Length == 0)
      {
        Console.WriteLine("Using an HTML Example.\nplease drag and drop a file or use\nright click -> SendTo");
        //use default file path if one is missing
        text = $@"
using System;
namespace ConsoleApp64
{{
  public class Program
  {{
    static void Main(string[] args)
    {{
      Console.WriteLine(""Hello, World!"");
      int[] arr = {{ 1, 3, 5 }};
      foreach (var item in arr)
        Console.WriteLine(item);
    }}
  }}
  class MyClass
  {{
    public MyClass()
    {{
      while (false)
      {{
      }}
    }}
    public bool Test(int x)
    {{
      return true;
    }}
  }}
}}";
        args = new string[] { @"C:\temp\test.cs" };
      }

      else
        text = File.ReadAllText(args[0]);

      //string text1 = ConvertToHtml(text);
      //save with html extension.
      string path = Path.GetDirectoryName(args[0])
         + Path.DirectorySeparatorChar
         + Path.GetFileNameWithoutExtension(args[0]);
      string pathEmbeded = path + ".Embeded.html";
      string pathConverted = path + ".Converted.html";// Path.GetExtension(args[0]);

      //=========================================
      // The code supports 2 different converters.
      // Apparently the git of ConvertWithCSharpToColouredHtml is more active and has more features.
      //=========================================
      //ConvertWithHtmlSyntaxHighlighter(pathEmbeded, pathConverted, text);
      ConvertWithCSharpToColouredHtml(pathEmbeded, pathConverted, text);
    }

    private static void ConvertWithCSharpToColouredHtml(string pathEmbeded, string pathConverted, string code)
    {
      //var myCustomCSS = "<style>...</style>";
      //var settings = new HTMLEmitterSettings().UseCustomCSS(myCustomCSS);
      var settings = new HTMLEmitterSettings().DisableIframe();//.DisableLineNumbers();
      string convertedHtml = new CsharpColourer().ProcessSourceCode(code, new HTMLEmitter(settings));
      File.WriteAllText(pathConverted, convertedHtml);
      int i = convertedHtml.IndexOf("<pre class=\"background\"")+24;
      string embeddedHtml= "<div class=\"code, swiftly\"><pre><code>" + convertedHtml.Substring(i, convertedHtml.Length - i);
      i = embeddedHtml.IndexOf("</pre>");
      embeddedHtml = embeddedHtml.Substring(0, i) + "</code></pre></div>";
      File.WriteAllText(pathEmbeded, embeddedHtml);
    }

    static void ConvertWithHtmlSyntaxHighlighter(string pathConverted, string pathEmbeded, string text)
    {
      //convert to html
      var syntaxHtml = HtmlSyntaxHighlighter.TransformCSharp(text);
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
      //<pre is needed since spaces are used as is for formatting.
      var embeddedHtml =
$@"  <pre>
    <code>{syntaxHtml}</code>  
  </pre>";

      File.WriteAllText(pathConverted, fullHtml);
      File.WriteAllText(pathEmbeded, embeddedHtml);

    }
  }
}