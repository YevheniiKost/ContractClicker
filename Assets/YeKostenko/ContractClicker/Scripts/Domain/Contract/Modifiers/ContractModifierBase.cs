namespace YeKostenko.ContractClicker.Domain.Contract.Modifiers
{
    public abstract class ContractModifierBase : IContractModifier
    {
        private static int _idCounter = 0;

        public virtual ModifierType Type { get; protected set; }
        public virtual int Priority => (int)Type;
        public virtual bool IsActive { get; protected set; } = true;
        public string Id { get; }

        protected IContract AttachedContract { get; private set; }

        protected ContractModifierBase()
        {
            Id = $"{GetType().Name}_{_idCounter++}";
        }

        protected ContractModifierBase(string customId)
        {
            Id = customId;
        }

        public virtual void OnApply(IContract contract)
        {
            AttachedContract = contract;
        }

        public virtual void OnRemove(IContract contract)
        {
            AttachedContract = null;
        }

        public abstract ModifierResult Process(ModifierContext context);

        public virtual void Update(float deltaTime)
        {

        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Active: {IsActive}, Priority: {Priority}]";
        }
    }
}

