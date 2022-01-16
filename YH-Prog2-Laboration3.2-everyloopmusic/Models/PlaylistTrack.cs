using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YH_Prog2_Laboration3._2_everyloopmusic.Models
{
    [Table("playlist_track", Schema = "music")]
    [Index(nameof(PlaylistTrackId), Name = "IX_playlist_track_PlaylistTrackId", IsUnique = true)]
    public partial class PlaylistTrack
    {
        public int PlaylistId { get; set; }
        public int TrackId { get; set; }
        [Key]
        public int PlaylistTrackId { get; set; }

        [ForeignKey(nameof(PlaylistId))]
        [InverseProperty("PlaylistTracks")]
        public virtual Playlist Playlist { get; set; } = null!;
        [ForeignKey(nameof(TrackId))]
        [InverseProperty("PlaylistTracks")]
        public virtual Track Track { get; set; } = null!;
    }
}
