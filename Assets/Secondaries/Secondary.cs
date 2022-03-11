using UnityEngine;

public class Secondary : MonoBehaviour
{
    // Ship instance
    public AudioClip sound;
    protected Ship ship;
    [HideInInspector]
    public SecondaryData data;
    protected float cooldown = 0;

    // Virtual setup method
    public virtual void Setup(Ship ship, SecondaryData data)
    {
        this.ship = ship;
        this.data = data;
    }

    // Virtual update method
    public void Update()
    {
        // Update secondary instance
        if (!Dealer.isOpen && cooldown > 0)
            cooldown -= Time.deltaTime;
        if (Input.GetKey(Keybinds.secondary)) Use();

        // Call custom update method
        CustomUpdate();
    }

    // Virtual update method
    public virtual void CustomUpdate() { }

    // Virtual use method
    public virtual void Use()
    {
        if (cooldown <= 0)
            cooldown = ship.cooldown;
    }

    // Destroys the instance
    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}
