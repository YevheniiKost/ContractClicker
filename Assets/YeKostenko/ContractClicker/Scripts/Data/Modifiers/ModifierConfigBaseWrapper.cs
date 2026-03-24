using System;

using UnityEngine;

namespace YeKostenko.ContractClicker.Data.Modifiers
{
    [Serializable]
    public class ModifierConfigBaseWrapper
    {
        [SerializeReference]
        private ModifierConfigBase _config;

        public ModifierConfigBase Config => _config;

        public ModifierConfigBaseWrapper()
        {
        }

        public ModifierConfigBaseWrapper(ModifierConfigBase config)
        {
            _config = config;
        }

        public bool HasConfig => _config != null;

        public void SetConfig(ModifierConfigBase config)
        {
            _config = config;
        }

        public void Clear()
        {
            _config = null;
        }
    }
}