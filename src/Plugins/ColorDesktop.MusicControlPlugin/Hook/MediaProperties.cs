﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.MusicControlPlugin.Hook;

public class MediaProperties
{
    public string AlbumArtist;
    public string AlbumTitle;
    public int AlbumTrackCount;
    public string Artist;
    public List<string> Genres;
    public PlaybackType? PlaybackType;
    public string Subtitle;
    public byte[]? Thumbnail;
    public string Title;
    public int TrackNumber;
}
