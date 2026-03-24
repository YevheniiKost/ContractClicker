using System;

using UnityEngine;

namespace YeKostenko.ContractClicker.Data.Game
{
    [Serializable]
    public class StatModificationConfigSerializable
    {
        [SerializeField]
        private StatId _statId;
        [SerializeField]
        private ModificationOperation _operation;
        [SerializeField]
        private float _value;
        [SerializeField]
        private int _levelInterval;
        [SerializeField]
        private int _levelCap;

        public StatModificationConfig ToStatModificationConfig()
        {
            return new StatModificationConfig(_statId, _operation, _value, _levelInterval, _levelCap);
        }
    }
}