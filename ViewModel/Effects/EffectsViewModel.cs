using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.Effects
{
    class EffectsViewModel : INotifyPropertyChanged,IDragSource,IDropTarget
    {
        public BaseEffect SelectedItem { get; set; }
        private Options options = Options.Instance;
        private DataModel dataModel = DataModel.Instance;
        public RelayCommand RemoveEffect { get; set; }

        public ObservableCollection<BaseEffect> Effectlist
        {
            get { return dataModel.EffectQueue; }
        }

        public EffectsViewModel()
        {
            RemoveEffect = new RelayCommand(removeEffect);
            dataModel.EffectQueueChanged += OnEffectQueueChanged;
        }

        private void OnEffectQueueChanged(object source,object queue)
        {
            RaisePropertyChanged("EffectList");
        }

        private void removeEffect(object argument)
        {
            dataModel.RemoveEffectFromQueue(SelectedItem);
            options.Modified = true;
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
            if (dragInfo != null) return true;
            else return false;
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
                dataModel.ChangeEffectListPosition(effect, dropInfo.UnfilteredInsertIndex);
            }
            options.Modified = true;
        }
        #endregion
    }
}
