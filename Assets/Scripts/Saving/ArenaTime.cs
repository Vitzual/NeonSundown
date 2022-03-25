using System.Collections.Generic;

[System.Serializable]
public class ArenaTimes
{
    public class Time
    {
        public float time;
        public string ship;
        public List<string> cards;
    }

    public List<Time> times;
}
