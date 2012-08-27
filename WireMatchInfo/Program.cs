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


            Console.WriteLine();

            Console.WriteLine("Get Playerinfo ... this will take some seconds\n");
            var l = new List<string>()
                {
                    "http://www.esl.eu/de/css/ui/versus/match/2778059",
                    //"http://www.esl.eu/de/css/ui/versus/match/2777955",
                    //"http://www.esl.eu/de/ui/versus/match/2758882",
                    //"http://www.esl.eu/de/css/ui/versus/match/2758197",
                    //"http://www.esl.eu/de/css/ui/versus/match/2754367"
                };
            var s = System.Diagnostics.Stopwatch.StartNew();
            MatchMember mm = new MatchMember();
            foreach (var link in l)
            {
                mm.GetMatchMembers(link); //("http://www.esl.eu/de/css/ui/versus/match/2758197");
            }
            s.Stop();

            Console.WriteLine("ElapsedTime: {0}ms\n", s.ElapsedMilliseconds);

            Console.ReadKey();
        }
    }

}
