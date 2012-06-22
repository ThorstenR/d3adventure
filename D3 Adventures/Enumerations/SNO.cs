using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3_Adventures.Enumerations
{
    public class SNO
    {
        public enum SNOPowerId
        {
            a1dun_Leor_Fire_Gutter_fire = 0x2ac37,
            a2dun_Cave_Goatmen_Dropping_Log_Trap_attack = 0x2abdd,
            ActorDisabledBuff = 0x16e14,
            ActorGhostedBuff = 0x36d7f,
            ActorInTownBuff = 0x35c90,
            ActorLoadingBuff = 0x33c40,
            AI_Circle = 0x7522,
            AI_Circle_Long = 0x7523,
            AI_Close = 0x7526,
            AI_Close_Long = 0x7527,
            AI_Follow = 0x7529,
            AI_Follow_Close = 0x752b,
            AI_Follow_MeleeLead = 0x19842,
            AI_Follow_MeleeLead_Pet = 0x3865c,
            AI_FollowPath = 0x752a,
            AI_FollowWithWalk = 0x6c0,
            AI_Idle = 0x752c,
            AI_Idle_Long = 0x752d,
            AI_Idle_Short = 0x752e,
            AI_Orbit = 0xd889,
            AI_ReturnToGuardObject = 0x2f383,
            AI_ReturnToPath = 0x7530,
            AI_RunAway = 0x7531,
            AI_RunAway_Long = 0x7532,
            AI_RunAway_Short = 0x7533,
            AI_RunInFront = 0x7534,
            AI_RunInFront_Guaranteed = 0x27e0b,
            AI_RunNearby = 0x7535,
            AI_RunNearby_Short = 0x7538,
            AI_RunTo = 0x7539,
            AI_RunTo_Guaranteed = 0x27e0a,
            AI_SprintInFront_Guaranteed = 0x27e08,
            AI_SprintTo_Guaranteed = 0x27e07,
            AI_TownWalkTo_Guaranteed = 0x35212,
            AI_WalkInFront_Guaranteed = 0x27e06,
            AI_WalkTo = 0x753d,
            AI_WalkTo_Guaranteed = 0x27e05,
            AI_Wander = 0x6c1,
            AI_Wander_Long = 0x753f,
            AI_WanderRun = 0x753e,
            AIEvadeBuff = 0x184d7,
            AncientSpearKnockback = 0x19f29,
            Axe_Bad_Data = 0x7544,
            Axe_Operate_Gizmo = 0x7545,
            Axe_Operate_NPC = 0x7546,
            BannerDrop = 0x2d2d0,
            Banter_Cooldown = 0x20cbe,
            Barbarian_AncientSpear = 0x1115b,
            Barbarian_Bash = 0x1358a,
            Barbarian_BattleRage = 0x134e4,
            Barbarian_CallOfTheAncients = 0x138b1,
            Barbarian_CallOfTheAncients_Basic_Melee = 0x2dad4,
            Barbarian_CallOfTheAncients_Cleave = 0x29377,
            Barbarian_CallOfTheAncients_FuriousCharge = 0x29378,
            Barbarian_CallOfTheAncients_Leap = 0x29379,
            Barbarian_CallOfTheAncients_SeismicSlam = 0x2937b,
            Barbarian_CallOfTheAncients_WeaponThrow = 0x2937c,
            Barbarian_CallOfTheAncients_Whirlwind = 0x2937e,
            Barbarian_Cleave = 0x13987,
            Barbarian_Earthquake = 0x1823e,
            Barbarian_Frenzy = 0x132d4,
            Barbarian_FuriousCharge = 0x17c9b,
            Barbarian_GroundStomp = 0x13656,
            Barbarian_GroundStompEffect = 0x7580,
            Barbarian_HammerOfTheAncients = 0x1389c,
            Barbarian_IgnorePain = 0x136a8,
            Barbarian_Leap = 0x16ce1,
            Barbarian_Overpower = 0x26dc1,
            Barbarian_Passive_Animosity = 0x321ac,
            Barbarian_Passive_BerserkerRage = 0x32183,
            Barbarian_Passive_Bloodthirst = 0x321a1,
            Barbarian_Passive_BoonOfBulKathos = 0x31f3b,
            Barbarian_Passive_Brawler = 0x3214d,
            Barbarian_Passive_InspiringPresence = 0x322ea,
            Barbarian_Passive_Juggernaut = 0x3238b,
            Barbarian_Passive_NervesOfSteel = 0x352db,
            Barbarian_Passive_NoEscape = 0x31fb5,
            Barbarian_Passive_PoundOfFlesh = 0x32195,
            Barbarian_Passive_Relentless = 0x32256,
            Barbarian_Passive_Ruthless = 0x32177,
            Barbarian_Passive_Superstition = 0x322b3,
            Barbarian_Passive_ToughAsNails = 0x32418,
            Barbarian_Passive_Unforgiving = 0x321f4,
            Barbarian_Passive_WeaponsMaster = 0x32543,
            Barbarian_Rend = 0x11348,
            Barbarian_Revenge = 0x1ab1e,
            Barbarian_Revenge_Buff = 0x1ab20,
            Barbarian_SeismicSlam = 0x153cd,
            Barbarian_Sprint = 0x132d7,
            Barbarian_ThreateningShout = 0x134e5,
            Barbarian_WarCry = 0x13ecc,
            Barbarian_WeaponThrow = 0x16ebd,
            Barbarian_Whirlwind = 0x17828,
            Barbarian_WrathOfTheBerserker = 0x136f7,
            BareHandedPassive = 0x75c1,
            Barrel_Explode_Instant = 0x6c8,
            Beast_Weapon_Melee_Instant = 0x1aae9,
            BeastCharge = 0x75c3,
            BodyGuard_Teleport = 0x20079,
            Brickhouse_Enrage = 0x11c09,
            BurrowInSetup = 0x1113d,
            BurrowOut = 0x75cd,
            BurrowOut_NoFacing = 0x125da,
            BurrowStartBuff = 0x75ce,
            Cain_Intro_Swing = 0x19031,
            CalldownGrenade = 0x16413,
            Callout_Cooldown = 0x20c51,
            Camera_Focus_Buff = 0x2502b,
            Camera_Focus_Pet_Buff = 0x25034,
            CannotDieDuringBuff = 0x3713f,
            ChampionClone = 0x75d6,
            ChampionTeleport = 0x75d7,
            Charmed_Monster_Ranged_Projectile = 0x2065a,
            Charmed_Weapon_Melee_Instant = 0x20657,
            Cooldown = 0x75e0,
            CopiedVisualEffectsBuff = 0x163ac,
            CoreElite_DropPod = 0x20ea0,
            CorpulentExplode = 0x75e2,
            CritDebuffCold = 0x75e4,
            CryptChildEat = 0x6ca,
            CryptChildLeapOut = 0x75e9,
            CryptChildLeapOutBuff = 0x75ea,
            DamageAttribute = 0x15088,
            DebuffBleed = 0x37c47,
            DebuffBlind = 0x19330,
            DebuffChilled = 0x75f3,
            DebuffFeared = 0x18a8a,
            DebuffRooted = 0x18a8b,
            DebuffSlowed = 0x18a6b,
            DebuffStunned = 0x18a88,
            DemonFlyer_Projectile = 0x1feee,
            DemonHunter_BolaShot = 0x12ef0,
            DemonHunter_Caltrops = 0x1f8c0,
            DemonHunter_Chakram = 0x1f8bd,
            DemonHunter_ClusterArrow = 0x1f8be,
            DemonHunter_Companion = 0x20a3f,
            DemonHunter_ElementalArrow = 0x200fd,
            DemonHunter_EntanglingShot = 0x12861,
            DemonHunter_EvasiveFire = 0x20c41,
            DemonHunter_EvasiveFire_Flip = 0x20c88,
            DemonHunter_FanOfKnives = 0x12eea,
            DemonHunter_Grenades = 0x15252,
            DemonHunter_HungeringArrow = 0x1f8bf,
            DemonHunter_Impale = 0x20126,
            DemonHunter_MarkedForDeath = 0x1feb2,
            DemonHunter_Multishot = 0x12f51,
            DemonHunter_Passive_Archery = 0x33346,
            DemonHunter_Passive_Ballistics = 0x2604b,
            DemonHunter_Passive_Brooding = 0x33771,
            DemonHunter_Passive_CullTheWeak = 0x26049,
            DemonHunter_Passive_CustomEngineering = 0x32ee2,
            DemonHunter_Passive_Grenadier = 0x32f8b,
            DemonHunter_Passive_HotPursuit = 0x2604d,
            DemonHunter_Passive_NightStalker = 0x354ee,
            DemonHunter_Passive_NumbingTraps = 0x3551e,
            DemonHunter_Passive_Perfectionist = 0x2604a,
            DemonHunter_Passive_Sharpshooter = 0x26043,
            DemonHunter_Passive_SteadyAim = 0x2820b,
            DemonHunter_Passive_TacticalAdvantage = 0x35511,
            DemonHunter_Passive_ThrillOfTheHunt = 0x33919,
            DemonHunter_Passive_Vengeance = 0x26042,
            DemonHunter_Preparation = 0x1f8bc,
            DemonHunter_RainOfVengeance = 0x1ff0f,
            DemonHunter_RapidFire = 0x20078,
            DemonHunter_Sentry = 0x1f8c1,
            DemonHunter_Sentry_TurretAttack = 0x1fa7d,
            DemonHunter_ShadowPower = 0x1ff0e,
            DemonHunter_SmokeScreen = 0x1fe87,
            DemonHunter_SpikeTrap = 0x12625,
            DemonHunter_Strafe = 0x20b8e,
            DemonHunter_Vault = 0x1b26f,
            DestructableObjectChandelier_AOE = 0x7601,
            DH_Companion_ChargeAttack = 0x20aff,
            DH_Companion_MeleeAttack = 0x377a8,
            DH_rainofArrows_shadowBeast_bombDrop = 0x24a3b,
            DOTDebuff = 0x175d5,
            DrinkHealthPotion = 0x7603,
            DualWieldBuff = 0x2f39e,
            EatCorpse = 0x7606,
            EmoteAttack = 0x2df5e,
            EmoteBye = 0x2d2fd,
            EmoteDie = 0x2d2ff,
            EmoteFollow = 0x2d2d2,
            EmoteGive = 0x2d2f9,
            EmoteGo = 0x2d51d,
            EmoteHelp = 0x2d305,
            EmoteHold = 0x2df60,
            EmoteLaugh = 0x2df62,
            EmoteNo = 0x2df5c,
            EmoteRetreat = 0x2df5f,
            EmoteRun = 0x2d4fe,
            EmoteSorry = 0x2d2fb,
            EmoteStay = 0x2df5d,
            EmoteTakeObjective = 0x2df61,
            EmoteThanks = 0x2d2fa,
            EmoteWait = 0x2d500,
            EmoteYes = 0x2df5b,
            Enchantress_Charm = 0x18ea9,
            Enchantress_Cripple = 0x149f5,
            Enchantress_Disorient = 0x18e66,
            Enchantress_FocusedMind = 0x18c31,
            Enchantress_ForcefulPush = 0x18e51,
            Enchantress_MassCharm = 0x31334,
            Enchantress_Melee_Instant = 0x3835e,
            Enchantress_PoweredArmor = 0x18c55,
            Enchantress_ReflectMissiles = 0x18ef5,
            Enchantress_ReflectMissiles_Proc = 0x18deb,
            Enchantress_RunAway = 0x2d758,
            Enchantress_ScorchedEarth = 0x35ec8,
            EnterRecallPortal = 0x31342,
            EnterStoneOfRecall = 0x30d64,
            EscortingBuff = 0x150e1,
            ExitRecallPortal = 0x31362,
            ExitStoneOfRecall = 0x30d67,
            Frenzy_Affix = 0x1e3c3,
            g_killElitePack = 0x38559,
            g_levelUp = 0x14fc2,
            Generic_SetCannotBeAddedToAITargetList = 0x1f96a,
            Generic_SetDoesFakeDamage = 0x1f973,
            Generic_SetInvisible = 0x1294b,
            Generic_SetInvulnerable = 0xf50b,
            Generic_SetObserver = 0x1f971,
            Generic_SetTakesNoDamage = 0x1f972,
            Generic_SetUntargetable = 0xf4ca,
            Generic_Taunt = 0xed69,
            GenericArrow_Projectile = 0x7622,
            Ghost_A_Unique_House1000Undead_Slow = 0x172fc,
            Ghost_Melee_Drain = 0x7623,
            Ghost_SoulSiphon = 0x7624,
            GhostWalkThroughWalls = 0x18316,
            GizmoOperatePortalWithAnimation = 0x7629,
            Goatman_Cold_Shield = 0x1e116,
            Goatman_Iceball = 0x18305,
            Goatman_Lightning_Shield = 0x762b,
            Goatman_Moonclan_Ranged_Projectile = 0x762c,
            Goatman_Shaman_Lightningbolt = 0x12e1e,
            GoatmanDrumsBeating = 0x17cd9,
            GoatMutantEnrage = 0x20204,
            GoatMutantGroundSmash = 0x20273,
            GraveDigger_Knockback_Attack = 0x762f,
            GraveRobber_Dodge_Left = 0x7630,
            GraveRobber_Dodge_Right = 0x7631,
            graveRobber_Projectile = 0x7632,
            HealingWell_Heal = 0x7638,
            Hearth = 0x7639,
            Hearth_Finish = 0x763a,
            HellPortalSummoningMachineActivate = 0x1cdd2,
            HelperArcherProjectile = 0x11e49,
            Hireling_Callout_BattleCry = 0x15435,
            Hireling_Callout_BattleFinished = 0x1ca4b,
            Hireling_Dismiss = 0x2fe2e,
            Hireling_Dismiss_Buff = 0x2fe07,
            Hireling_Dismiss_Buff_Remove = 0x2fe9b,
            IdentifyWithCast = 0x375c5,
            ImmuneToFearDuringBuff = 0x764b,
            ImmuneToRootDuringBuff = 0x764c,
            ImmuneToSnareDuringBuff = 0x764d,
            ImmuneToStunDuringBuff = 0x764e,
            Interact_Crouching = 0x764f,
            Interact_Normal = 0x7650,
            InvisibileDuringBuff = 0x7651,
            InvulnerableDuringBuff = 0x7652,
            Knockback = 0x11320,
            Knockdown = 0x7658,
            Laugh = 0x7663,
            Laugh_SkeletonKing = 0x14adb,
            Leah_Vortex = 0x16e87,
            MagicPainting_Summon_Skeleton = 0x7669,
            Manual_Walk = 0x37f08,
            MastaBlasta_Steed_Combine = 0x233a1,
            Monk_BlindingFlash = 0x216fa,
            Monk_BreathOfHeaven = 0x10e0a,
            Monk_CripplingWave = 0x17837,
            Monk_CycloneStrike = 0x368f1,
            Monk_DashingStrike = 0x177cb,
            Monk_DeadlyReach = 0x17713,
            Monk_ExplodingPalm = 0x17c30,
            Monk_FistsofThunder = 0x176c4,
            Monk_InnerSanctuary = 0x17bc6,
            Monk_LashingTailKick = 0x1b43c,
            Monk_MantraOfConviction = 0x17554,
            Monk_MantraOfEvasion = 0x2ef95,
            Monk_MantraOfHealing = 0x10f72,
            Monk_MantraOfRetribution = 0x10f6c,
            Monk_MysticAlly = 0x1e148,
            Monk_MysticAlly_Pet_RuneA_Kick = 0x294c3,
            Monk_MysticAlly_Pet_RuneB_WaveAttack = 0x2956d,
            Monk_MysticAlly_Pet_RuneC_GroundPunch = 0x296f3,
            Monk_MysticAlly_Pet_RuneD_AOEAttack = 0x29700,
            Monk_MysticAlly_Pet_Weapon_Melee_Instant = 0x29479,
            Monk_Passive_BeaconOfYtar = 0x330d0,
            Monk_Passive_ChantOfResonance = 0x26333,
            Monk_Passive_CombinationStrike = 0x3552f,
            Monk_Passive_ExaltedSoul = 0x33083,
            Monk_Passive_FleetFooted = 0x33085,
            Monk_Passive_GuidingLight = 0x2634c,
            Monk_Passive_NearDeathExperience = 0x26344,
            Monk_Passive_OneWithEverything = 0x332f8,
            Monk_Passive_Pacifism = 0x33395,
            Monk_Passive_Resolve = 0x33a7d,
            Monk_Passive_SeizeTheInitiative = 0x332dc,
            Monk_Passive_SixthSense = 0x332d6,
            Monk_Passive_TheGuardiansPath = 0x33394,
            Monk_Passive_Transcendence = 0x33162,
            Monk_ResistAura = 0x10f71,
            Monk_ResistAura_RuneC_Arcane = 0x233b8,
            Monk_ResistAura_RuneC_Cold = 0x23345,
            Monk_ResistAura_RuneC_Fire = 0x23016,
            Monk_ResistAura_RuneC_Holy = 0x233c2,
            Monk_ResistAura_RuneC_Lightning = 0x2333c,
            Monk_ResistAura_RuneC_Poison = 0x2334a,
            Monk_Serenity = 0x177d7,
            Monk_SevenSidedStrike = 0x179b6,
            Monk_SweepingWind = 0x1775a,
            Monk_TempestRush = 0x1da62,
            Monk_WaveOfLight = 0x17721,
            Monk_WayOfTheHundredFists = 0x17b56,
            Monster_Ranged_Projectile = 0x767e,
            Monster_Spell_Projectile = 0x7682,
            MonsterAffix_ArcaneEnchanted = 0x34642,
            MonsterAffix_ArcaneEnchanted_Champion = 0x35fca,
            MonsterAffix_ArcaneEnchanted_Minion = 0x36023,
            MonsterAffix_ArcaneEnchanted_New_PetBasic = 0x35a17,
            MonsterAffix_ArcaneEnchantedCast = 0x34707,
            MonsterAffix_Avenger_Buff = 0x373f4,
            MonsterAffix_Avenger_Champion = 0x373f1,
            MonsterAffix_Ballista = 0x163da,
            MonsterAffix_ChampionBuff = 0x3359d,
            MonsterAffix_DesecratorBuff = 0x261ca,
            MonsterAffix_DesecratorBuff_Champion = 0x35fcb,
            MonsterAffix_DesecratorCast = 0x261c9,
            MonsterAffix_DieTogether = 0x16460,
            MonsterAffix_Electrified = 0x13e0c,
            MonsterAffix_Electrified_Minion = 0x1ad4b,
            MonsterAffix_EnrageExecute = 0x3359f,
            MonsterAffix_ExtraHealth = 0x113fa,
            MonsterAffix_Fast = 0x114c1,
            MonsterAffix_Frozen = 0x16020,
            MonsterAffix_FrozenCast = 0x386ed,
            MonsterAffix_FrozenRare = 0x386f5,
            MonsterAffix_Healthlink = 0x11647,
            MonsterAffix_Illusionist = 0x115c4,
            MonsterAffix_Jailer = 0x36617,
            MonsterAffix_Jailer_Champion = 0x36619,
            MonsterAffix_JailerCast = 0x36618,
            MonsterAffix_Knockback = 0x113ff,
            MonsterAffix_Linked = 0x374c1,
            MonsterAffix_MissileDampening = 0x16394,
            MonsterAffix_Molten = 0x160ca,
            MonsterAffix_Mortar = 0x34acc,
            MonsterAffix_MortarCast = 0x34acd,
            MonsterAffix_Pheonix = 0x1d74f,
            MonsterAffix_Plagued = 0x161c6,
            MonsterAffix_PlaguedCast = 0x386cb,
            MonsterAffix_Puppetmaster = 0x1156f,
            MonsterAffix_Puppetmaster_Minion = 0x11570,
            MonsterAffix_ReflectsDamage = 0x385dd,
            MonsterAffix_Shaman = 0x1152f,
            MonsterAffix_Shielding = 0x37485,
            MonsterAffix_ShieldingCast = 0x37486,
            MonsterAffix_TeleporterBuff = 0x26136,
            MonsterAffix_TeleporterCast = 0x26137,
            MonsterAffix_Vampiric = 0x112a5,
            MonsterAffix_VortexBuff = 0x1d5f2,
            MonsterAffix_VortexBuff_Champion = 0x35fcc,
            MonsterAffix_VortexCast = 0x1d5f1,
            MonsterAffix_Waller = 0x373f5,
            MonsterAffix_WallerCast = 0x373f6,
            MonsterAffix_WallerRare = 0x386cd,
            MonsterAffix_WallerRareCast = 0x386ce,
            NPC_LookAt = 0x7686,
            OnDeathArcane = 0x7687,
            OnDeathCold = 0x7688,
            OnDeathFire = 0x7689,
            OnDeathLightning = 0x768a,
            OnDeathPoison = 0x768b,
            Operate_Helper_Attach = 0x768c,
            PickupNearby = 0x20388,
            PlagueOfToadsKnockback = 0x241a4,
            Proxy_Delayed_Power = 0x76b1,
            Punch = 0x76b7,
            PvP_DamageBuff = 0x317cd,
            Quest_CanyonBridge_Enchantress_RevealFootsteps = 0x193aa,
            Quest_CanyonBridge_Player_RevealFootsteps = 0x193a9,
            QuillDemon_Projectile = 0x1a4d1,
            RangedEscort_Projectile = 0x76ba,
            Resurrection_Buff = 0x76d8,
            Rockworm_Web = 0x76df,
            RootTryGrab = 0x76e1,
            SandsharkBurrowIn = 0x76e8,
            Scavenger_Leap = 0x6d8,
            ScavengerBurrowIn = 0x76f2,
            ScavengerBurrowOut = 0x76f3,
            Scoundrel_Anatomy = 0x76f6,
            Scoundrel_Bandage = 0x76f7,
            Scoundrel_CripplingShot = 0x175bb,
            Scoundrel_DirtyFighting = 0x17c9c,
            Scoundrel_Hysteria = 0x30de9,
            Scoundrel_Multishot = 0x76fa,
            Scoundrel_PoisonArrow = 0x76fc,
            Scoundrel_PowerShot = 0x175ca,
            Scoundrel_Ranged_Projectile = 0x1863e,
            Scoundrel_RunAway = 0x18640,
            Scoundrel_Vanish = 0x7700,
            ScrollBuff = 0x7705,
            SelectingSkill = 0x350fc,
            SetItemBonusBuff = 0x1e086,
            SetMode_EscortFollow = 0x7707,
            Shield_Skeleton_Melee_Instant = 0x770a,
            ShieldSkeleton_Shield = 0x7709,
            Shrine_Desecrated_Blessed = 0x770c,
            Shrine_Desecrated_Enlightened = 0x770d,
            Shrine_Desecrated_Fortune = 0x770e,
            Shrine_Desecrated_Frenzied = 0x770f,
            SkeletonArcher_Projectile = 0x771f,
            SkeletonKing_Cleave = 0x7728,
            SkeletonKing_KillSummonedSkeletons = 0x155e3,
            SkeletonKing_Summon_Skeleton = 0x7720,
            SkeletonKing_Teleport = 0x135e6,
            SkeletonKing_Teleport_Away = 0x13e60,
            SkeletonKing_Whirlwind = 0x12060,
            skeletonMage_poison_death = 0x7725,
            SkeletonSummoner_Projectile = 0x7727,
            SkillOverrideStartedOrEnded = 0x3605b,
            Snakeman_MeleeStealth = 0x7730,
            Snakeman_MeleeUnstealth = 0x7731,
            SnakemanCaster_ElectricBurst = 0x772d,
            Spider_Web_Slow = 0x12ca1,
            SporeCloud = 0x773d,
            StealthBuff = 0x773f,
            StitchExplode = 0x7741,
            StitchMeleeAlternate = 0x7742,
            StitchPush = 0x7743,
            Suicide_Proc = 0x774a,
            Summon_Skeleton = 0x774f,
            Summon_Skeleton_Jondar = 0x29114,
            Summon_Skeleton_OnSpawn = 0x7751,
            Summon_Skeleton_Orb = 0x7752,
            Summon_Triune_Demon = 0x7753,
            Summon_Zombie_Crawler = 0x7756,
            Summon_Zombie_Vomit = 0x1720e,
            Summoned = 0x774c,
            Summoning_Machine_Summon = 0x1cb4c,
            TarPitSlowOn = 0x10622,
            Taunted_Monster_Ranged_Projectile = 0x33fd8,
            Taunted_Weapon_Melee_Instant = 0x33fd9,
            Templar_Guardian = 0x7697,
            Templar_Heal = 0x6d3,
            Templar_Inspire = 0x7694,
            Templar_Intervene = 0x16ef2,
            Templar_Intervene_Proc = 0x16f38,
            Templar_Intimidate = 0x16ecd,
            Templar_Loyalty = 0x7695,
            Templar_Melee_Instant = 0x3835f,
            Templar_Onslaught = 0x16ec0,
            Templar_ShieldCharge = 0x7698,
            Thorns = 0x775a,
            Trait_Barbarian_Fury = 0x757e,
            Trait_Monk_Spirit = 0xce11,
            Trait_Witchdoctor_ZombieDogSpawner_Passive = 0x1abf8,
            TransformToActivatedTriune = 0x7763,
            trDun_Cath_WallCollapse_Damage = 0x2d768,
            trDun_Cath_WallCollapse_Damage_offset = 0x37a6d,
            TreasureGoblin_Escape = 0x19b9b,
            TreasureGoblin_ThrowPortal = 0xd634,
            TreasureGoblin_ThrowPortal_Fast = 0x19cc1,
            TreasureGoblinPause = 0xd327,
            TriuneBerserker_Hit = 0x7767,
            TriuneSummoner_Projectile = 0x776a,
            TriuneSummoner_Shield = 0x776b,
            TriuneSummoner_SplitSummonCast = 0x776c,
            TriuneVesselCharge = 0x776d,
            TriuneVesselOverpower = 0x776e,
            trOut_LogStack_Short_Damage = 0x2d71a,
            trOut_LogStack_Trap = 0x187bf,
            trout_tristramfields_punji_trap_aoe = 0x1647d,
            trout_tristramfields_punji_trap_mirror_aoe = 0x1749b,
            Unburied_Knockback = 0x7774,
            Unburied_Melee_Attack = 0x7775,
            Unburied_Wreckable_Attack = 0x31668,
            UnholyShield = 0x1e061,
            UninterruptibleDuringBuff = 0x1367e,
            Unique_Monster_Generic_Projectile = 0x253dc,
            UntargetableDuringBuff = 0x7776,
            UseArcaneGlyph = 0x286b1,
            UseDungeonStone = 0x35c9e,
            UseHealthGlyph = 0x7778,
            UseItem = 0x6df,
            UseManaGlyph = 0x7779,
            UseStoneOfRecall = 0x2ec66,
            Walk = 0x777c,
            Warp = 0x777d,
            WarpInMagical = 0x2072e,
            Weapon_Melee_Instant = 0x7780,
            Weapon_Melee_Instant_BothHand = 0x7781,
            Weapon_Melee_Instant_OffHand = 0x7782,
            Weapon_Melee_Instant_Wreckables = 0x31669,
            Weapon_Melee_NoClose = 0x1124a,
            Weapon_Melee_Obstruction = 0x7783,
            Weapon_Melee_Reach_Instant = 0x7784,
            Weapon_Melee_Reach_Instant_Freeze_Facing = 0x1c3a8,
            Weapon_Ranged_Instant = 0x7786,
            Weapon_Ranged_Projectile = 0x7787,
            Weapon_Ranged_Wand = 0x7789,
            Witchdoctor_AcidCloud = 0x11337,
            Witchdoctor_BigBadVoodoo = 0x1ca9a,
            Witchdoctor_CorpseSpider = 0x110ea,
            Witchdoctor_CorpseSpider_Leap = 0x1a25f,
            Witchdoctor_FetishArmy = 0x11c51,
            Witchdoctor_FetishArmy_Hunter = 0x1d17e,
            Witchdoctor_FetishArmy_Melee = 0x37582,
            Witchdoctor_FetishArmy_Shaman = 0x1ceaa,
            Witchdoctor_Firebats = 0x19deb,
            Witchdoctor_Firebomb = 0x107ef,
            Witchdoctor_Gargantuan = 0x77a0,
            Witchdoctor_Gargantuan_Cleave = 0x1dc56,
            Witchdoctor_Gargantuan_Slam = 0x1dc57,
            Witchdoctor_Gargantuan_Smash = 0x2d9e3,
            Witchdoctor_GraspOfTheDead = 0x10e3e,
            Witchdoctor_Haunt = 0x14692,
            Witchdoctor_Hex = 0x77a7,
            Witchdoctor_Hex_ChickenWalk = 0x3016e,
            Witchdoctor_Hex_Explode = 0x2e01a,
            Witchdoctor_Hex_Fetish = 0x1a325,
            Witchdoctor_Hex_Fetish_Heal = 0x1a4de,
            Witchdoctor_Horrify = 0x10854,
            Witchdoctor_Locust_Swarm = 0x110eb,
            Witchdoctor_MassConfusion = 0x10810,
            Witchdoctor_Passive_BadMedicine = 0x352e2,
            Witchdoctor_Passive_BloodRitual = 0x32eb8,
            Witchdoctor_Passive_CircleOfLife = 0x32ebb,
            Witchdoctor_Passive_FetishSycophants = 0x355dc,
            Witchdoctor_Passive_FierceLoyalty = 0x32eff,
            Witchdoctor_Passive_GraveInjustice = 0x3544f,
            Witchdoctor_Passive_GruesomeFeast = 0x32ed2,
            Witchdoctor_Passive_JungleFortitude = 0x35370,
            Witchdoctor_Passive_PierceTheVeil = 0x32ef4,
            Witchdoctor_Passive_RushOfEssence = 0x32eb5,
            Witchdoctor_Passive_SpiritualAttunement = 0x32eb9,
            Witchdoctor_Passive_SpiritVessel = 0x35585,
            Witchdoctor_Passive_TribalRites = 0x32ed9,
            Witchdoctor_Passive_VisionQuest = 0x33091,
            Witchdoctor_Passive_ZombieHandler = 0x32eb3,
            Witchdoctor_PlagueOfToads = 0x19fe1,
            Witchdoctor_PlagueOfToads_BigToad_TongueSlap = 0x35eec,
            Witchdoctor_PlagueOfToads_BigToadAttack = 0x1a060,
            Witchdoctor_PoisonDart = 0x1930d,
            Witchdoctor_Sacrifice = 0x190ac,
            Witchdoctor_SoulHarvest = 0x10820,
            Witchdoctor_SpiritBarrage = 0x1a7da,
            Witchdoctor_SpiritBarrage_RuneC_AOE = 0x2d867,
            Witchdoctor_SpiritWalk = 0x19efd,
            Witchdoctor_SummonZombieDog = 0x190ad,
            Witchdoctor_WallOfZombies = 0x20eb5,
            Witchdoctor_ZombieCharger = 0x12113,
            Witchdoctor_ZombieDog_Melee = 0x37584,
            Wizard_ArcaneOrb = 0x77cc,
            Wizard_ArcaneTorrent = 0x20d38,
            Wizard_ArcaneTorrent_RuneC_Mine = 0x286de,
            Wizard_Archon = 0x20ed8,
            Wizard_Archon_ArcaneBlast = 0x28dbb,
            Wizard_Archon_ArcaneStrike = 0x20ffe,
            Wizard_Archon_Cancel = 0x28ad8,
            Wizard_Archon_DisintegrationWave = 0x21046,
            Wizard_Archon_SlowTime = 0x211ef,
            Wizard_Archon_Teleport = 0x28ee0,
            Wizard_Blizzard = 0x77d8,
            Wizard_DiamondSkin = 0x1274f,
            Wizard_Disintegrate = 0x1659d,
            Wizard_Electrocute = 0x6e5,
            Wizard_EnergyArmor = 0x153cf,
            Wizard_EnergyShield = 0x77f4,
            Wizard_EnergyTwister = 0x12d39,
            Wizard_ExplosiveBlast = 0x155e5,
            Wizard_Familiar = 0x18330,
            Wizard_FrostNova = 0x77fe,
            Wizard_Hydra = 0x7805,
            Wizard_Hydra_DefaultFire_Prototype = 0x12d0c,
            Wizard_Hydra_RuneAcid_Prototype = 0x12d0a,
            Wizard_Hydra_RuneArcane_Prototype = 0x12d0b,
            Wizard_Hydra_RuneBig_Prototype = 0x1483e,
            Wizard_Hydra_RuneFrost_Prototype = 0x14460,
            Wizard_Hydra_RuneLightning_Prototype = 0x12d09,
            Wizard_IceArmor = 0x11e07,
            Wizard_MagicMissile = 0x7818,
            Wizard_MagicWeapon = 0x1294c,
            Wizard_Meteor = 0x10e46,
            Wizard_MirrorImage = 0x17eeb,
            Wizard_Passive_ArcaneDynamo = 0x32fb7,
            Wizard_Passive_AstralPresence = 0x32e58,
            Wizard_Passive_Blur = 0x32e54,
            Wizard_Passive_ColdBlooded = 0x373fd,
            Wizard_Passive_Conflagration = 0x353bc,
            Wizard_Passive_CriticalMass = 0x35429,
            Wizard_Passive_Evocation = 0x32e59,
            Wizard_Passive_GalvanizingWard = 0x32e9d,
            Wizard_Passive_GlassCannon = 0x32e57,
            Wizard_Passive_Illusionist = 0x32ea3,
            Wizard_Passive_Paralysis = 0x3742c,
            Wizard_Passive_PowerHungry = 0x32e5e,
            Wizard_Passive_Prodigy = 0x32e6d,
            Wizard_Passive_TemporalFlux = 0x32e5d,
            Wizard_Passive_UnstableAnomaly = 0x32e5a,
            Wizard_RayOfFrost = 0x16cd3,
            Wizard_ShockPulse = 0x783f,
            Wizard_SlowTime = 0x6e9,
            Wizard_SpectralBlade = 0x1177c,
            Wizard_StormArmor = 0x12303,
            Wizard_Teleport = 0x29198,
            Wizard_WaveOfForce = 0x784c,
            WoodWraithSummonSpores = 0x7850,
            WorldCreatingBuff = 0x36974,
            ZombieEatStart = 0x2b933,
            ZombieEatStop = 0x2b935,
            ZombieFemale_Projectile = 0x1afb6,
            ZombieKillerGrab = 0x6eb
        }

        public enum ItemLocation : uint
        {
            Unknown = uint.MaxValue, //  meaning NPC or item on the ground.
            Backpack = 0,
            Head = 1,
            Torso = 2,
            RightHand = 3,
            LeftHand = 4,
            Hands = 5,
            Waist = 6,
            Feet = 7,
            Shoulders = 8,
            Legs = 9,
            Bracers = 10,
            LeftFinger = 11,
            RightFinger = 12,
            Neck = 13,
            Talisman = 14,
            Stash = 15, //  
            Gold = 16,   // these 2 in the thread from ownedcore is wrong (because old version d3)
            Merchant = 18, // Not Sure
            InSocket = 20, // Not Sure
            PetRightHand = 23,
            PetLeftHand = 24,
            PetSpecial = 25,
            PetLeftFinger = 28,
            PetRightFinger = 27,
            PetNeck = 26,
        }
    }
}
