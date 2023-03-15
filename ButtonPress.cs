using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagment;

public class ButtonPress : MonoBehaviour
{
    Button hitButton, currentButton;
    private float timer = 3.0f;
    private float countDown = 3.0f;
    public Slider ProgressBar;
    public Text percentage;
    public float percentage_num = 0.0f;
    public float counter = 0.0f;
    public Canvas Progres;
    public Image fillarea;
    public Button scene1button, scene2button, scene3button, hidemenubutton, showmenubutton, mainmenubutton;
    public Canvas menu;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Transform camera = Camera.main.transform;
        Ray ray = new Ray(camera.position, camera.rotation * Vector3.forward);
        Debug.DrawRay(camera.position, camera.rotation * Vector3.forward * 100, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Button") // IF YA LOOKING AT BUTTON
            {
                hitButton = hit.transform.parent.gameObject.GetComponent<Button>();
                Progres.enabled = true;
                Progres.transform.position = hit.transform.position + new Vector3(0, 0, 0); // Puts progress bar above button.
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
                if (countDown <= 0f) // When countdown is 0, makes variable currentbutton the same as the button youre looking at
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
                        SceneManager.LaodScene("Main");
                    }
                    if (hitButton == hidemenubutton)
                    {
                        menu.enabled = false;
                    }
                    if (hitButton == showmenubutton)
                    {
                        menu.enabled = true;
                    }
                    currentButton = hitButton;
                }
                
                if (currentButton != null) // Probably isnt needed
                {
                    currentButton.onClick.Invoke(); //activates "current button" which is the button you are looking at "hit button"
                    currentButton.OnPointerEnter(new PointerEventData(EventSystem.current));
                    // currentButton = null; This is my fix (maybe) for button being pressed a million times a second
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
