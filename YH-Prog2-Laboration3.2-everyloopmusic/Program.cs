﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using YH_Prog2_Laboration3._2_everyloopmusic.Models;

MusicContext context = new MusicContext(); // kanske inte idiotiskt
DatabaseHandler handler = new DatabaseHandler();

// TESTER, TA BORT INNAN INLÄMNING
var artistsWithThe = context.Artists.Where(a => a.Name.Contains("The "));
var qq = context.PlaylistTracks.Where(i => i.PlaylistId < 2).ToList();
foreach (var item in artistsWithThe) // TEST
{
    Console.WriteLine(item.Name);
}
Console.ReadKey();
//

bool isRunning = true;
while (isRunning)
{
    Console.Clear();
    Console.WriteLine("Vänligen mata in ett knappval.\n" +
                      "[1] Visa alla befintliga spellistor\n" +
                      "[2] Visa alla låtar\n" +
                      "[3] Skapa ny spellista");
    if (Int16.TryParse(Console.ReadLine(), out short menuInput))
    {
        switch (menuInput)
        {
            case 1:
                Console.Clear();
                List<string> currentPlaylists = handler.FormatPlaylistSelection(context);
                for (int i = 0; i < currentPlaylists.Count; i++)
                {
                    Console.WriteLine(currentPlaylists[i]);
                }
                Console.Write("\n Om du vill ändra i en spellista, vänligen mata in spellistans ID: ");
                if (Int16.TryParse(Console.ReadLine(), out short idSearch))
                {
                    Playlist? selectedPlaylist = handler.FindPlaylist(context, idSearch); // Hämtar den valda raden i Playlist-tabellen.
                    if (selectedPlaylist != null)
                    {
                        List<PlaylistTrack> tracksInPlaylist = handler.CountTracksInPlaylist(context, selectedPlaylist);
                        Console.Clear();
                        Console.WriteLine(selectedPlaylist.Name);
                        Console.WriteLine($"[1] Lägg till låt i spellistan\n" + // NÄSTAN KLAR
                                          $"[2] Ta bort låt i spellistan\n" +   // KLAR
                                          $"[3] Ändra namn på spellistan\n" +   // KLAR
                                          $"[4] Radera spellistan");            // KLAR
                        if (Int16.TryParse(Console.ReadLine(), out short playlistMenuInput))
                        {
                            switch (playlistMenuInput)
                            {
                                case 1:
                                    // TODO: Funktion för att söka och lägga till låt i spellistan.
                                    Console.Clear();
                                    Console.WriteLine(handler.OutputAllTracks(context));
                                    Console.Write("Vänligen mata in ID på den låt du vill ta bort: ");
                                    if (Int16.TryParse(Console.ReadLine(),out menuInput ) && menuInput >= 1 && menuInput <= context.Tracks.Count())
                                    {
                                        handler.AddTrackToPlaylist(context, selectedPlaylist, menuInput); 
                                    }
                                    else
                                    {
                                        handler.ErrorMessage();
                                    }
                                    break;
                                case 2:
                                    Console.Clear();
                                    Console.WriteLine(handler.FormatTrackSelection(context, selectedPlaylist));
                                    Console.Write("Vänligen mata in ID på den låt du vill ta bort: ");
                                    if (Int16.TryParse(Console.ReadLine(), out short trackToRemove) && trackToRemove <= tracksInPlaylist.Count() && trackToRemove > 0)
                                    { 
                                        handler.RemoveTrackFromPlaylist(context, tracksInPlaylist[trackToRemove - 1]); // Borde fungera.
                                    }
                                    else
                                    {
                                        handler.ErrorMessage();
                                        Console.ReadKey();
                                    }
                                    break;
                                case 3:
                                    Console.Clear();
                                    Console.WriteLine(selectedPlaylist.Name);
                                    Console.Write("Vänligen mata in det nya namnet på spellistan: ");
                                    string newName = Console.ReadLine();
                                    if (newName.Count() < 120) // Hårdkodade Max Length av kolumnen. Inte så bra men det verkade krångligare än jag trodde
                                    {                          // att läsa Max Length av en kolumn i databasen.
                                        selectedPlaylist.Name = newName;
                                        context.Update(selectedPlaylist);
                                        Console.WriteLine("Spellistan har nu döpts om.");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Det nya namnet överskrider maxlängden. Försök igen!");
                                        Console.ReadKey();
                                    }
                                    break;
                                case 4:
                                    Console.WriteLine($"Är du säker att du vill radera [{selectedPlaylist.Name}]? Du kommer inte kunna ångra detta.\n" +
                                                      $"[1] Radera spellistan\n" +
                                                      $"[2] Avbryt");
                                    if (Int16.TryParse(Console.ReadLine(), out short input) && input == 1)
                                    {
                                        Console.WriteLine($"{selectedPlaylist.Name} har nu raderats.");
                                        int playlistIndex = selectedPlaylist.PlaylistId;
                                        var query = from pt in context.PlaylistTracks
                                                    where pt.PlaylistId == playlistIndex
                                                    select pt;
                                        query.ToList().ForEach(pt => context.Remove(pt));
                                        context.Remove(selectedPlaylist);
                                        context.SaveChanges();
                                        Console.ReadKey();
                                    }
                                    break;
                                default:
                                    handler.ErrorMessage();
                                    break;
                            }
                        }
                        else { handler.ErrorMessage(); }
                    }
                    else{ handler.ErrorMessage(); }
                }
                break;
            case 2:
                Console.Clear();
                Console.WriteLine(handler.OutputAllTracks(context));
                Console.ReadKey();
                break;
            case 3:
                Console.Clear();
                Console.Write("Vänligen mata in namnet på din nya spellista: ");
                string newPlaylistName = Console.ReadLine();
                handler.AddNewPlaylist(context, newPlaylistName);
                var getNewPlaylist = from p in context.Playlists
                                     where p.Name == newPlaylistName
                                     select p;
                getNewPlaylist.ToList();
                foreach (var item in getNewPlaylist)
                {
                    Console.WriteLine($"{item.PlaylistId}, {item.Name}");
                }
                Console.ReadKey();
                break;
            default:
                break;
        }
    }
}