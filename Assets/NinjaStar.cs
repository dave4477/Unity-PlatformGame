using UnityEngine;
using System.Collections;

public class NinjaStar : MonoBehaviour {

    public float speedX = 15;
    public float interval = 2f;
    public float damage = 2f;
    public GameObject ninjaStarPrefab;

    float removeInterval = 0.7f;


	// Use this for initialization
	void Start () {
	}
	

    public void Throw(int direction)
    {
        if (ninjaStarPrefab)
        {
            var ninjaStar = GameObject.Instantiate(ninjaStarPrefab);
            ninjaStar.tag = "NinjaStar";
            ninjaStar.transform.position = this.transform.position;
            ninjaStar.GetComponent<Rigidbody2D>().AddForce(new Vector2(800*direction, 0));
            StartCoroutine(RemoveStar(ninjaStar));
        }
        else
        {
            print("No ninjastar prefab set");
        }
    }

    IEnumerator RemoveStar(GameObject ninjaStar)
    {
        yield return new WaitForSeconds(removeInterval);

        Destroy(ninjaStar);
    }
}
