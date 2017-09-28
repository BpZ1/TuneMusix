using System.Collections.ObjectModel;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;

namespace TuneMusix.ViewModel.Effects
{
    class EffectsViewModel
    {

        public ObservableCollection<BaseEffect> EffectList;
        private DataModel dataModel = DataModel.Instance;

        public RelayCommand AddEffect { get; set; }

        public EffectsViewModel()
        {
            EffectList = dataModel.EffectQueue;


            AddEffect = new RelayCommand(_addEffect);
        }

        private void _addEffect(object argument)
        {
            dataModel.AddEffectToQueue(argument as BaseEffect);
        }

    }
}
