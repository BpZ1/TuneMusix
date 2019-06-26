using CSCore;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public abstract class BaseEffect
    {
        private bool _isActive = true;
        public bool IsModified { get; set; } = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                IsModified = true;
                OnEffectActivated();
            }
        }

        public abstract IWaveSource Apply(IWaveSource waveSource);

        public delegate void EffectChangedEventHandler();
        public event EffectChangedEventHandler EffectActivated;

        protected virtual void OnEffectActivated()
        {
            EffectActivated?.Invoke();
        }
    }
}
