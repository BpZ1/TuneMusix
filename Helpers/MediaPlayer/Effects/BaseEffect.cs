using CSCore;
using System.Collections.Generic;
using System.Linq;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public abstract class BaseEffect
    {
        private bool _isActive = true;
        public EffectType EffectType { get; private set; }
        public int QueueIndex { get; set; }
        public bool IsModified { get; set; }

        public static int MaxValues = 10;

        public BaseEffect( EffectType effectType )
        {
            EffectType = effectType;
        }
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

        public abstract IWaveSource Apply( IWaveSource waveSource );

        public abstract IEnumerable<string> GetValues();

        public abstract void SetValues( IEnumerable<string> values );

        public delegate void EffectChangedEventHandler();

        public event EffectChangedEventHandler EffectActivated;

        protected virtual void OnEffectActivated()
        {
            EffectActivated?.Invoke();
        }

        protected int GetIntValue( string value )
        {
            if ( int.TryParse( value, out var result ) )
            {
                return result;
            }
            return 0;
        }

        protected double GetDoubleValue( string value )
        {
            if ( double.TryParse( value, out var result ) )
            {
                return result;
            }
            return 0;
        }

        protected float GetFloatValue( string value )
        {
            if ( float.TryParse( value, out var result ) )
            {
                return result;
            }
            return 0;
        }

        protected bool GetBoolValue( string value )
        {
            if ( bool.TryParse( value, out var result ) )
            {
                return result;
            }
            return false;
        }

        public override bool Equals( object obj )
        {
            if ( !( obj is BaseEffect effect ) )
            {
                return false;
            }
            if ( EffectType != effect.EffectType )
            {
                return false;
            }
            var ownValues = GetValues();
            var otherValues = effect.GetValues();
            if ( ownValues.Count() != otherValues.Count() )
            {
                return false;
            }
            for ( var i = 0; i < ownValues.Count(); i++ )
            {
                if ( !otherValues.ElementAt( i ).Equals( ownValues.ElementAt( i ) ) )
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = -1106141115;
            foreach ( var value in GetValues() )
            {
                hashCode = hashCode * -1521134295 + value.GetHashCode();
            }
            hashCode = hashCode * -1521134295 + _isActive.GetHashCode();
            hashCode = hashCode * -1521134295 + EffectType.GetHashCode();
            hashCode = hashCode * -1521134295 + QueueIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + IsModified.GetHashCode();
            hashCode = hashCode * -1521134295 + IsActive.GetHashCode();
            return hashCode;
        }
    }
}
