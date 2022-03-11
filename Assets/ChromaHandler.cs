using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaHandler : MonoBehaviour
{
    // Active chroma handler instance
    public static ChromaHandler active;

    // Get active instance
    public void Awake()
    {
        active = this;
    }

    // Setup chroma card
    public void Setup(ChromaData chroma)
    {
        switch(chroma.type)
        {
            case ChromaType.InverseExplosions:
                ExplosiveHandler.inverse = true;
                break;
        }
    }
}
