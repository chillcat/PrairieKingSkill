using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrairieKingSkill
{
    class PrairieKingProfession : Profession
    {
        //To avoid id colision for professions
        private const int ProfessionBaseId = 41;

        public const int LivesProfessionId = ProfessionBaseId + 1;
        public const int ShootingDelayProfessionId = ProfessionBaseId + 2;
        public const int PowerUpDurationProfessionId = ProfessionBaseId + 3;
        public const int CoinsProfessionId = ProfessionBaseId + 4;
        public const int CoinChanceProfessionId = ProfessionBaseId + 5;
        public const int LootChanceProfessionId = ProfessionBaseId + 6;

        public static readonly PrairieKingProfession Lives = new PrairieKingProfession(LivesProfessionId, "LevelUpValue_Lives", "More lives");
        public static readonly PrairieKingProfession ShootingDelay = new PrairieKingProfession(ShootingDelayProfessionId, "LevelUpValue_ShootingDelay", "Shorter shooting delay");
        public static readonly PrairieKingProfession PowerUpDuration = new PrairieKingProfession(PowerUpDurationProfessionId, "LevelUpValue_PowerUpDuration", "Longer power up duration");
        public static readonly PrairieKingProfession Coins = new PrairieKingProfession(CoinsProfessionId, "LevelUpValue_Coins", "More coins");
        public static readonly PrairieKingProfession CoinChance = new PrairieKingProfession(CoinChanceProfessionId, "LevelUpValue_CoinChance", "Higher chance for enemies to drop coins.");
        public static readonly PrairieKingProfession LootChance = new PrairieKingProfession(LootChanceProfessionId, "LevelUpValue_LootChance", "Higher chance for enemies to drop loot.");

        private readonly string name;
        public string Name { get { return name; } }
        private readonly int id;
        public int Id { get { return id; } }
        private readonly string description;
        public string Description { get { return description; } }

        private PrairieKingProfession(int id, string name, string description)
        {
            this.id = id + ProfessionBaseId;
            this.name = name;
            this.description = description;
        }

        public static IEnumerable<PrairieKingProfession> Values
        {
            get
            {
                yield return Lives;
                yield return ShootingDelay;
                yield return PowerUpDuration;
                yield return Coins;
                yield return CoinChance;
                yield return LootChance;
            }
        }
    }
}
