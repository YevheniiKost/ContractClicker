using System.Collections.Generic;

using UnityEngine;

namespace YeKostenko.ContractClicker.Data.Boosters
{
    [CreateAssetMenu(fileName = "BoosterConfig", menuName = "ContractClicker/Boosters/Booster Config")]
    public class BoosterConfig : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField]
        private string _boosterId;

        [SerializeField]
        private string _displayName;

        [SerializeField]
        [TextArea(2, 4)]
        private string _description;

        [SerializeField]
        private Sprite _icon;

        [Header("Classification")]
        [SerializeField]
        private BoosterRarity _rarity;

        [Header("Purchase")]
        [SerializeField]
        [Min(0)]
        private int _price;

        [Header("Effect")]
        [SerializeField]
        [Min(1f)]
        private float _durationInSeconds = 60f;

        [SerializeField]
        private BoosterStatModificationData[] _statModifications;

        [Header("Visual")]
        [SerializeField]
        private Color _rarityColor = Color.white;

        public string BoosterId => _boosterId;
        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public BoosterRarity Rarity => _rarity;
        public int Price => _price;
        public float DurationInSeconds => _durationInSeconds;
        public IReadOnlyList<BoosterStatModificationData> StatModifications => _statModifications;
        public Color RarityColor => _rarityColor;
    }
}

