using System;

namespace DiLite.Builders
{
    public abstract class BuilderBase<T>
    {
        protected bool IsInBuildingStage { get; set; } = true;

        public T Build()
        {
            CheckState();
            IsInBuildingStage = false;

            return BuildInternal();
        }

        protected void CheckState()
        {
            if (!IsInBuildingStage)
            {
                throw new InvalidOperationException("The Builder is no longer in the configuration stage (Build was called).");
            }
        }

        protected abstract T BuildInternal();
    }
}