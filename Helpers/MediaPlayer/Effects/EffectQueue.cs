using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{ 
    public class EffectQueue
    {
        private bool modified;
        private LinkedList<Func<IWaveSource,IWaveSource>> queue;
        private ObservableCollection<BaseEffect> effectlist;

        public EffectQueue()
        {
            queue = new LinkedList<Func<IWaveSource, IWaveSource>>();
            effectlist = new ObservableCollection<BaseEffect>();
        }

        public void Add(BaseEffect effect)
        {
            if (effect == null)
                throw new ArgumentNullException("Can't add null to to effectlist!");

            effectlist.Add(effect);
            modified = true;
            effect.EffectActivated += OnQueueChanged;
            OnQueueChanged();
        }

        public bool Remove(BaseEffect effect)
        {
            if (effect == null)
                throw new ArgumentNullException("Can't remove null from effectlist");

            bool success;
            effectlist.Remove(effect);
            Func<IWaveSource, IWaveSource> func = effect.Apply;
            success = queue.Remove(func);
            modified = true;
            effect.EffectActivated -= OnQueueChanged;
            OnQueueChanged();
            return success;
        }
       
        /// <summary>
        /// Adds an 10 band equalizer at the end of the queue.
        /// </summary>
        public void Add10BandEqualizer()
        {
            Func<IWaveSource, IWaveSource> func = create10BandEqualizer;
            queue.AddLast(func);
            modified = true;
            OnQueueChanged();
        }

        private IWaveSource create10BandEqualizer(IWaveSource waveSource)
        {
            return waveSource
                .ToSampleSource()
                .AppendSource(Equalizer.Create10BandEqualizer)
                .ToWaveSource();
        }

        /// <summary>
        /// Removes the last element of the Effectqueue if the queue has at least one element.
        /// </summary>
        public void RemoveLast()
        {
            if(queue.Count > 0)
            {
                queue.RemoveLast();
                modified = true;
                OnQueueChanged();
            }           
        }

        public int Count
        {
            get { return queue.Count; }
        }

        /// <summary>
        /// Applies all effects that are contained in the queue to the given IWavesource and returns it.
        /// </summary>
        /// <param name="waveSource"></param>
        /// <returns></returns>
        public IWaveSource Apply(IWaveSource waveSource)
        {
            foreach (Func<IWaveSource,IWaveSource> effect in queue)
            {
                waveSource = effect(waveSource);
            }
            return waveSource;
        }

        public delegate void EffectQueueEventHandler(object changed);
        public event EffectQueueEventHandler QueueChanged;

        protected virtual void OnQueueChanged()
        {
            queue = new LinkedList<Func<IWaveSource, IWaveSource>>();
            foreach(BaseEffect effect in effectlist)
            {
                Func<IWaveSource, IWaveSource> func = effect.Apply;
                queue.AddLast(func);
            }

            if (QueueChanged != null)
            {
                QueueChanged(queue);
            }
        }

        public bool IsModified
        {
            get { return modified; }
            set
            {
                modified = value;
            }
        }

        public ObservableCollection<BaseEffect> Effectlist
        {
            get { return effectlist; }
            set { effectlist = value; }
        }

        public void Clear()
        {
            effectlist.Clear();
            queue.Clear();
            modified = true;
            OnQueueChanged();
        }

        /// <summary>
        /// Changed the position of a given effect in the effectqueue
        /// if it is already contained, or inserts a new one if it is not.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="position"></param>
        public void ChangeEffectListPosition(BaseEffect effect, int position)
        {
            if (effectlist.Contains(effect)) //Effect is already contained in the list.
            {
                int pos1 = effectlist.IndexOf(effect);
                Logger.Log("Moved effect from queue position " + pos1 + " to position " + position + ".");
                if (position == effectlist.Count) //If the new position is at the end of the list
                {
                    effectlist.Move(pos1, position - 1);
                }
                else
                {
                    effectlist.Move(pos1, position);
                }
            }
            else //Effect is a new effect.
            {
                Logger.Log("Added effect on queue position " + position + ".");
                effectlist.Insert(position, effect);
                effect.EffectActivated += OnQueueChanged;
                modified = true;
            }
            OnQueueChanged();
        }
    }
}
