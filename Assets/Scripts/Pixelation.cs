// CREDIT: https://answers.unity.com/questions/916790/is-it-possible-to-make-unitys-camera-render-in-chu.html

// Used to make the camera have a pixelated effect.
using UnityEngine;
using System.Collections;
 
public class Pixelation : MonoBehaviour {
 
    public RenderTexture renderTexture;
    public int GUIDepth = -1;
    void Start() {
        Debug.Log("(Pixelation)(Start)renderTexture.width: " + renderTexture.width);
        transform.position = new Vector3(0,0,-0.000001f);
    }
 
    void OnGUI() {
        GUI.depth = GUIDepth;
        GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), renderTexture);
    }
 
    /*
    int NearestSuperiorPowerOf2( int n ) {
        return (int) Mathf.Pow( 2, Mathf.Ceil( Mathf.Log( n ) / Mathf.Log( 2 ) ) );
    } 
    */
}
