﻿using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CarpathianMadness.Framework
{
        public partial class Swgoh
        {
            [JsonProperty("players")]
            public Player[] Players { get; set; }

            [JsonProperty("data")]
            public SwgohData Data { get; set; }
        }

        public partial class SwgohData
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("member_count")]
            public long MemberCount { get; set; }

            [JsonProperty("galactic_power")]
            public long GalacticPower { get; set; }

            [JsonProperty("rank")]
            public long Rank { get; set; }

            [JsonProperty("profile_count")]
            public long ProfileCount { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }
        }

        public partial class Player
        {
            [JsonProperty("units")]
            public Unit[] Units { get; set; }

            [JsonProperty("data")]
            public PlayerData Data { get; set; }
        }

        public partial class PlayerData
        {
            [JsonProperty("arena")]
            public object Arena { get; set; }

            [JsonProperty("arena_rank")]
            public object ArenaRank { get; set; }

            [JsonProperty("arena_leader_base_id")]
            public object ArenaLeaderBaseId { get; set; }

            [JsonProperty("last_updated")]
            public DateTimeOffset LastUpdated { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("galactic_war_won")]
            public long GalacticWarWon { get; set; }

            [JsonProperty("ally_code")]
            public long AllyCode { get; set; }

            [JsonProperty("galactic_power")]
            public long GalacticPower { get; set; }

            [JsonProperty("level")]
            public long Level { get; set; }

            [JsonProperty("pve_hard_won")]
            public long PveHardWon { get; set; }

            [JsonProperty("pve_battles_won")]
            public long PveBattlesWon { get; set; }

            [JsonProperty("character_galactic_power")]
            public long CharacterGalacticPower { get; set; }

            [JsonProperty("fleet_arena")]
            public object FleetArena { get; set; }

            [JsonProperty("ship_galactic_power")]
            public long ShipGalacticPower { get; set; }

            [JsonProperty("pvp_battles_won")]
            public long PvpBattlesWon { get; set; }

            [JsonProperty("guild_exchange_donations")]
            public long GuildExchangeDonations { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("guild_contribution")]
            public long GuildContribution { get; set; }

            [JsonProperty("ship_battles_won")]
            public long ShipBattlesWon { get; set; }

            [JsonProperty("guild_raid_won")]
            public long GuildRaidWon { get; set; }
        }

        public partial class Unit
        {
            [JsonProperty("data")]
            public UnitData Data { get; set; }
        }

        public partial class UnitData
        {
            [JsonProperty("relic_tier")]
            public long RelicTier { get; set; }

            [JsonProperty("gear_level")]
            public long GearLevel { get; set; }

            [JsonProperty("gear")]
            public Gear[] Gear { get; set; }

            [JsonProperty("power")]
            public long Power { get; set; }

            [JsonProperty("level")]
            public long Level { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("combat_type")]
            public long CombatType { get; set; }

            [JsonProperty("mod_set_ids")]
            [JsonConverter(typeof(DecodeArrayConverter))]
            public long[] ModSetIds { get; set; }

            [JsonProperty("rarity")]
            public long Rarity { get; set; }

            [JsonProperty("base_id")]
            public string BaseId { get; set; }

            [JsonProperty("stats")]
            public Dictionary<string, double> Stats { get; set; }

            [JsonProperty("zeta_abilities")]
            public string[] ZetaAbilities { get; set; }

            [JsonProperty("ability_data")]
            public AbilityDatum[] AbilityData { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public partial class AbilityDatum
        {
            [JsonProperty("is_omega")]
            public bool IsOmega { get; set; }

            [JsonProperty("is_zeta")]
            public bool IsZeta { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("ability_tier")]
            public long AbilityTier { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("tier_max")]
            public long TierMax { get; set; }
        }

        public partial class Gear
        {
            [JsonProperty("slot")]
            public long Slot { get; set; }

            [JsonProperty("is_obtained")]
            public bool IsObtained { get; set; }

            [JsonProperty("base_id")]
            public string BaseId { get; set; }
        }

        public partial class Swgoh
        {
            public static Swgoh FromJson(string json) => JsonConvert.DeserializeObject<Swgoh>(json, Converter.Settings);
        }

        public static class Serialize
        {
            public static string ToJson(this Swgoh self) => JsonConvert.SerializeObject(self, Converter.Settings);
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class DecodeArrayConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(long[]);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                reader.Read();
                var value = new List<long>();
                while (reader.TokenType != JsonToken.EndArray)
                {
                    var converter = ParseStringConverter.Singleton;
                    var arrayItem = (long)converter.ReadJson(reader, typeof(long), null, serializer);
                    value.Add(arrayItem);
                    reader.Read();
                }
                return value.ToArray();
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                var value = (long[])untypedValue;
                writer.WriteStartArray();
                foreach (var arrayItem in value)
                {
                    var converter = ParseStringConverter.Singleton;
                    converter.WriteJson(writer, arrayItem, serializer);
                }
                writer.WriteEndArray();
                return;
            }

            public static readonly DecodeArrayConverter Singleton = new DecodeArrayConverter();
        }

        internal class ParseStringConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                long l;
                if (Int64.TryParse(value, out l))
                {
                    return l;
                }
                throw new Exception("Cannot unmarshal type long");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (long)untypedValue;
                serializer.Serialize(writer, value.ToString());
                return;
            }

            public static readonly ParseStringConverter Singleton = new ParseStringConverter();
        }
    }

