using UnityEngine;

public class Ammo : MonoBehaviour
{
    public DataHandler.AmmoType AmmoType;
    [Min(1)] public int AmmoCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Gun[] playerGuns = other.gameObject.GetComponentsInChildren<Gun>(true);
            foreach (var gun in playerGuns)
                gun.TakeAmmo(this);
        }
    }
}
