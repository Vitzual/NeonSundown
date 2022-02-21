using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Enemy
{
    // Do da boom boom
    public AudioClip boom;

    // Override destroy method
    public override void Destroy()
    {
        AudioPlayer.Play(boom);
        base.Destroy();
    }
}
