using System.Collections.Generic;

[System.Serializable]
public class MetaContext
{
    public MetaContext(string lastArena, string lastShip, List<string> lastBlacklisted)
    {
        this.lastArena = lastArena;
        this.lastShip = lastShip;
        this.lastBlacklisted = lastBlacklisted;
    }

    public string lastArena;
    public string lastShip;
    public List<string> lastBlacklisted;
}
