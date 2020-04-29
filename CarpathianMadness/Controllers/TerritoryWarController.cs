using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarpathianMadness.Models;
using CarpathianMadness.Services;
using CarpathianMadness.Framework;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Specialized;

namespace CarpathianMadness.Controllers
{
    public class TerritoryWarController : Controller
    {
        private readonly ILogger<TerritoryWarController> _logger;        
        private readonly IHomeService _homeService;
        private List<UnitsInfo> ls = new List<UnitsInfo>();
        public TerritoryWarController(ILogger<TerritoryWarController> logger)
        {
            _logger = logger;
            this._homeService = new HomeService();
            
        }

        public IActionResult Index()
        {
            // not finished 
            TerritoryWarModel tw = new TerritoryWarModel();

          //  TriggerSync("43789");



            string json = string.Empty;

            Swgoh[] guilds = new Swgoh[2];


            using (StreamReader r = new StreamReader(@"C:\Users\claudio.admin\Desktop\swgoh.json"))
            {
                json = r.ReadToEnd();
            }
            guilds[0] = Swgoh.FromJson(json);
            using (StreamReader r = new StreamReader(@"C:\Users\claudio.admin\Desktop\swgohenemy.json"))
            {
                json = r.ReadToEnd();
            }
            guilds[1] = Swgoh.FromJson(json);

            tw.EventName = guilds[0].Data.Name + " - " + guilds[1].Data.Name;
            tw.EventGalacticPower = String.Format("{0:n0}", guilds[0].Data.GalacticPower).ToString() + " - " + String.Format("{0:n0}", guilds[1].Data.GalacticPower).ToString();
            tw.EventMembersCount = guilds[0].Data.MemberCount + " - " + guilds[1].Data.MemberCount;

            tw.EventUnits = new List<string>();

            ///////////////
            tw.OurGuildName = guilds[0].Data.Name;
            tw.EnemyGuildName = guilds[1].Data.Name;

            tw.OurGalacticPower = String.Format("{0:n0}", guilds[0].Data.GalacticPower);
            tw.EnemyGalactivPower = String.Format("{0:n0}", guilds[1].Data.GalacticPower);

            tw.OurMembersCount = guilds[0].Data.MemberCount;
            tw.EnemyMembersCount = guilds[1].Data.MemberCount;

            tw.OverviewStyle = "border-info";

            if(guilds[1].Data.GalacticPower - guilds[0].Data.GalacticPower > 5000000)
            {
                tw.OverviewStyle = "border-danger";
            }
            else if(guilds[1].Data.GalacticPower - guilds[0].Data.GalacticPower > 0)
            {
                tw.OverviewStyle = "border-warning";
            }
            else if(guilds[1].Data.GalacticPower - guilds[0].Data.GalacticPower < 5000000)
            {
                tw.OverviewStyle = "border-success";
            }

            ///////////////
            



            string[] units = new string[22];
            units[0] = "General Skywalker";
            units[1] = "Darth Malak";
            units[2] = "Darth Revan";
            units[3] = "Jedi Knight Revan";
            units[4] = "Enfys Nest";
            units[5] = "Padmé Amidala";
            units[6] = "General Grievous";
            units[7] = "Negotiator";
            units[8] = "Malevolence";
            units[9] = "Geonosian Brood Alpha";
            units[10] = "Darth Traya";
            units[11] = "Wat Tambor";
            units[12] = "Hound's Tooth";
            units[13] = "Jedi Knight Anakin";
            units[14] = "Han's Millennium Falcon";
            units[15] = "Rey";
            units[16] = "Supreme Leader Kylo Ren";
            units[17] = "Maniac";
            units[18] = "ARC Trooper";
            units[19] = "Kylo Ren (Unmasked)";
            units[20] = "Bossk";
            units[21] = "Nute Gunray";

            List<Unit> unitsList = new List<Unit>();
            List<Unit> enemyUnits = new List<Unit>();

            List<Unit> unitsWithZetas = new List<Unit>();
            List<Unit> enemyUnitsWithZetas = new List<Unit>();

            

            foreach (string c in units)
            {
                UnitsInfo ui = new UnitsInfo();
                tw.EventUnits.Add(c);
                ui.Name = c;
                ls.Add(ui);

            }

            foreach (Player player in guilds[0].Players)
            {
                foreach (Unit playerData in player.Units)
                {
                    if (Array.Exists(units, element => element == playerData.Data.Name))
                    {
                        unitsList.Add(playerData);
                    }
                }
            }

            foreach (Player player in guilds[1].Players)
            {
                foreach (Unit playerData in player.Units)
                {

                    if (Array.Exists(units, element => element == playerData.Data.Name))
                    {
                        enemyUnits.Add(playerData);
                    }
                }
            }

            tw.OurUnitsList = unitsList;
            tw.EnemyUnitsList = enemyUnits;         



            InitiateArrays();
            GetStars(unitsList, 0);
            GetStars(enemyUnits, 1);
            GetGear(unitsList, 0);
            GetGear(enemyUnits, 1);
            GetRelic(unitsList, 0);
            GetRelic(enemyUnits, 1);
            GetZetas(unitsList, 0);
            GetZetas(enemyUnits, 1);
            GetAvailableUnits(unitsList, 0);
            GetAvailableUnits(enemyUnits, 1);

            tw.EventUnitsInfo = ls;
            tw.RelicCounter = GetRelicCounter(guilds[0]);
            tw.EnemyRelicCounter = GetRelicCounter(guilds[1]);
            tw.ZetasCount = new int[2];
            tw.ZetasCount[0] = GetZetasCount(guilds[0]);
            tw.ZetasCount[1] = GetZetasCount(guilds[1]);
            // _homeService.SearchHome(8);

            return View(tw);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        } 

        private void GetStars(List<Unit> units, int guildNumber)
        {
            foreach(UnitsInfo ui in ls)
            { 
                // 7 stars
                ui.SevenStarUnitCount[guildNumber] = units.Where(x => x.Data.Rarity == 7)
                    .Where(x => x.Data.Name == ui.Name).Count();
                // 6 stars
                ui.SixStarUnitCount[guildNumber] = units.Where(x => x.Data.Rarity == 6)
                    .Where(x => x.Data.Name == ui.Name).Count();
                // 5 stars
                ui.FiveStarUnitCount[guildNumber] = units.Where(x => x.Data.Rarity == 5)
                    .Where(x => x.Data.Name == ui.Name).Count();

            }
        }

        private void GetGear(List<Unit> units, int guildNumber)
        {
            foreach (UnitsInfo ui in ls)
            {
                ui.G11UnitCount[guildNumber] = units.Where(x => x.Data.GearLevel == 11)
                            .Where(x => x.Data.Name == ui.Name).Count();
                ui.G12UnitCount[guildNumber] = units.Where(x => x.Data.GearLevel == 12)
                        .Where(x => x.Data.Name == ui.Name).Count();
                ui.G13UnitCount[guildNumber] = units.Where(x => x.Data.GearLevel == 13)
                        .Where(x => x.Data.Name == ui.Name).Count();

            }
        }

        private void GetRelic(List<Unit> units, int guildNumber)
        {
            foreach (UnitsInfo ui in ls)
            {
                ui.RelicTier3UnitCount[guildNumber] = units.Where(x => x.Data.RelicTier == 5)
                            .Where(x => x.Data.Name == ui.Name).Count();
                ui.RelicTier4UnitCount[guildNumber] = units.Where(x => x.Data.RelicTier == 6)
                        .Where(x => x.Data.Name == ui.Name).Count();
                ui.RelicTier5UnitCount[guildNumber] = units.Where(x => x.Data.RelicTier == 7)
                        .Where(x => x.Data.Name == ui.Name).Count();
                ui.RelicTier6UnitCount[guildNumber] = units.Where(x => x.Data.RelicTier == 8)
                        .Where(x => x.Data.Name == ui.Name).Count();
                ui.RelicTier7UnitCount[guildNumber] = units.Where(x => x.Data.RelicTier == 9)
                        .Where(x => x.Data.Name == ui.Name).Count();

            }
        }
        private void GetZetas(List<Unit> units, int guildNumber)
        {
            foreach (UnitsInfo ui in ls)
            {
                ui.OneZetaUnitCount[guildNumber] = units.Where(x => x.Data.ZetaAbilities.Count() == 1)
                            .Where(x => x.Data.Name == ui.Name).Count();
                ui.DoubleZetaUnitCount[guildNumber] = units.Where(x => x.Data.ZetaAbilities.Count() == 2)
                        .Where(x => x.Data.Name == ui.Name).Count();
                ui.TripleZetaUnitCount[guildNumber] = units.Where(x => x.Data.ZetaAbilities.Count() == 3)
                        .Where(x => x.Data.Name == ui.Name).Count();
                ui.QuadrupleZetaUnitCount[guildNumber] = units.Where(x => x.Data.ZetaAbilities.Count() == 4)
                        .Where(x => x.Data.Name == ui.Name).Count();

            }
        }

        private void GetAvailableUnits(List<Unit> units, int guildNumber)
        {
            foreach (UnitsInfo ui in ls)
            {
                ui.AvailableUnits[guildNumber] = units.Where(x => x.Data.Name == ui.Name).Count();
            }
        }

        private int[] GetRelicCounter(Swgoh guild)
        {
            int[] relics = new int[7];

            foreach (Player player in guild.Players)
            {
                foreach (Unit playerData in player.Units)
                {
                    switch (playerData.Data.RelicTier)
                    {
                        case 9:
                            relics[6] += 1;
                            break;
                        case 8:
                            relics[5] += 1;
                            break;
                        case 7:
                            relics[4] += 1;
                            break;
                        case 6:
                            relics[3] += 1;
                            break;
                        case 5:
                            relics[2] += 1;
                            break;
                        case 4:
                            relics[1] += 1;
                            break;
                        case 3:
                            relics[0] += 1;
                            break;
                        
                        default:
                            break;
                    } 
                }
            }

            return relics;
        }

        private int GetZetasCount(Swgoh guild)
        {
            int counter = 0;

            foreach (Player player in guild.Players)
            {
                foreach (Unit playerData in player.Units)
                {
                    counter += playerData.Data.ZetaAbilities.Count();
                }
            }

            return counter;
        }


        private void InitiateArrays()
        {
            foreach (UnitsInfo ui in ls)
            {
                //stars
                ui.SevenStarUnitCount = new int[2];
                ui.SixStarUnitCount = new int[2];
                ui.FiveStarUnitCount = new int[2];
                //gear
                ui.G11UnitCount = new int[2];
                ui.G12UnitCount = new int[2];
                ui.G13UnitCount = new int[2];
                //relic
                ui.RelicTier3UnitCount = new int[2];
                ui.RelicTier4UnitCount = new int[2];
                ui.RelicTier5UnitCount = new int[2];
                ui.RelicTier6UnitCount = new int[2];
                ui.RelicTier7UnitCount = new int[2];
                //zetas
                ui.OneZetaUnitCount = new int[2];
                ui.DoubleZetaUnitCount = new int[2];
                ui.TripleZetaUnitCount = new int[2];
                ui.QuadrupleZetaUnitCount = new int[2];
                //available units
                ui.AvailableUnits = new int[2];                
            } 
        }

        private void TriggerSync(string guildID)
        {
            // build the sync trygger and add the header 
            var request = (HttpWebRequest)WebRequest.Create("http://swgoh.gg/api/guilds/{guild_id}/trigger-sync/");

            var postData = "guild_id=" + Uri.EscapeDataString(guildID);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


        }

        private Swgoh GetGuildData(string guildID)
        {
            Swgoh temp = new Swgoh();

            using (var wb = new WebClient())
            {
                var response = wb.DownloadString(@"http://swgoh.gg/api/guild/" + guildID + "/");
                temp = Swgoh.FromJson(response);
            }

            return temp;
        }


    }
}
