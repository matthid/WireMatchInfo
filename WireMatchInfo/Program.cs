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

            Console.WriteLine("Downloading Matchinfo. This will take some seconds !!\n");
            MatchMember mm = new MatchMember();

            List<MatchMember.Player> pl = mm.GetMatchMembers("http://www.esl.eu/de/css/ui/versus/match/2758197");

            Console.WriteLine();

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
