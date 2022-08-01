using System.Collections.Generic;

[System.Serializable]
public class MetaContext
{
    public MetaContext(string lastArena, string lastShip, string lastSong, List<string> lastModules)
    {
        this.lastArena = lastArena;
        this.lastShip = lastShip;
        this.lastSong = lastSong;
        this.lastModules = lastModules;
    }

    public string lastArena;
    public string lastShip;
    public string lastSong;
    public List<string> lastModules;
}
