using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSpeedSetting : MonoBehaviour
{ 
    // Text that tells the current speeds of the x and y mouse movements.
    public Text xVal;
    public Text yVal;

    // Sliders that change the speeds of the x and y mouse movements.
    public Slider xSlider;
    public Slider ySlider;

    // The ySlider's value is divided by 2 while rounded up to the hundreths.
    // This is because the y mouse movement is half as fast as the x mouse movement speed.
    private float ySliderText;

    // Get the player camera and its movement script.
    private GameObject playerCamera;
    private MouseOrbitImproved my_MouseOrbitImproved_script;

    // Get the slider's values and change the camera's movement speeds to these float values.
    private float newCameraXVal;
    private float newCameraYVal;

    // Object will always remain in game, even when scene changes
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Find camera based on "PlayerCamera" tag, get its movement script from the object.
        playerCamera = GameObject.FindWithTag("PlayerCamera");
        my_MouseOrbitImproved_script = playerCamera.GetComponent<MouseOrbitImproved>();
        
        // Get UI speed value text
        xVal = GameObject.Find("PauseScreenCamera/PauseCanvas/UI_Group/SliderGroup/X Value").GetComponent<Text>();
        yVal = GameObject.Find("PauseScreenCamera/PauseCanvas/UI_Group/SliderGroup/Y Value").GetComponent<Text>();
        
        // Get UI sliders that change speeds.
        xSlider = GameObject.Find("PauseScreenCamera/PauseCanvas/UI_Group/SliderGroup/MouseXSlider").GetComponent<Slider>();
        ySlider = GameObject.Find("PauseScreenCamera/PauseCanvas/UI_Group/SliderGroup/MouseYSlider").GetComponent<Slider>();
        
        // Change slider values to the camera movement script's current speeds.
        xSlider.value = my_MouseOrbitImproved_script.xSpeed;
        ySlider.value = my_MouseOrbitImproved_script.ySpeed;
    }

    void FixedUpdate()
    {
        // Only change values when the player camera goes inactive, which shows the pause screen behind it.
        if (playerCamera.active == true)
        {
            // Round up the slider's value to the hundreths.
            xSlider.value = Mathf.Round(xSlider.value * 100f) / 100f;
            ySlider.value = Mathf.Round(ySlider.value * 100f) / 100f;
                      
            // Store the slider's values into these two float slots.
            newCameraXVal = xSlider.value;
            newCameraYVal = ySlider.value;
            
            // Change the camera's movement speed variables to the float slots if it hasn't been done already.
            if (my_MouseOrbitImproved_script.xSpeed != newCameraXVal ||
                my_MouseOrbitImproved_script.ySpeed != newCameraYVal)
            {
                my_MouseOrbitImproved_script.xSpeed = newCameraXVal;
                my_MouseOrbitImproved_script.ySpeed = newCameraYVal;
            }
        }
    }

    void Update()
    {
        // Change speed value texts to slider's current values
        xVal.text = (Mathf.Round(xSlider.value * 100f) / 100f).ToString();
            
        // See ySliderText initialization for info.
        ySliderText = Mathf.Round((ySlider.value/2) * 100f) / 100f;
        yVal.text = ySliderText.ToString();      
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // If xVal slot is null, get all the objects again.
        // See void Start() for info on objects.
        if (xVal == null)
        {
            playerCamera = GameObject.FindWithTag("PlayerCamera");
            my_MouseOrbitImproved_script = playerCamera.GetComponent<MouseOrbitImproved>();
            
            xVal = GameObject.Find("PauseScreenCamera/PauseCanvas/UI_Group/SliderGroup/X Value").GetComponent<Text>();
            yVal = GameObject.Find("PauseScreenCamera/PauseCanvas/UI_Group/SliderGroup/Y Value").GetComponent<Text>();
            xSlider = GameObject.Find("PauseScreenCamera/PauseCanvas/UI_Group/SliderGroup/MouseXSlider").GetComponent<Slider>();
            ySlider = GameObject.Find("PauseScreenCamera/PauseCanvas/UI_Group/SliderGroup/MouseYSlider").GetComponent<Slider>();

            xSlider.value = newCameraXVal;
            ySlider.value = newCameraYVal;              
        }
        




    }
}
