using CarpathianMadness.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarpathianMadness.Models
{
    public class TerritoryWarModel
    {
        public string EventName { get; set; }
        public string EventGalacticPower { get; set; }
        public string EventMembersCount { get; set; }

        public List<string> EventUnits { get; set; }

        public string EventZetas { get; set; }



        // used

        public string OurGuildName { get; set; }
        public string EnemyGuildName { get; set; }
        public string OurGalacticPower { get; set; }
        public string EnemyGalactivPower { get; set; }
        public long OurMembersCount { get; set; }
        public long EnemyMembersCount { get; set; }
        public string OverviewStyle { get; set; }

        public List<Unit> OurUnitsList { get; set; }
        public List<Unit> EnemyUnitsList { get; set; }

        public List<UnitsInfo> EventUnitsInfo { get; set; }

        public int[] RelicCounter { get; set; }
        public int[] EnemyRelicCounter { get; set; }
        public int[] ZetasCount { get; set; }        

    }

    public class UnitsInfo
    {
        public string Name { get; set; }
        public string ColumnStyle { get; set; }
        public int[] AvailableUnits { get; set; }
        public int[] SevenStarUnitCount { get; set; }
        public int[] SixStarUnitCount { get; set; }
        public int[] FiveStarUnitCount { get; set; }
        public int[] G11UnitCount { get; set; }
        public int[] G12UnitCount { get; set; }
        public int[] G13UnitCount { get; set; }
        public int[] RelicTier3UnitCount { get; set; }
        public int[] RelicTier4UnitCount { get; set; }
        public int[] RelicTier5UnitCount { get; set; }
        public int[] RelicTier6UnitCount { get; set; }
        public int[] RelicTier7UnitCount { get; set; }
        public int[] OneZetaUnitCount { get; set; }
        public int[] DoubleZetaUnitCount { get; set; }
        public int[] TripleZetaUnitCount { get; set; }
        public int[] QuadrupleZetaUnitCount { get; set; }

    }
}
