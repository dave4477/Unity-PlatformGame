using UnityEngine;

/**
 * Class for checking collision when the player fights.
 */
public class HitBoxManager : MonoBehaviour
{
    Enemy enemyScript;
    // Set these in the editor.
    public PolygonCollider2D highPunch;
    public PolygonCollider2D middleKick;

    // Used for organization.
    private PolygonCollider2D[] colliders;

    // Collider on this game object.
    private PolygonCollider2D localCollider;

    // We say box, but we're still using polygons.
    public enum hitBoxes
    {
        frame2,
        frame3,
        clear // Special case to remove all boxes.
    }

    void Start()
    {
        // Set up an array so our script can more easily set up the hit boxes.
        colliders = new PolygonCollider2D[] { highPunch, middleKick };

        // Create a polygon collider.
        localCollider = gameObject.AddComponent<PolygonCollider2D>();
        localCollider.isTrigger = true; // Set as a trigger so it doesn't collide with our environment.
        localCollider.pathCount = 0; // Clear auto-generated polygons.
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyScript = other.gameObject.GetComponent<Enemy>();
            enemyScript.energy--;

            Debug.Log("Enter Enemy Energy:" + enemyScript.energy);
        }
    }

    public void setHitBox(hitBoxes val)
    {
        if (val != hitBoxes.clear)
        {
            localCollider.SetPath(0, colliders[(int)val].GetPath(0));
            return;
        }
        localCollider.pathCount = 0;
    }
}