using GongSolutions.Wpf.DragDrop;
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
    class EffectsViewModel : INotifyPropertyChanged,IDragSource,IDropTarget
    {
        public BaseEffect SelectedItem { get; set; }

        private DataModel dataModel = DataModel.Instance;
        public RelayCommand AddEffect { get; set; }
        public RelayCommand RemoveEffect { get; set; }

        public ObservableCollection<BaseEffect> Effectlist
        {
            get { return dataModel.EffectQueue; }
        }

        private bool isDragging = false;

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

        #region dragdrop
        public void StartDrag(IDragInfo dragInfo)
        {
            isDragging = true;

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
            isDragging = false;
        }

        public void DragCancelled()
        {
            isDragging = false;
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
            Console.WriteLine("Dropposition: " + dropInfo.InsertIndex);
            Console.WriteLine("Dropposition filtered: " + dropInfo.UnfilteredInsertIndex);
            BaseEffect effect = dropInfo.Data as BaseEffect;
            if (effect != null && dropInfo != null)
            {
                dataModel.ChangeEffectListPosition(effect, dropInfo.UnfilteredInsertIndex);
            }
          
        }
        #endregion
    }
}
