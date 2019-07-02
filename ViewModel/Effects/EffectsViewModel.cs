using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.Effects
{
    class EffectsViewModel : INotifyPropertyChanged,IDragSource,IDropTarget
    {
        public BaseEffect SelectedItem { get; set; }
        private Options _options = Options.Instance;
        public RelayCommand RemoveEffect { get; set; }

        public ObservableCollection<BaseEffect> Effectlist => AudioControls.Instance.EffectQueue.Effectlist;

        public EffectsViewModel()
        {
            RemoveEffect = new RelayCommand(_removeEffect);
            AudioControls.Instance.EffectQueue.QueueChanged += OnEffectQueueChanged;
        }

        private void OnEffectQueueChanged(object source)
        {
            RaisePropertyChanged("Effectlist");
        }

        private void _removeEffect(object argument)
        {
            if(SelectedItem != null)
            {
                AudioControls.Instance.EffectQueue.Remove(SelectedItem);
                _options.Modified = true;
            }      
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

        #region dragdrop
        public void StartDrag(IDragInfo dragInfo)
        {
            dragInfo.Data = dragInfo.SourceItem;
            dragInfo.Effects = DragDropEffects.Copy;
        }

        public bool CanStartDrag(IDragInfo dragInfo)
        {
            if (dragInfo != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dropped(IDropInfo dropInfo)
        {
        }

        public void DragCancelled()
        {
        }

        public bool TryCatchOccurredException(Exception exception)
        {
            Logger.LogException(exception);
            throw exception;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.NotHandled = true;
            var sourceItem = dropInfo.Data;
            if (sourceItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            BaseEffect effect = dropInfo.Data as BaseEffect;
            if (effect != null && dropInfo != null)
            {
                AudioControls.Instance.
                    EffectQueue.ChangeEffectListPosition(effect, dropInfo.UnfilteredInsertIndex);
            }
            _options.Modified = true;
        }
        #endregion
    }
}
