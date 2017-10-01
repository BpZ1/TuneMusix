using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;

namespace TuneMusix.ViewModel.Effects
{
    class AddEffectsViewModel : IDragSource
    {
        private bool isDragging = false;

        bool IDragSource.CanStartDrag(IDragInfo dragInfo)
        {
            if (dragInfo != null) return true;
            else return false;
        }

        void IDragSource.DragCancelled()
        {
            isDragging = false;
        }

        void IDragSource.Dropped(IDropInfo dropInfo)
        {
            isDragging = false;
        }

        void IDragSource.StartDrag(IDragInfo dragInfo)
        {
            isDragging = true;
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
