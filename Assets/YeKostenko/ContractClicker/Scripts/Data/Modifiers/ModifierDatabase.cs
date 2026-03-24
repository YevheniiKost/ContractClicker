using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Cysharp.Threading.Tasks;

using UnityEngine;

using YeKostenko.ContractClicker.Data.Game;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace YeKostenko.ContractClicker.Data.Modifiers
{
    [CreateAssetMenu(fileName = "ModifierDatabase", menuName = "ContractClicker/Modifiers/Modifier Database", order = 0)]
    public class ModifierDatabase : ScriptableObject, IModifierDatabase
    {
        public static async Task<ModifierDatabase> Load()
        {
            Object asset = await Resources.LoadAsync<ModifierDatabase>("ModifierDatabase");
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            return asset as ModifierDatabase;
        }

        [Header("Набори модифікаторів")]
        [SerializeField]
        private List<ModifierSetConfig> _modifierSets = new List<ModifierSetConfig>();

        [Header("Налаштування генерації")]
        [SerializeField]
        [Range(0f, 100f)]
        [Tooltip("Шанс що контракт отримає модифікатори")]
        private float _baseModifierChance = 60f;

        public IReadOnlyList<ModifierSetConfig> ModifierSets => _modifierSets;
        public float BaseModifierChance => _baseModifierChance;

        public IEnumerable<ModifierSetConfig> GetSetsByDifficulty(ContractDifficulty difficulty)
        {
            return _modifierSets.Where(s => s != null && s.Difficulty == difficulty);
        }

        public IEnumerable<ModifierSetConfig> GetSetsForLevel(int contractLevel)
        {
            return _modifierSets.Where(s => s != null && s.IsValidForLevel(contractLevel));
        }

        public IEnumerable<ModifierSetConfig> GetSets(ContractDifficulty difficulty, int contractLevel)
        {
            return _modifierSets.Where(s =>
                s != null &&
                s.Difficulty == difficulty &&
                s.IsValidForLevel(contractLevel)
            );
        }

        public ModifierSetConfig GetRandomSet(int contractLevel)
        {
            List<ModifierSetConfig> validSets = GetSetsForLevel(contractLevel).ToList();
            if (validSets.Count == 0)
            {
                return null;
            }

            float totalWeight = validSets.Sum(s => s.SpawnChance);
            float randomValue = Random.Range(0f, totalWeight);

            float currentWeight = 0f;
            foreach (ModifierSetConfig set in validSets)
            {
                currentWeight += set.SpawnChance;
                if (randomValue <= currentWeight)
                {
                    return set;
                }
            }

            return validSets.Last();
        }

        public ModifierSetConfig GetRandomSet(ContractDifficulty difficulty, int contractLevel)
        {
            var validSets = GetSets(difficulty, contractLevel).ToList();
            if (validSets.Count == 0)
                return null;

            // Зважений вибір
            float totalWeight = validSets.Sum(s => s.SpawnChance);
            float randomValue = Random.Range(0f, totalWeight);

            float currentWeight = 0f;
            foreach (var set in validSets)
            {
                currentWeight += set.SpawnChance;
                if (randomValue <= currentWeight)
                {
                    return set;
                }
            }

            return validSets.Last();
        }


        public ModifierSetConfig GetSetByName(string setName)
        {
            return _modifierSets.FirstOrDefault(s => s != null && s.SetName == setName);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _modifierSets.RemoveAll(s => s == null);

            var duplicates = _modifierSets
                .GroupBy(s => s.SetName)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            if (duplicates.Any())
            {
                Debug.LogWarning($"[ModifierDatabase] Знайдено дублікати назв сетів: {string.Join(", ", duplicates)}");
            }
        }
#endif
    }
}

