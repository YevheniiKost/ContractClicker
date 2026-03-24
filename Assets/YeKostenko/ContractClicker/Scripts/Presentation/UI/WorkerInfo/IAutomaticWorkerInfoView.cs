using System;

namespace YeKostenko.ContractClicker.Presentation.UI.WorkerInfo
{
    public interface IAutomaticWorkerInfoView
    {
        event Action Create;
        event Action UpgradeClick;
        event Action CloseClick;

        void SetUpgradeButtonInteractable(bool interactable);
        void SetUpgradeCost(int cost);
        void SetLevel(int level);
        void SetStats(float yieldPerTick, float tickInterval, float critChance, float critMultiplier);
        void SetNextLevelStats(float yieldPerTick, float tickInterval, float critChance, float critMultiplier);
        void Close();
    }
}

