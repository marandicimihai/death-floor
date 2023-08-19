using UnityEngine;

public class Key : Item
{
    [SyncValue] [SerializeField] GameObject key1;
    [SyncValue] [SerializeField] GameObject key2;
    [SyncValue] [SerializeField] float timeLasting;
    [SyncValue] [SerializeField] float force;
    [SyncValue] [SerializeField] string keybreak;

    protected override void OnBreak()
    {
        GameObject firstkey = Instantiate(key1, transform.position, Quaternion.identity);
        firstkey.AddComponent<Lifetime>().Initiate(timeLasting);
        firstkey.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * force, ForceMode.Impulse);
        GameObject secondkey = Instantiate(key2, transform.position, Quaternion.identity);
        secondkey.AddComponent<Lifetime>().Initiate(timeLasting);
        secondkey.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * force, ForceMode.Impulse);

        AudioManager.Instance.PlayClip(keybreak);
    }
}