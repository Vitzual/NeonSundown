using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    public TextMeshPro amount;
    public Color critColor;
    [HideInInspector]
    public float time;
    public float speed;
    [HideInInspector]
    public bool isActive = false;
    private Vector3 increaseScale;
    private Vector3 decreaseScale;

    public void Set(float dmg, bool crit) 
    {
        time = 1f;
        isActive = true;
        amount.text = Formatter.Round(dmg);

        // Set crit color
        if (crit)
        {
            amount.color = critColor;
            transform.localScale = new Vector2(2.5f, 2.5f);
            increaseScale = new Vector3(3f, 3f, 3f);
            decreaseScale = new Vector3(-6f, -6f, -6f);
        }
        else 
        { 
            amount.color = Color.white;
            transform.localScale = new Vector2(2f, 2f);
            increaseScale = new Vector3(2f, 2f, 2f);
            decreaseScale = new Vector3(-4f, -4f, -4f);
        }
    }

    public void Move()
    {
        // Move the object up
        transform.position += transform.up * speed * Time.deltaTime;
        time -= Time.deltaTime;

        // Scale based on time
        if (time > 0.5f) transform.localScale += increaseScale * Time.deltaTime;
        else if (time > 0f) transform.localScale += decreaseScale * Time.deltaTime;
        else Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        isActive = false;
    }
}
