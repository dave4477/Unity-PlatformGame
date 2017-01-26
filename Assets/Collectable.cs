using UnityEngine;
using System.Collections;

/**
 * Collectable objects, like weapons, health packs and other collectables.
 */
public class Collectable : MonoBehaviour
{
    public GameObject graphic;
    public ParticleSystem ps;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ps.Play();
            StartCoroutine(Collect());

            if (this.tag == "Untagged")
            {
                other.GetComponent<PlayerController>().SetScore();
            }

            if (this.tag == "CollectableHealth")
            {
                other.GetComponent<PlayerController>().energy = other.GetComponent<PlayerController>().maxEnergy;
            }
        }
    }

    IEnumerator Collect()
    {
        graphic.SetActive(false);
        yield return new WaitForSeconds(ps.duration);
        Destroy(gameObject);
    }
}
