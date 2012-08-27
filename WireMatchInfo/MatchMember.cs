using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace UserData
{
    class MatchMember
    {
        private readonly Regex regex;
        private HtmlNode playerNodes;
        private const string regexIdPattern = @"(?<=player/).*(?=/)";
        // private const string regexNamePattern = "(?<=Nick</td><td>).*(?=</td></tr><tr><td class=\"firstcol\">Member)";
        private readonly HtmlWeb htmlWeb;
        private List<Player> players;
        private HtmlDocument matchDocument;

        public enum KindOfMatch
        {
            Versus,
            Regular
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////   MatchMember() (constructor)   ////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////
        
        public MatchMember()
        {
            htmlWeb = new HtmlWeb();
            regex = new Regex(regexIdPattern);
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////   Player Class   ///////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////
         
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

        ////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////   CheckWebserverAvailability(string link)   ////////////////
        ////////////////////////////////////////////////////////////////////////////////////////
        
        public bool CheckWebserverAvailability(string link)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(link);
            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)httpRequest.GetResponse(); 
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }

            if (response.StatusCode == HttpStatusCode.OK) // http code 200
                return true;

            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////   GetRegularMatchTeamLinks(string link)   //////////////////
        ////////////////////////////////////////////////////////////////////////////////////////
        
        public string[] GetRegularMatchTeamLinks(string link)
        {
            string[] teamLinks = new string[2];
            HtmlAgilityPack.HtmlDocument matchDocument = htmlWeb.Load(link);
            HtmlNode teamLinkNodes;
            teamLinkNodes = matchDocument.GetElementbyId("main_content");
            HtmlNode linkTeam1 = teamLinkNodes.SelectSingleNode("./table/tr[3]/td[1]/table/tr[2]/td[1]/a[1]");
            HtmlNode linkTeam2 = teamLinkNodes.SelectSingleNode("./table/tr[3]/td[1]/table/tr[2]/td[3]/a[1]");

            string eslLinkTeam1 = "http://esl.eu" + linkTeam1.Attributes["href"].Value;
            teamLinks[0] = eslLinkTeam1;
            string eslLinkTeam2 = "http://esl.eu" + linkTeam2.Attributes["href"].Value;
            teamLinks[1] = eslLinkTeam2;

            return teamLinks;
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////   GetRegularMatchMemberNodes(string link)   ////////////////
        ////////////////////////////////////////////////////////////////////////////////////////
        
        public IEnumerable<HtmlNode> GetRegularMatchMemberNodes(string matchlink)
        {
            matchDocument = htmlWeb.Load(matchlink);
            playerNodes = matchDocument.GetElementbyId("main_content");
            IEnumerable<HtmlNode> allLinks = playerNodes.SelectNodes("./div[4]/div[5]/table/tr/td[2]/div").Descendants("a");

            return allLinks;
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////   GetMatchmemberInfo(string memberLink)   /////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        public Player GetMatchMemberInfo(HtmlNode memberNode)
        {
            // Get playerLink
            string playerLink = "http://www.esl.eu" + memberNode.Attributes["href"].Value;

            if (playerLink.Contains("playercard")) return null;

            int playerId;

            //if the playerId within the playerLink isn't an 'int', load Player document
            if (!int.TryParse(Convert.ToString(regex.Match(playerLink)), out playerId))
            {
                HtmlDocument playerDocument = htmlWeb.Load(playerLink);
                playerNodes = playerDocument.GetElementbyId("player_logo");
                HtmlNode playerLinkNode = playerNodes.SelectSingleNode("a");
                playerLink = playerLinkNode.Attributes["href"].Value;
                playerId = int.Parse(regex.Match(playerLink).ToString());
            }
            
            //Get PlayerNick
            string playerNick1 = memberNode.InnerText.Trim();

            // return Player
            return new Player(playerId, playerLink, playerNick1);
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////   GetVersusMatchMemberNodes(string matchlink)   ///////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        public IEnumerable<HtmlNode> GetVersusMatchMemberNodes(string matchlink)
        {
            matchDocument = htmlWeb.Load(matchlink);
            playerNodes = matchDocument.GetElementbyId("votes_table");
            IEnumerable<HtmlNode> allLinks = playerNodes.Descendants("a");

            return allLinks;
        }
        ///////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////   GetMatchMembers(string matchlink, KindOfMatch kind)   //////
        ///////////////////////////////////////////////////////////////////////////////////////

        public List<Player> GetMatchMembers(string matchlink, KindOfMatch kind)
        {
            players = new List<Player>();


            if (kind == KindOfMatch.Versus)
            {
                IEnumerable<HtmlNode> memberNodes = GetVersusMatchMemberNodes(matchlink);

                foreach (var memberNode in memberNodes)
                {
                    if (GetMatchMemberInfo(memberNode) != null)
                    players.Add(GetMatchMemberInfo(memberNode));
                }
            }
            else // Regular Match
            {
                string[] teamLinks = GetRegularMatchTeamLinks(matchlink);
                IEnumerable<HtmlNode> memberNodesTeam1 = GetRegularMatchMemberNodes(teamLinks[0]);

                foreach (var memberNode in memberNodesTeam1)
                {
                    if (GetMatchMemberInfo(memberNode) != null) //if a playercardlink is detected, GetMatchMemverInfor() returns null
                    players.Add(GetMatchMemberInfo(memberNode));
                }

                IEnumerable<HtmlNode> memberNodesTeam2 = GetRegularMatchMemberNodes(teamLinks[1]);

                foreach (var memberNode in memberNodesTeam2)
                {
                    if (GetMatchMemberInfo(memberNode) != null)
                    players.Add(GetMatchMemberInfo(memberNode));
                }
            }

            return players;
        }
    } // end MatchMember Class
}
