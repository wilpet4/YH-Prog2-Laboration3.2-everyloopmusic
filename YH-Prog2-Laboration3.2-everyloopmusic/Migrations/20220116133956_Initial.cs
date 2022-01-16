using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YH_Prog2_Laboration3._2_everyloopmusic.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "music");

            migrationBuilder.CreateTable(
                name: "artists",
                schema: "music",
                columns: table => new
                {
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artists", x => x.ArtistId);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                schema: "music",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "media_types",
                schema: "music",
                columns: table => new
                {
                    MediaTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_types", x => x.MediaTypeId);
                });

            migrationBuilder.CreateTable(
                name: "playlists",
                schema: "music",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlists", x => x.PlaylistId);
                });

            migrationBuilder.CreateTable(
                name: "albums",
                schema: "music",
                columns: table => new
                {
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albums", x => x.AlbumId);
                    table.ForeignKey(
                        name: "FK_albums_artists",
                        column: x => x.ArtistId,
                        principalSchema: "music",
                        principalTable: "artists",
                        principalColumn: "ArtistId");
                });

            migrationBuilder.CreateTable(
                name: "tracks",
                schema: "music",
                columns: table => new
                {
                    TrackId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AlbumId = table.Column<int>(type: "int", nullable: true),
                    MediaTypeId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: true),
                    Composer = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: true),
                    Milliseconds = table.Column<int>(type: "int", nullable: false),
                    Bytes = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tracks", x => x.TrackId);
                    table.ForeignKey(
                        name: "FK_tracks_albums",
                        column: x => x.AlbumId,
                        principalSchema: "music",
                        principalTable: "albums",
                        principalColumn: "AlbumId");
                    table.ForeignKey(
                        name: "FK_tracks_genres",
                        column: x => x.GenreId,
                        principalSchema: "music",
                        principalTable: "genres",
                        principalColumn: "GenreId");
                    table.ForeignKey(
                        name: "FK_tracks_media_types",
                        column: x => x.MediaTypeId,
                        principalSchema: "music",
                        principalTable: "media_types",
                        principalColumn: "MediaTypeId");
                });

            migrationBuilder.CreateTable(
                name: "playlist_track",
                schema: "music",
                columns: table => new
                {
                    PlaylistTrackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaylistId = table.Column<int>(type: "int", nullable: false),
                    TrackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlist_track", x => x.PlaylistTrackId);
                    table.ForeignKey(
                        name: "FK_playlist_track_playlists",
                        column: x => x.PlaylistId,
                        principalSchema: "music",
                        principalTable: "playlists",
                        principalColumn: "PlaylistId");
                    table.ForeignKey(
                        name: "FK_playlist_track_tracks",
                        column: x => x.TrackId,
                        principalSchema: "music",
                        principalTable: "tracks",
                        principalColumn: "TrackId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_albums_ArtistId",
                schema: "music",
                table: "albums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_playlist_track_PlaylistId",
                schema: "music",
                table: "playlist_track",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_playlist_track_PlaylistTrackId",
                schema: "music",
                table: "playlist_track",
                column: "PlaylistTrackId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_playlist_track_TrackId",
                schema: "music",
                table: "playlist_track",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_tracks_AlbumId",
                schema: "music",
                table: "tracks",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_tracks_GenreId",
                schema: "music",
                table: "tracks",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_tracks_MediaTypeId",
                schema: "music",
                table: "tracks",
                column: "MediaTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "playlist_track",
                schema: "music");

            migrationBuilder.DropTable(
                name: "playlists",
                schema: "music");

            migrationBuilder.DropTable(
                name: "tracks",
                schema: "music");

            migrationBuilder.DropTable(
                name: "albums",
                schema: "music");

            migrationBuilder.DropTable(
                name: "genres",
                schema: "music");

            migrationBuilder.DropTable(
                name: "media_types",
                schema: "music");

            migrationBuilder.DropTable(
                name: "artists",
                schema: "music");
        }
    }
}
