using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        // Obține limitele grilei din colider
        Bounds bounds = this.gridArea.bounds;

        // Setăm o poziție random, asigurându-ne că valorile sunt întregi (aliniate pe grilă)
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));

        // Setăm poziția mâncării pe grilă
        this.transform.position = new Vector3(x, y, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Dacă șarpele mănâncă mâncarea, repoziționăm mâncarea
        if (other.CompareTag("Player"))
        {
            RandomizePosition();
        }
    }
}
