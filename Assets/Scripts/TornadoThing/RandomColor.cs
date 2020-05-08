using UnityEngine;

public class RandomColor : MonoBehaviour
{
    public Color randomColor;
    private TrailRenderer myTrail;
    public MeshRenderer[] myRenderer;
    private float randomScale;
    void Start()
    {
        myTrail = gameObject.GetComponent<TrailRenderer>();
        
        // Generate random color value.
        randomColor = new Color(
            Random.Range(0f, 1f), 
            0, 
            0
        );
        
        // Set the start color to random color
        myTrail.startColor = randomColor;
        
        // Generate random color value.
        randomColor = new Color(
            Random.Range(0f, 1f), 
            0, 
            0
        );
        // Set the end color to random color
        myTrail.endColor = randomColor;
        
        // Generate random color value.
        randomColor = new Color(
            Random.Range(0f, 1f), 
            0, 
            0
        );
        gameObject.GetComponent<Renderer> ().material.color = randomColor;
        randomScale = Random.Range(0.5f, 15f);
        transform.localScale = new Vector3(randomScale,randomScale,randomScale);
        myTrail.widthMultiplier = randomScale;
    }
}
