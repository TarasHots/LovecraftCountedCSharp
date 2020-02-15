using System;
using HtmlAgilityPack;
using System.Net;
using System.Linq;

namespace Scrapper
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("Need arguments!");
            }

            var pageReader = new HtmlWeb();
            var page = pageReader.Load(args[0]);

            var titleNode = page.DocumentNode.SelectSingleNode("//head/title");
            Console.WriteLine("Reading " + titleNode.InnerText);

            var textElement = page.DocumentNode.SelectSingleNode("//body/tr/td/font/div/div");

            var dividedText = WebUtility.HtmlDecode(textElement.InnerText)
                .Replace(",", "")
                .Replace(".", "")
                .Replace(":", "")
                .Replace("\r\n", "")
                .Replace("\r", "")
                .Replace("\"", "")
                .Split(' ');

            var matchQuery = from word in dividedText
                             group word by word.ToLower() into g
                             orderby g.Count() descending
                             select new { Word = g.Key, Count = g.Count() };

            foreach (var nameGroup in matchQuery)
            {
                Console.WriteLine($"{nameGroup.Word} : {nameGroup.Count}");
            }
        }
    }
}