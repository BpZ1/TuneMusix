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
        private bool isActive = true;
        private bool isModified = false;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                isModified = true;
                OnEffectActivated();
            }
        }
        public bool IsModified
        {
            get { return isModified; }
            set
            {
                isModified = value;
            }
        }

        public abstract IWaveSource Apply(IWaveSource waveSource);

        public delegate void EffectChangedEventHandler();
        public event EffectChangedEventHandler EffectActivated;

        protected virtual void OnEffectActivated()
        {
            if(EffectActivated != null)
            {
                EffectActivated();
            }
        }
    }
}
