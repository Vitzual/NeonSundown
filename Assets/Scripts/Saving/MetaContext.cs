using System.Collections.Generic;

[System.Serializable]
public class MetaContext
{
    public MetaContext(string lastArena, string lastShip, List<string> lastModules)
    {
        this.lastArena = lastArena;
        this.lastShip = lastShip;
        this.lastModules = lastModules;
    }

    public string lastArena;
    public string lastShip;
    public List<string> lastModules;
}
