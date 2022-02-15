[System.Serializable]
public class ArenaSave 
{
    public ArenaSave(float bestTime, bool primary, bool secondary)
    {
        this.bestTime = bestTime;
        this.primary = primary;
        this.secondary = secondary;
    }

    public float bestTime;
    public bool primary;
    public bool secondary;
}
