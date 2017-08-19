﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Deezer.Api
{

	public interface IUser : IObjectWithImage
	{
		ulong Id { get; set; }
		string Name { get; set; }
		string Link { get; set; }
		string Country { get; set; }


		// ** Methods **
        //Favourites
		Task<IEnumerable<IAlbum>> GetFavouriteAlbums();
        Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aCount);
        Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart, uint aCount);

		Task<IEnumerable<IArtist>> GetFavouriteArtists();
        Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aCount);
        Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart, uint aCount);

		Task<IEnumerable<ITrack>> GetFavouriteTracks();
        Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart, uint aCount);


        Task<IEnumerable<ITrack>> GetPersonalTracks();
        Task<IEnumerable<ITrack>> GetPersonalTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> GetPlaylists();
        Task<IEnumerable<IPlaylist>> GetPlaylists(uint aCount);
        Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart, uint aCount);


		Task<IEnumerable<ITrack>> GetFlow();
        Task<IEnumerable<ITrack>> GetFlow(uint aCount);
        Task<IEnumerable<ITrack>> GetFlow(uint aStart, uint aCount);

		Task<IEnumerable<ITrack>> GetHistory();
        Task<IEnumerable<ITrack>> GetHistory(uint aCount);
        Task<IEnumerable<ITrack>> GetHistory(uint aStart, uint aCount);

        //Recommendations
		Task<IEnumerable<IAlbum>> GetRecommendedAlbums();
        Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aCount);
        Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart, uint aCount);

		Task<IEnumerable<IArtist>> GetRecommendedArtists();
        Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aCount);
        Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart, uint aCount);

		Task<IEnumerable<ITrack>> GetRecommendedTracks();
        Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart, uint aCount);

		Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists();
        Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aCount);
        Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart, uint aCount);

        Task<uint> CreatePlaylist(string title);
        Task<bool> AddToPlaylist(ulong playlistId, string songids);

        Task<bool> AddRadioToFavorite(ulong radioId);
        Task<bool> RemoveRadioFromFavorite(ulong radioId);

        Task<bool> AddTrackToFavorite(ulong trackId);
        Task<bool> RemoveTrackFromFavorite(ulong trackId);

        Task<bool> AddPlaylistToFavorite(ulong playlistId);
        Task<bool> RemovePlaylistFromFavorite(ulong playlistId);

        Task<bool> AddArtistToFavorite(ulong artistId);
        Task<bool> RemoveArtistFromFavorite(ulong artistId);

        Task<bool> AddAlbumToFavorite(ulong albumId);
        Task<bool> RemoveAlbumFromFavorite(ulong albumId);
    }

	internal class User : ObjectWithImage, IUser, IDeserializable<DeezerClient>
	{
		public ulong Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public string Country { get; set; }

		//Local Serailization info
		public DeezerClient Client { get; set; }
		public void Deserialize(DeezerClient aClient) { Client = aClient; }

		public override string ToString()
		{
			return Name;
		}

        //Internal wrapper around get for all user methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, DeezerPermissions aPermisisons, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClient>
        {
            if (!Client.IsAuthenticated) { throw new NotLoggedInException(); }
            if (!Client.HasPermission(aPermisisons)) { throw new DeezerPermissionsException(aPermisisons); }

            string method = string.Format("user/me/{0}", aMethod);
            return Client.Get<TSource>(method, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                return Client.Transform<TSource, TDest>(aTask.Result);
            }, Client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums() { return GetFavouriteAlbums(0, Client.ResultSize);  }
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aCount) { return GetFavouriteAlbums(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart, uint aCount) { return Get<Album, IAlbum>("albums", DeezerPermissions.BasicAccess, aStart, aCount); }

        public Task<IEnumerable<IArtist>> GetFavouriteArtists() { return GetFavouriteArtists(0, Client.ResultSize); }
        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aCount) {  return GetFavouriteArtists(0, aCount); }
        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart, uint aCount) { return Get<Artist, IArtist>("artists", DeezerPermissions.BasicAccess, aStart, aCount); }

        public Task<IEnumerable<ITrack>> GetFavouriteTracks() { return GetFavouriteTracks(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aCount) { return GetFavouriteTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart, uint aCount) { return Get<Track, ITrack>("tracks", DeezerPermissions.BasicAccess, aStart, aCount); }


        public Task<IEnumerable<ITrack>> GetPersonalTracks() {  return GetPersonalTracks(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aCount) {  return GetPersonalTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart, uint aCount) { return Get<Track, ITrack>("personal_songs", DeezerPermissions.BasicAccess, aStart, aCount); }


        public Task<IEnumerable<IPlaylist>> GetPlaylists() { return GetPlaylists(0, Client.ResultSize); }
        public Task<IEnumerable<IPlaylist>> GetPlaylists(uint aCount) { return GetPlaylists(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart, uint aCount) { return Get<Playlist, IPlaylist>("playlists", DeezerPermissions.BasicAccess, aStart, aCount); }



        public Task<IEnumerable<ITrack>> GetFlow() { return GetFlow(0, Client.ResultSize);  }
        public Task<IEnumerable<ITrack>> GetFlow(uint aCount) { return GetFlow(0, aCount); }
        public Task<IEnumerable<ITrack>> GetFlow(uint aStart, uint aCount) { return Get<Track, ITrack>("flow", DeezerPermissions.BasicAccess, aStart, aCount); }


        public Task<IEnumerable<ITrack>> GetHistory() { return GetHistory(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetHistory(uint aCount) { return GetHistory(0, aCount); }
        public Task<IEnumerable<ITrack>> GetHistory(uint aStart, uint aCount) {  return Get<Track, ITrack>("history", DeezerPermissions.ListeningHistory, aStart, aCount); }


        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums() { return GetRecommendedAlbums(0, Client.ResultSize); }
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aCount) { return GetRecommendedAlbums(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart, uint aCount) { return Get<Album, IAlbum>("recommendations/albums", DeezerPermissions.BasicAccess, aStart, aCount); }

        public Task<IEnumerable<IArtist>> GetRecommendedArtists() { return GetRecommendedArtists(0, Client.ResultSize); }
        public Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aCount) { return GetRecommendedArtists(0, aCount); }
        public Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart, uint aCount) { return Get<Artist, IArtist>("recommendations/artists", DeezerPermissions.BasicAccess, aStart, aCount); }

        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists() { return GetRecommendedPlaylists(0, Client.ResultSize); }
        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aCount) { return GetRecommendedPlaylists(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart, uint aCount) { return Get<Playlist, IPlaylist>("recommendations/playlists",DeezerPermissions.BasicAccess, aStart, aCount);}

        public Task<IEnumerable<ITrack>> GetRecommendedTracks() { return GetRecommendedTracks(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aCount) { return GetRecommendedTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart, uint aCount) { return Get<Track, ITrack>("recommendations/tracks", DeezerPermissions.BasicAccess, aStart, aCount); }

        public Task<uint> CreatePlaylist(string title)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id),
                RequestParameter.GetNewQueryStringParameter("title", title)
            };

            return Client.Post<DeezerCreateResponse>("user/{id}/playlists", parms, DeezerPermissions.ManageLibrary).ContinueWith(t => t.Result.Id);                        
        }

        [Obsolete("Preferable to use IPlaylist.AddTrack(s) methods instead")]
        public Task<bool> AddToPlaylist(ulong playlistId, string songids)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("playlist_id", playlistId),
                RequestParameter.GetNewQueryStringParameter("songs", songids)
            };

            return Client.Post("playlist/{playlist_id}/tracks", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> AddRadioToFavorite(ulong radioId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("radio_id", radioId),
            };

            return Client.Post("user/me/radios", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveRadioFromFavorite(ulong radioId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("radio_id", radioId),
            };

            return Client.Delete("user/me/radios", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }        

        public Task<bool> AddTrackToFavorite(ulong trackId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("track_id", trackId),                
            };

            return Client.Post("user/me/tracks", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveTrackFromFavorite(ulong trackId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("track_id", trackId),
            };

            return Client.Delete("user/me/tracks", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddPlaylistToFavorite(ulong playlistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("playlist_id", playlistId),
            };

            return Client.Post("user/me/playlists", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemovePlaylistFromFavorite(ulong playlistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("playlist_id", playlistId),
            };

            return Client.Delete("user/me/playlists", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddArtistToFavorite(ulong artistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("artist_id", artistId),
            };

            return Client.Post("user/me/artists", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveArtistFromFavorite(ulong artistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("artist_id", artistId),
            };

            return Client.Delete("user/me/artists", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddAlbumToFavorite(ulong albumId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("album_id", albumId),
            };

            return Client.Post("user/me/albums", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveAlbumFromFavorite(ulong albumId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("album_id", albumId),
            };

            return Client.Delete("user/me/albums", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }
    }
}
