using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace UserData
{
    class Program
    {
        static void Main(string[] args)
        {

            //HtmlWeb htmlWeb = new HtmlWeb();
            //HtmlAgilityPack.HtmlDocument document = htmlWeb.Load("http://www.esl.eu/de/css/ui/versus/match/2758197");

            //string paternId = @"(?<=player/).*(?=/)";
            //string paternName = "(?<=Nick</td><td>).*(?=</td></tr><tr><td class=\"firstcol\">Member)";

            //Regex regex;
            //HtmlNode playerNodes, playerLinkNode;
            //Console.WriteLine("\nPlayer links and ID's\n");

            //playerNodes = document.GetElementbyId("votes_table");

            //// If there is no node with that Id, someNode will be null
            //if (playerNodes != null)
            //{
            //    // Extracts all links within that node
            //    IEnumerable<HtmlNode> allLinks = playerNodes.Descendants("a"); // alle links der player sammeln

            //    // Outputs the href for external links
            //    foreach (HtmlNode link in allLinks) // die linksdurchgehen und die id parsen
            //    {
            //        string playerLink = "http://www.esl.eu" + link.Attributes["href"].Value;

            //        Console.WriteLine(playerLink);

            //        document = htmlWeb.Load(playerLink);
            //        playerNodes = document.GetElementbyId("player_logo");
            //        playerLinkNode = playerNodes.SelectSingleNode("a"); // siehe XPath-Syntax
            //        string playerLinkWithId = playerLinkNode.Attributes["href"].Value;
            //        regex = new Regex(paternId);

            //        string playerId = regex.Match(playerLinkWithId).ToString();

            //        Console.WriteLine(playerId);

                    
            //        HtmlDocument myDoc = htmlWeb.Load(playerLink);
            //        string html = myDoc.DocumentNode.OuterHtml;

            //        regex = new Regex(paternName);
            //        html = html.Replace("\n", "");
            //        string uname = regex.Match(html).ToString();

            //        Console.WriteLine(uname + "\n");

            //        //StreamWriter myWriter = File.CreateText(@"c:\text.txt");
            //        //myWriter.WriteLine(html);
            //        //myWriter.Close();
            //    }


            //    Console.ReadKey();

           // }

            Console.WriteLine("Get Playerinfo ... this will take some seconds\n");
            MatchMember mm = new MatchMember();
            var pl = mm.GetMatchMembers("http://www.esl.eu/de/css/ui/versus/match/2758197");
            
            foreach (var player in pl)
            {
                Console.WriteLine(player.EslPlayerLink);
                Console.WriteLine(player.Id);
                Console.WriteLine(player.PlayerNick);
                Console.WriteLine();
            }

            Console.ReadKey();

        }
    }

}
