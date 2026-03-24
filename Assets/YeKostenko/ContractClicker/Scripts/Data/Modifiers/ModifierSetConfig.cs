using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YeKostenko.ContractClicker.Data.Game;

namespace YeKostenko.ContractClicker.Data.Modifiers
{
    /// <summary>
    /// Набір (пакет) модифікаторів що застосовуються разом
    /// </summary>
    [CreateAssetMenu(fileName = "ModifierSet", menuName = "ContractClicker/Modifiers/Modifier Set", order = 1)]
    public class ModifierSetConfig : ScriptableObject
    {
        [Header("Базова інформація")]
        [SerializeField]
        private string _setName;
        [SerializeField]
        [TextArea(3, 6)]
        private string _setDescription;
        [SerializeField]
        private ContractDifficulty _difficulty;

        [SerializeField]
        [Tooltip("Мінімальний рівень контракту для застосування")]
        [Min(1)]
        private int _minContractLevel = 1;
        [SerializeField]
        [Range(0f, 100f)]
        [Tooltip("Шанс появи цього сету (0-100%)")]
        private float _spawnChance = 50f;

        [Header("Модифікатори прогресу")]
        [SerializeField]
        private ModifierConfigBaseWrapper[] _progressModifiers;

        [Header("Візуальне відображення")]
        [SerializeField]
        private Color _difficultyColor = Color.white;
        [SerializeField]
        private Sprite _icon;

        // Properties
        public string SetName => _setName;
        public string SetDescription => _setDescription;
        public ContractDifficulty Difficulty => _difficulty;
        public int MinContractLevel => _minContractLevel;
        public float SpawnChance => _spawnChance;
        public Color DifficultyColor => _difficultyColor;
        public Sprite Icon => _icon;


        /// <summary>
        /// Отримати всі активні конфіги модифікаторів
        /// </summary>
        public IEnumerable<ModifierConfigBase> GetActiveModifierConfigs()
        {
            List<ModifierConfigBase> configs = new List<ModifierConfigBase>();

            if (_progressModifiers != null)
            {
                foreach (ModifierConfigBaseWrapper mod in _progressModifiers)
                {
                    if (mod?.Config != null)
                    {
                        configs.Add(mod.Config);
                    }
                }
            }

            return configs;
        }

        /// <summary>
        /// Чи підходить цей сет для заданого рівня контракту
        /// </summary>
        public bool IsValidForLevel(int contractLevel)
        {
            return contractLevel >= _minContractLevel;
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            // Автоматичне встановлення кольору за складністю
            if (_difficultyColor == Color.white)
            {
                _difficultyColor = _difficulty switch
                {
                    ContractDifficulty.Easy => new Color(0.6f, 0.9f, 0.6f), // Світло-зелений
                    ContractDifficulty.Medium => new Color(0.6f, 0.8f, 1f), // Синій
                    ContractDifficulty.Hard => new Color(0.9f, 0.3f, 0.3f), // Червоний
                    _ => Color.white
                };
            }
        }
#endif
    }
}

