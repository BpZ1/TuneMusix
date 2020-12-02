using System.Collections.Generic;
using System.Linq;
using TuneMusix.Data.Database.Tables;
using TuneMusix.Model;

namespace TuneMusix.Data.Database
{
    public class FolderReconstructor
    {
        public FolderReconstructor( FolderTable folderTable )
        {
            _folderTable = folderTable;
        }

        public IEnumerable<Folder> CreateFolders( IEnumerable<Track> tracks )
        {
            var allFolders = _folderTable.SelectAll();
            foreach ( var track in tracks )
            {
                if ( !string.IsNullOrEmpty( track.FolderID.Value ) )
                {
                    var parentFolder = allFolders.Single( x => x.Id.Equals( track.FolderID.Value ) );
                    parentFolder.Add( track );
                }
            }
            foreach ( var folder in allFolders )
            {
                if ( !string.IsNullOrEmpty( folder.FolderId ) )
                {
                    var parentFolder = allFolders.Single( x => x.FolderId.Equals( folder.FolderId ) );
                    parentFolder.Add( folder );
                }
            }
            return allFolders.Where( x => string.IsNullOrEmpty( x.FolderId ) );
        }

        private FolderTable _folderTable;
    }
}
