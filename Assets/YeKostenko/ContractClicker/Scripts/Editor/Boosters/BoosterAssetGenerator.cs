using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

using YeKostenko.ContractClicker.Data.Boosters;
using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Editor.Boosters
{
    public static class BoosterAssetGenerator
    {
        private const string BoosterFolder = "Assets/YeKostenko/ContractClicker/Data/Boosters";
        private const string ResourcesFolder = "Assets/YeKostenko/ContractClicker/Resources";

        private struct ModEntry
        {
            public BoosterTarget Target;
            public StatId Stat;
            public float Value;
            public ModificationOperation Operation;

            public ModEntry(BoosterTarget target, StatId stat, float value, ModificationOperation operation)
            {
                Target = target;
                Stat = stat;
                Value = value;
                Operation = operation;
            }
        }

        private struct BoosterDef
        {
            public string Id;
            public string DisplayName;
            public string Description;
            public BoosterRarity Rarity;
            public Color RarityColor;
            public int Price;
            public float Duration;
            public ModEntry[] Mods;
        }

        [MenuItem("ContractClicker/Boosters/Generate All Booster Assets")]
        public static void GenerateAllBoosterAssets()
        {
            EnsureFolderExists("Assets/YeKostenko/ContractClicker/Data");
            EnsureFolderExists(BoosterFolder);
            EnsureFolderExists(ResourcesFolder);

            BoosterDef[] definitions = BuildBoosterDefinitions();
            List<BoosterConfig> createdConfigs = new List<BoosterConfig>(definitions.Length);

            for (int i = 0; i < definitions.Length; i++)
            {
                BoosterConfig config = CreateOrUpdateBoosterConfig(definitions[i]);
                createdConfigs.Add(config);
            }

            CreateOrUpdateShopConfig(createdConfigs);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[BoosterAssetGenerator] Generated {createdConfigs.Count} boosters and saved BoosterShopConfig.");
        }

        private static BoosterDef[] BuildBoosterDefinitions()
        {
            const BoosterTarget Manual = BoosterTarget.ManualWorker;
            const BoosterTarget Auto = BoosterTarget.AutomaticWorkers;
            const BoosterTarget All = BoosterTarget.AllWorkers;
            const ModificationOperation Mul = ModificationOperation.Mul;
            const ModificationOperation Add = ModificationOperation.Add;

            Color colorCommon = new Color(0.75f, 0.75f, 0.75f, 1f);
            Color colorRare = new Color(0.20f, 0.50f, 1.00f, 1f);
            Color colorEpic = new Color(0.70f, 0.20f, 1.00f, 1f);
            Color colorLegendary = new Color(1.00f, 0.75f, 0.10f, 1f);

            return new BoosterDef[]
            {
                new BoosterDef
                {
                    Id = "booster_quick_hands", DisplayName = "Quick Hands",
                    Description = "Increases click power by 25% for 60 seconds.",
                    Rarity = BoosterRarity.Common, RarityColor = colorCommon, Price = 50, Duration = 60f,
                    Mods = new ModEntry[] { new ModEntry(Manual, StatId.ClickPower, 1.25f, Mul) }
                },
                new BoosterDef
                {
                    Id = "booster_lucky_click", DisplayName = "Lucky Click",
                    Description = "Adds 10% critical strike chance to manual clicks for 90 seconds.",
                    Rarity = BoosterRarity.Common, RarityColor = colorCommon, Price = 65, Duration = 90f,
                    Mods = new ModEntry[] { new ModEntry(Manual, StatId.CritChance, 0.10f, Add) }
                },
                new BoosterDef
                {
                    Id = "booster_worker_hustle", DisplayName = "Worker Hustle",
                    Description = "Automatic workers produce 30% more per tick for 60 seconds.",
                    Rarity = BoosterRarity.Common, RarityColor = colorCommon, Price = 80, Duration = 60f,
                    Mods = new ModEntry[] { new ModEntry(Auto, StatId.YieldPerTick, 1.30f, Mul) }
                },
                new BoosterDef
                {
                    Id = "booster_swift_workers", DisplayName = "Swift Workers",
                    Description = "Automatic workers act 25% more frequently for 2 minutes.",
                    Rarity = BoosterRarity.Rare, RarityColor = colorRare, Price = 160, Duration = 120f,
                    Mods = new ModEntry[] { new ModEntry(Auto, StatId.TickInterval, 0.75f, Mul) }
                },
                new BoosterDef
                {
                    Id = "booster_double_strike", DisplayName = "Double Strike",
                    Description = "Increases click power by 50% for 2 minutes.",
                    Rarity = BoosterRarity.Rare, RarityColor = colorRare, Price = 200, Duration = 120f,
                    Mods = new ModEntry[] { new ModEntry(Manual, StatId.ClickPower, 1.50f, Mul) }
                },
                new BoosterDef
                {
                    Id = "booster_hawk_eye", DisplayName = "Hawk Eye",
                    Description = "All workers gain 15% critical strike chance for 2 minutes.",
                    Rarity = BoosterRarity.Rare, RarityColor = colorRare, Price = 220, Duration = 120f,
                    Mods = new ModEntry[] { new ModEntry(All, StatId.CritChance, 0.15f, Add) }
                },
                new BoosterDef
                {
                    Id = "booster_efficiency_boost", DisplayName = "Efficiency Boost",
                    Description = "Automatic workers produce 60% more per tick for 2 minutes.",
                    Rarity = BoosterRarity.Rare, RarityColor = colorRare, Price = 250, Duration = 120f,
                    Mods = new ModEntry[] { new ModEntry(Auto, StatId.YieldPerTick, 1.60f, Mul) }
                },
                new BoosterDef
                {
                    Id = "booster_berserker", DisplayName = "Berserker",
                    Description = "Doubles click power for 3 minutes.",
                    Rarity = BoosterRarity.Epic, RarityColor = colorEpic, Price = 550, Duration = 180f,
                    Mods = new ModEntry[] { new ModEntry(Manual, StatId.ClickPower, 2.00f, Mul) }
                },
                new BoosterDef
                {
                    Id = "booster_overdrive", DisplayName = "Overdrive",
                    Description = "Automatic workers produce 80% more and act 35% faster for 3 minutes.",
                    Rarity = BoosterRarity.Epic, RarityColor = colorEpic, Price = 750, Duration = 180f,
                    Mods = new ModEntry[]
                    {
                        new ModEntry(Auto, StatId.YieldPerTick, 1.80f, Mul),
                        new ModEntry(Auto, StatId.TickInterval, 0.65f, Mul)
                    }
                },
                new BoosterDef
                {
                    Id = "booster_fury", DisplayName = "Fury",
                    Description = "All workers gain 25% crit chance and 50% more crit damage for 3 minutes.",
                    Rarity = BoosterRarity.Epic, RarityColor = colorEpic, Price = 850, Duration = 180f,
                    Mods = new ModEntry[]
                    {
                        new ModEntry(All, StatId.CritChance, 0.25f, Add),
                        new ModEntry(All, StatId.CritMultiplier, 1.50f, Mul)
                    }
                },
                new BoosterDef
                {
                    Id = "booster_power_fist", DisplayName = "Power Fist",
                    Description = "Click power x2.5 and +20% critical chance for 3 minutes.",
                    Rarity = BoosterRarity.Epic, RarityColor = colorEpic, Price = 950, Duration = 180f,
                    Mods = new ModEntry[]
                    {
                        new ModEntry(Manual, StatId.ClickPower, 2.50f, Mul),
                        new ModEntry(Manual, StatId.CritChance, 0.20f, Add)
                    }
                },
                new BoosterDef
                {
                    Id = "booster_time_warp", DisplayName = "Time Warp",
                    Description = "All workers act 2.5x faster for 5 minutes.",
                    Rarity = BoosterRarity.Legendary, RarityColor = colorLegendary, Price = 2000, Duration = 300f,
                    Mods = new ModEntry[] { new ModEntry(All, StatId.TickInterval, 0.40f, Mul) }
                },
                new BoosterDef
                {
                    Id = "booster_midas_touch", DisplayName = "Midas Touch",
                    Description = "Triple click power and double critical damage for 5 minutes.",
                    Rarity = BoosterRarity.Legendary, RarityColor = colorLegendary, Price = 3000, Duration = 300f,
                    Mods = new ModEntry[]
                    {
                        new ModEntry(Manual, StatId.ClickPower, 3.00f, Mul),
                        new ModEntry(Manual, StatId.CritMultiplier, 2.00f, Mul)
                    }
                },
                new BoosterDef
                {
                    Id = "booster_automation_god", DisplayName = "Automation God",
                    Description = "Automatic workers produce 3x more and act twice as fast for 5 minutes.",
                    Rarity = BoosterRarity.Legendary, RarityColor = colorLegendary, Price = 3500, Duration = 300f,
                    Mods = new ModEntry[]
                    {
                        new ModEntry(Auto, StatId.YieldPerTick, 3.00f, Mul),
                        new ModEntry(Auto, StatId.TickInterval, 0.50f, Mul)
                    }
                },
                new BoosterDef
                {
                    Id = "booster_omnipotent", DisplayName = "Omnipotent",
                    Description = "Clicks, workers, speed, and crits all massively amplified for 5 minutes.",
                    Rarity = BoosterRarity.Legendary, RarityColor = colorLegendary, Price = 5000, Duration = 300f,
                    Mods = new ModEntry[]
                    {
                        new ModEntry(Manual, StatId.ClickPower, 2.00f, Mul),
                        new ModEntry(Auto, StatId.YieldPerTick, 2.50f, Mul),
                        new ModEntry(All, StatId.CritChance, 0.30f, Add),
                        new ModEntry(All, StatId.CritMultiplier, 1.80f, Mul),
                        new ModEntry(All, StatId.TickInterval, 0.55f, Mul)
                    }
                },
            };
        }

        private static BoosterConfig CreateOrUpdateBoosterConfig(BoosterDef def)
        {
            string assetPath = BoosterFolder + "/" + def.Id + ".asset";
            BoosterConfig asset = AssetDatabase.LoadAssetAtPath<BoosterConfig>(assetPath);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<BoosterConfig>();
                AssetDatabase.CreateAsset(asset, assetPath);
            }

            SerializedObject so = new SerializedObject(asset);
            so.FindProperty("_boosterId").stringValue = def.Id;
            so.FindProperty("_displayName").stringValue = def.DisplayName;
            so.FindProperty("_description").stringValue = def.Description;
            so.FindProperty("_rarity").intValue = (int)def.Rarity;
            so.FindProperty("_price").intValue = def.Price;
            so.FindProperty("_durationInSeconds").floatValue = def.Duration;
            so.FindProperty("_rarityColor").colorValue = def.RarityColor;

            SerializedProperty modsProp = so.FindProperty("_statModifications");
            modsProp.arraySize = def.Mods.Length;

            for (int i = 0; i < def.Mods.Length; i++)
            {
                SerializedProperty elem = modsProp.GetArrayElementAtIndex(i);
                elem.FindPropertyRelative("Target").intValue = (int)def.Mods[i].Target;
                elem.FindPropertyRelative("Stat").intValue = (int)def.Mods[i].Stat;
                elem.FindPropertyRelative("Value").floatValue = def.Mods[i].Value;
                elem.FindPropertyRelative("Operation").intValue = (int)def.Mods[i].Operation;
            }

            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(asset);
            return asset;
        }

        private static void CreateOrUpdateShopConfig(List<BoosterConfig> boosters)
        {
            string assetPath = ResourcesFolder + "/BoosterShopConfig.asset";
            BoosterShopConfig asset = AssetDatabase.LoadAssetAtPath<BoosterShopConfig>(assetPath);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<BoosterShopConfig>();
                AssetDatabase.CreateAsset(asset, assetPath);
            }

            SerializedObject so = new SerializedObject(asset);
            so.FindProperty("_shopRefreshIntervalInSeconds").floatValue = 300f;
            so.FindProperty("_slotsCount").intValue = 3;

            SerializedProperty boostersProp = so.FindProperty("_availableBoosters");
            boostersProp.arraySize = boosters.Count;
            for (int i = 0; i < boosters.Count; i++)
            {
                boostersProp.GetArrayElementAtIndex(i).objectReferenceValue = boosters[i];
            }

            SerializedProperty weightsProp = so.FindProperty("_rarityWeights");
            weightsProp.arraySize = 4;
            SetRarityWeight(weightsProp.GetArrayElementAtIndex(0), BoosterRarity.Common, 60f);
            SetRarityWeight(weightsProp.GetArrayElementAtIndex(1), BoosterRarity.Rare, 30f);
            SetRarityWeight(weightsProp.GetArrayElementAtIndex(2), BoosterRarity.Epic, 8f);
            SetRarityWeight(weightsProp.GetArrayElementAtIndex(3), BoosterRarity.Legendary, 2f);

            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(asset);
        }

        private static void SetRarityWeight(SerializedProperty element, BoosterRarity rarity, float weight)
        {
            element.FindPropertyRelative("Rarity").intValue = (int)rarity;
            element.FindPropertyRelative("SpawnWeight").floatValue = weight;
        }

        private static void EnsureFolderExists(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            int lastSlash = path.LastIndexOf('/');
            string parentPath = path.Substring(0, lastSlash);
            string folderName = path.Substring(lastSlash + 1);
            AssetDatabase.CreateFolder(parentPath, folderName);
        }
    }
}

