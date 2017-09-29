using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;

namespace TuneMusix.ViewModel.Effects
{
    class EffectsViewModel : INotifyPropertyChanged
    {
        public BaseEffect SelectedItem { get; set; }

        private DataModel dataModel = DataModel.Instance;
        public RelayCommand AddEffect { get; set; }
        public RelayCommand RemoveEffect { get; set; }

        public ObservableCollection<BaseEffect> Effectlist
        {
            get { return dataModel.EffectQueue; }
        }

        

        public EffectsViewModel()
        {
            AddEffect = new RelayCommand(_addEffect);
            RemoveEffect = new RelayCommand(_removeEffect);

            dataModel.EffectQueueChanged += OnEffectQueueChanged;
        }

        private void OnEffectQueueChanged(object source,object queue)
        {
            RaisePropertyChanged("EffectList");
        }

        private void _addEffect(object argument)
        {
            dataModel.AddEffectToQueue(argument as BaseEffect);
        }
        private void _removeEffect(object argument)
        {
            dataModel.RemoveEffectFromQueue(SelectedItem);
        }

        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        #region notifyInterface
        public event PropertyChangedEventHandler PropertyChanged;

        bool? _CloseWindowFlag;
        public bool? CloseWindowFlag
        {
            get { return _CloseWindowFlag; }
            set
            {
                _CloseWindowFlag = value;
                RaisePropertyChanged("CloseWindowFlag");
            }
        }

        public virtual void CloseWindow(bool? result = true)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                CloseWindowFlag = CloseWindowFlag == null
                    ? true
                    : !CloseWindowFlag;
            }));
        }
        #endregion
    }
}
