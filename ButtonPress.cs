using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;


public class ButtonPress : MonoBehaviour
{
    Button hitButton, currentButton;
    private float timer = 3.0f;
    private float countDown = 3.0f;
    public Slider ProgressBar;
    public Text percentage;
    private float percentage_num = 0.0f;
    private float counter = 0.0f;
    public Canvas Progres;
    public Image fillarea;
    public Button scene1button, scene2button, scene3button, hidemenubutton, showmenubutton, mainmenubutton, breathingButton;
    public Canvas menu;
    // Jeez time to organize at least some. This next stuff is just for the breathing guide
    public Canvas breathingCanvas;
    public Slider breathingSlider;
    public Text breathingText;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;
    private float CameraDistance = 50f;
    public Image breathingFillarea;
    private Boolean goingdown = false;
    private float holdtimer = 0f;


 

    // Start is called before the first frame update
    void Start()
    {
        breathingCanvas.enabled = false;
        breathingCanvas.transform.position = new Vector3(0, 0, 50);
        breathingSlider.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Transform camera = Camera.main.transform;
        Ray ray = new Ray(camera.position, camera.rotation * Vector3.forward);
        Debug.DrawRay(camera.position, camera.rotation * Vector3.forward * 100, Color.red);
        RaycastHit hit;
        /*
        if (menu.enabled == true)
        {
            menu.transform.LookAt(Camera.main.transform); // Makes progress bar look at you
            Vector3 rots = menu.transform.rotation.eulerAngles;
            rots = new Vector3(rots.x, rots.y + 180, rots.z);
            menu.transform.rotation = Quaternion.Euler(rots);
        }*/

        if (breathingCanvas.enabled == true) // all this is for the breathing guide boi
        {
            Vector3 targetPosition = camera.transform.TransformPoint(new Vector3(0, -10, CameraDistance)); // makes that ish, not like directly infront of you eyes
            breathingCanvas.transform.position = Vector3.SmoothDamp(breathingCanvas.transform.position, targetPosition, ref velocity, smoothTime);
            var lookAtPos = new Vector3(camera.transform.position.x, breathingCanvas.transform.position.y, camera.transform.position.z);
            breathingCanvas.transform.LookAt(lookAtPos);
            Vector3 rote = breathingCanvas.transform.rotation.eulerAngles;
            rote = new Vector3(rote.x, rote.y + 180, rote.z);
            breathingCanvas.transform.rotation = Quaternion.Euler(rote);
            // All of that just so it smoothly follows you and is rotated the correct way... !!!
            // Now to make it go up and down and say breathe in and exhale.... 
            if (goingdown != true) // If you aren't exhaling...
            {
                breathingSlider.value += Time.deltaTime;
                breathingFillarea.GetComponent<Image>().color = new Color(0, 255, 0);
                breathingText.text = "Inhale";
                breathingText.GetComponent<Text>().color = new Color(255, 150, 0);
                if (breathingSlider.value == 5) // when slider at full, start the hold
                {
                    holdtimer += Time.deltaTime;
                    breathingText.GetComponent<Text>().color = new Color(255, 0, 0);
                    breathingText.text = "Hold!";
                    breathingFillarea.GetComponent<Image>().color = new Color(255, 0, 0);

                    if (holdtimer > 3f && breathingSlider.value == 5) // If done holding breath,tell program to exhale.
                    {
                        goingdown = true;
                    }
                }
            }

            if (goingdown == true) // If Exhaling
            {
                breathingSlider.value -= Time.deltaTime * 1.5f;
                breathingFillarea.GetComponent<Image>().color = new Color(255, 150, 0);
                breathingText.text = "Exhale";
                breathingText.GetComponent<Text>().color = new Color(0, 255, 0);
                holdtimer = 0f;
                if (breathingSlider.value == 0) // Once slider is at 0, tell program youre done exhaling and to inhale again.
                {
                    goingdown = false;
                }
                
            }
        }

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Button") // IF YA LOOKING AT BUTTON
            {
                hitButton = hit.transform.parent.gameObject.GetComponent<Button>();
                Progres.enabled = true;
                Progres.transform.position = hit.transform.position + new Vector3(0, 0, -2); // Puts progress bar above button.
                Progres.transform.LookAt(Camera.main.transform); // Makes progress bar look at you
                Vector3 rot = Progres.transform.rotation.eulerAngles;
                rot = new Vector3(rot.x, rot.y + 180, rot.z);
                Progres.transform.rotation = Quaternion.Euler(rot);
                if (hitButton == showmenubutton)
                {
                    Progres.enabled = true;
                    Progres.transform.position = hit.transform.position + new Vector3(0, 0, -1); // Puts progress bar above button.
                    Progres.transform.LookAt(Camera.main.transform); // Makes progress bar look at you
                    Vector3 rotz = Progres.transform.rotation.eulerAngles;
                    rotz = new Vector3(rotz.x + 180, rotz.y - 90, rotz.z + 90); 
                    Progres.transform.rotation = Quaternion.Euler(rotz);
                }

                //print("name= " + hitButton.name);
                countDown -= Time.deltaTime; // TIMER GO DOWN BY SECOND
                counter += Time.deltaTime;
                ProgressBar.value = counter;
                percentage_num = ((counter / 3.00f) * 100); // Math 4 percentage
                if (counter >= 0 || counter <= 1)
                {
                    fillarea.GetComponent<Image>().color = new Color(255, 0, 0);
                    percentage.text = $"{Math.Round(percentage_num, 2)}%"; // Writes text to percentage
                }
                if (counter >= 1.5 || counter <= 3)
                {
                    fillarea.GetComponent<Image>().color = new Color(255, 255, 0);
                    percentage.text = $"{Math.Round(percentage_num, 2)}%"; // Writes text to percentage
                }
                if (counter >= 3)
                {
                    percentage.text = "Done!";
                    fillarea.GetComponent<Image>().color = new Color(0, 255, 0);
                }

            }
            else
            {
                hitButton = null;
                counter = 0.0f;
                countDown = timer; // RESETS TIMER IF UR NOT LOOKING AT IT
                ProgressBar.value = counter;
                percentage_num = 0.0f;
                percentage.text = $"{Math.Round(percentage_num, 2)}%"; // Writes text to percentage
                Progres.transform.position = new Vector3(9999, 9999, 9999); // Gets progress bar outta there
                Progres.enabled = false;

            }
            if (currentButton != hitButton)
            {
                //unhighlight
                if (currentButton != null)
                {
                    currentButton.OnPointerExit(new PointerEventData(EventSystem.current));
                }
                //make changes
                if (countDown <= 0f && countDown > -.5f) // When countdown is 0, makes variable currentbutton the same as the button youre looking at
                {
                   
                    // not sure this is needed at all
                    if (hitButton == scene1button)
                    {
                        SceneManager.LoadScene("Scene1");
                    }
                    if (hitButton == scene2button)
                    {
                        SceneManager.LoadScene("Scene2");
                    }
                    if (hitButton == scene3button)
                    {
                        SceneManager.LoadScene("Scene3");
                    }
                    if (hitButton == mainmenubutton)
                    {
                        SceneManager.LoadScene("Main");
                    }
                    if (hitButton == hidemenubutton)
                    {
                        menu.enabled = false;
                        menu.transform.position = new Vector3(9999, 9999, 9999);
                        countDown = -5f; // this makes the button press once
                    }
                    if (hitButton == showmenubutton)
                    {
                        menu.enabled = true;
                        menu.transform.position = new Vector3(0, 0, 66.4f);
                        countDown = -5f; // yes the fix
                    }
                    if (hitButton == breathingButton)
                    {
                        breathingCanvas.enabled = !breathingCanvas.enabled;
                        countDown = -5f; // finally the fix
                    }
                    currentButton = hitButton;
                }
                
                if (currentButton != null) // Probably isnt needed
                {
                    currentButton.onClick.Invoke(); //activates "current button" which is the button you are looking at "hit button"
                    currentButton.OnPointerEnter(new PointerEventData(EventSystem.current));
                    currentButton = null; //This is my fix (maybe) for button being pressed a million times a second... **THIS DIDNT FIX IT BUT THE FIX IS UP THERE^^^^**
                }
            }
        }
        else
        {
            hitButton = null;
            counter = 0.0f;
            countDown = timer; // RESETS TIMER IF UR NOT LOOKING AT IT
            ProgressBar.value = counter;
            percentage_num = 0.0f;
            percentage.text = $"{Math.Round(percentage_num, 2)}%"; // Writes text to percentage
            Progres.transform.position = new Vector3(9999, 9999, 9999); // Gets progress bar outta there
            Progres.enabled = false;

        }
        
    }
}
