using UnityEngine;

public class Halo : MonoBehaviour
{
    [SerializeField] BoxCollider gateCollider;
    [SerializeField] HaloManager haloManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        haloManager.Count();
    }

    // Update is called once per frame

    void OnTriggerExit(Collider other)
    {
        haloManager.Hit();
        Destroy(this.gameObject);
    }
}
