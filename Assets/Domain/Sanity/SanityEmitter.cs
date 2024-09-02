using System.Collections.Generic;
using UnityEngine;

public class SanityEmitter : MonoBehaviour
{
    public float sanityDifferencePerSecond = 0f;
    public float emitterRadius = 3f;
    public List<SanityReceiver> sanityReceiversInRadius = new List<SanityReceiver>();

    void Start()
    {
        // Set up the SphereCollider to match the emitter radius and set it as a trigger
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = emitterRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        SanityReceiver sanityReceiver = other.GetComponent<SanityReceiver>();
        if (sanityReceiver != null && !sanityReceiversInRadius.Contains(sanityReceiver))
        {
            sanityReceiversInRadius.Add(sanityReceiver);
            sanityReceiver.sanityModifier += sanityDifferencePerSecond;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SanityReceiver sanityReceiver = other.GetComponent<SanityReceiver>();
        if (sanityReceiver != null && sanityReceiversInRadius.Contains(sanityReceiver))
        {
            sanityReceiversInRadius.Remove(sanityReceiver);
            sanityReceiver.sanityModifier -= sanityDifferencePerSecond;
        }
    }
}
