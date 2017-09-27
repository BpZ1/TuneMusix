using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public abstract class BaseEffect
    {
        private bool _isActive = true;
        private bool _isModified = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                _isModified = true;
                OnEffectActivated();
            }
        }
        public bool IsModified
        {
            get { return _isModified; }
            set
            {
                _isModified = value;
            }
        }

        public abstract IWaveSource Apply(IWaveSource waveSource);

        public delegate void EffectChangedEventHandler(object source);
        public event EffectChangedEventHandler EffectActivated;

        protected virtual void OnEffectActivated()
        {
            if(EffectActivated != null)
            {
                EffectActivated(this);
            }
        }
    }
}
