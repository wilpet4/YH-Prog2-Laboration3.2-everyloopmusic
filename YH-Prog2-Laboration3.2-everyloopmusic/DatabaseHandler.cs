using Microsoft.Extensions.Configuration;
using YH_Prog2_Laboration3._2_everyloopmusic.Models;

class DatabaseHandler
{
    public string OutputAllTracks(in MusicContext context)
    {
        List<string> listOfTracks = new List<string>();
        var query = from track in context.Tracks
                    join album in context.Albums on track.AlbumId equals album.AlbumId
                    join artist in context.Artists on album.ArtistId equals artist.ArtistId
                    orderby track.TrackId
                    select new {track.TrackId, track.Name, Artist = artist.Name};
        query.ToList();
        foreach (var item in query)
        {
            listOfTracks.Add($"[{item.TrackId}] {item.Name} ## {item.Artist}");
        }
        string result = String.Join("\n", listOfTracks);
        return result;
    }
    public List<string> FormatPlaylistSelection(in MusicContext context)
    {
        List<string> listOfPlaylist = new List<string>();
        foreach (var item in context.Playlists)
        {
            listOfPlaylist.Add(String.Concat($"[{item.PlaylistId}] ", item.Name));
        }
        return listOfPlaylist;
    }   
    public Playlist? FindPlaylist(in MusicContext context, in short idSearch)
    {
        foreach (var item in context.Playlists)
        {
            if (item.PlaylistId == idSearch)
            {
                return item;
            }
        }
        return null;
    }
    public string FormatTrackSelection(in MusicContext context, in Playlist playlist)
    {
        List<string> listofTracks = new List<string>();
        int playlistID = playlist.PlaylistId;
        int index = 1;
        var query = from pt in context.PlaylistTracks
                    join p in context.Playlists on pt.PlaylistId equals p.PlaylistId
                    join t in context.Tracks on pt.TrackId equals t.TrackId
                    join album in context.Albums on t.AlbumId equals album.AlbumId
                    join artist in context.Artists on album.ArtistId equals artist.ArtistId
                    orderby t.Name
                    where p.PlaylistId == playlistID
                    select new { t.Name, Artist = artist.Name, Album = album.Title };
        foreach (var item in query)
        {
            listofTracks.Add($"[{index}] {item.Name} # {item.Artist} # {item.Album}");
            index++;
        }
        string result = String.Join("\n", listofTracks);
        return result;
    }
    public List<PlaylistTrack> CountTracksInPlaylist(in MusicContext context, in Playlist playlist)
    {
        int playlistID = playlist.PlaylistId;
        var query = (from pt in context.PlaylistTracks
                     join t in context.Tracks on pt.TrackId equals t.TrackId
                     where pt.PlaylistId == playlistID
                     orderby t.Name
                     select pt).ToList();
        return query;
    }
    public void RemoveTrackFromPlaylist(in MusicContext context, in PlaylistTrack trackToRemove)
    {
        Console.WriteLine($"Track nr: {trackToRemove.TrackId} is being removed...");
        context.PlaylistTracks.Remove(trackToRemove);
        context.SaveChanges();
    }
    public void AddTrackToPlaylist(in MusicContext context, in Playlist playlist, short trackID)
    {
        Console.WriteLine($"Track nr: {trackID} is being added...");
        var query = context.Tracks.Where(t => t.TrackId == trackID).First();
        if (query == null)
        {
            ErrorMessage();
        }
        else
        {
            var newRow = new PlaylistTrack { PlaylistId = playlist.PlaylistId, TrackId = query.TrackId };
            context.PlaylistTracks.Add(newRow);
            context.SaveChanges();
        }
    }
    public void AddNewPlaylist(in MusicContext context, in string playlistName)
    {
        Console.WriteLine($"Playlist [{playlistName}] is being added...");
        var newPlaylist = new Playlist { Name = playlistName };
        context.Playlists.Add(newPlaylist);
        context.SaveChanges();
    }
    public void ErrorMessage()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Fel vid inmatning, försök igen!");
        Console.ReadKey();
        Console.ForegroundColor= ConsoleColor.White;
    }
    public string GetConnectionString()
    {
        var builder = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.json", true, true);
        string connectionString =
        builder.Build().GetConnectionString("DefaultConnection");
        return connectionString;
    }
}