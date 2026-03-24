using System;

using UnityEngine;

namespace YeKostenko.ContractClicker.Presentation.UI
{
    public class View : MonoBehaviour
    {
        protected virtual void OnAwake()
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnDestroyView()
        {
        }

        protected virtual void OnValidateView()
        {
        }

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void OnDestroy()
        {
            OnDestroyView();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            OnValidateView();
        }
#endif
    }
}