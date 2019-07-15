using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows;
using System.Windows.Controls;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer;
using TuneMusix.Helpers.MediaPlayer.Effects;

namespace TuneMusix.ViewModel.Effects
{
    class AddEffectsViewModel : IDragSource
    {
        private AudioControls _audioControls = AudioControls.Instance;
        public RelayCommand AddEffect { get; set; }

        public AddEffectsViewModel()
        {
            AddEffect = new RelayCommand(_addEffect);
        }

        private void _addEffect(object argument)
        {
            string effectType = argument as string;
            switch (effectType)
            {
                case "Distortion":
                    _audioControls.EffectQueue.ChangeEffectListPosition(new DistortionEffect(), _audioControls.EffectQueue.Count);
                    break;
                case "Chorus":
                    _audioControls.EffectQueue.ChangeEffectListPosition(new ChorusEffect(), _audioControls.EffectQueue.Count);
                    break;
                case "Compressor":
                    _audioControls.EffectQueue.ChangeEffectListPosition(new CompressorEffect(), _audioControls.EffectQueue.Count);
                    break;
                case "Echo":
                    _audioControls.EffectQueue.ChangeEffectListPosition(new EchoEffect(), _audioControls.EffectQueue.Count);
                    break;
                case "Equalizer":
                    _audioControls.EffectQueue.ChangeEffectListPosition(new EqualizerEffect(), _audioControls.EffectQueue.Count);
                    break;
                case "Gargle":
                    _audioControls.EffectQueue.ChangeEffectListPosition(new GargleEffect(), _audioControls.EffectQueue.Count);
                    break;
                case "Reverb":
                    _audioControls.EffectQueue.ChangeEffectListPosition(new ReverbEffect(), _audioControls.EffectQueue.Count);
                    break;
                case "Flanger":
                    _audioControls.EffectQueue.ChangeEffectListPosition(new FlangerEffect(), _audioControls.EffectQueue.Count);
                    break;

            }          
        }

        bool IDragSource.CanStartDrag(IDragInfo dragInfo)
        {
            if (dragInfo != null) return true;
            else return false;
        }

        void IDragSource.DragCancelled()
        {

        }

        void IDragSource.Dropped(IDropInfo dropInfo)
        {

        }

        void IDragSource.StartDrag(IDragInfo dragInfo)
        {
            ListViewItem item = dragInfo.SourceItem as ListViewItem;
            if(item != null)
            {
                if (item.Name.Equals("DistortionItem"))
                {
                    dragInfo.Data = new DistortionEffect();
                }
                if (item.Name.Equals("ChorusItem"))
                {
                    dragInfo.Data = new ChorusEffect();
                }
                if (item.Name.Equals("FlangerItem"))
                {
                    dragInfo.Data = new FlangerEffect();
                }
                if (item.Name.Equals("Equalizer10Item"))
                {
                    dragInfo.Data = new EqualizerEffect();
                }
                if (item.Name.Equals("ReverbItem"))
                {
                    dragInfo.Data = new ReverbEffect();
                }
                if (item.Name.Equals("CompressorItem"))
                {
                    dragInfo.Data = new CompressorEffect();
                }
                if (item.Name.Equals("GargleItem"))
                {
                    dragInfo.Data = new GargleEffect();
                }
                if (item.Name.Equals("EchoItem"))
                {
                    dragInfo.Data = new EchoEffect();
                }
            }
            
            dragInfo.Effects = DragDropEffects.Copy;        
        }

        bool IDragSource.TryCatchOccurredException(Exception exception)
        {
            Logger.LogException(exception);
            throw exception;
        }
    }
}
