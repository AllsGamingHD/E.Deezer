﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Endpoint
{
    public interface IPlaylistEndpoint
    {

    }

    internal class PlaylistEndpoint : IPlaylistEndpoint
    {
        private DeezerClientV2 iClient;


        public PlaylistEndpoint(DeezerClientV2 aClient) {  iClient = aClient;  }
    }
}
