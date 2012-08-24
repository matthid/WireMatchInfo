using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace UserData
{
    class MatchMember
    {
        private Regex regex;
        private HtmlNode playerNodes, playerLinkNode;
        private const string regexIdPattern = @"(?<=player/).*(?=/)";
        // private const string regexNamePattern = "(?<=Nick</td><td>).*(?=</td></tr><tr><td class=\"firstcol\">Member)";
        private HtmlWeb htmlWeb;
        private List<Player> players;

        public MatchMember()
        {
            htmlWeb = new HtmlWeb();
            regex = new Regex(regexIdPattern);
        }

        internal class Player
        {
            private readonly int id;
            private readonly string eslPlayerLink;
            private readonly string playerNick;

            public Player(int id, string eslPlayerLink, string playerNick)
            {
                this.id = id;
                this.eslPlayerLink = eslPlayerLink;
                this.playerNick = playerNick;
            }

            public int Id
            {
                get { return id; }
            }

            public string EslPlayerLink
            {
                get { return eslPlayerLink; }
            }

            public string PlayerNick
            {
                get { return playerNick; }
            }


        } // end internal Player Class

        public List<Player> GetMatchMembers(string matchlink)
        {
            HtmlAgilityPack.HtmlDocument matchDocument = htmlWeb.Load(matchlink);
            HtmlAgilityPack.HtmlDocument playerDocument;
       
            playerNodes = matchDocument.GetElementbyId("votes_table");
            players = new List<Player>();

            // If there is no node with that Id, someNode will be null
            if (playerNodes != null)
            {
                // Extracts all links within that node
                IEnumerable<HtmlNode> allLinks = playerNodes.Descendants("a"); // alle links der player sammeln
                // Outputs the href for external links
                foreach (HtmlNode link in allLinks)
                {
                    
                    // Get Playerlink
                    string playerLink = "http://www.esl.eu" + link.Attributes["href"].Value;
                    
                    int playerId;

                    //if the playerId within the playerLink isn't an 'int', load Player document
                    if (!int.TryParse(Convert.ToString(regex.Match(playerLink)), out playerId))
                    {
                        playerDocument = htmlWeb.Load(playerLink);
                        playerNodes = playerDocument.GetElementbyId("player_logo");
                        playerLinkNode = playerNodes.SelectSingleNode("a"); // XPath-Syntax
                        playerLink = playerLinkNode.Attributes["href"].Value;
                        playerId = int.Parse(regex.Match(playerLink).ToString());
                    }
                    
                    //Get PlayerNick
                    string playerNick1 = link.InnerText.Trim();
       
                    //Add Player to Playerlist
                    players.Add(new Player(playerId, playerLink, playerNick1));
                } // end foreach
            }//end if
            return players;
        } // end method
    } // end MachMember Class
}
