using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

    [SerializeField] Generate ShaderScript;

    bool hidden = false;
    bool fullscreen = false;

    [SerializeField] RectTransform genSettings;
    [SerializeField] RectTransform prcSettings;
    [SerializeField] RectTransform agnSettings;
    [SerializeField] RectTransform clrSettings;

    [SerializeField] GameObject genDropdown;
    [SerializeField] GameObject prcDropdown;
    [SerializeField] GameObject agnDropdown;
    [SerializeField] GameObject clrDropdown;

    Color agentColour;

    [SerializeField] Slider redSlider;
    [SerializeField] Slider greenSlider;
    [SerializeField] Slider blueSlider;

    [SerializeField] Image colourImage;

    // Start is called before the first frame update
    void Start()
    {
        agentColour = ShaderScript.agentColour;
        agentColour.a = 0.3f;

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public void ShrinkMenu(int menu)
    {
        switch (menu) 
        {
            case 0:
                if (genDropdown.activeSelf)
                {
                    genDropdown.SetActive(false);
                    genSettings.offsetMin = new Vector2(-370, -65);
                }
                else
                {
                    genDropdown.SetActive(true);
                    genSettings.offsetMin = new Vector2(-370, -155);
                }
                break;
            case 1:
                if (prcDropdown.activeSelf)
                {
                    prcDropdown.SetActive(false);
                    prcSettings.offsetMin = new Vector2(0, -50);
                }
                else
                {
                    prcDropdown.SetActive(true);
                    prcSettings.offsetMin = new Vector2(0, -160);
                }
                break;
            case 2:
                if (agnDropdown.activeSelf)
                {
                    agnDropdown.SetActive(false);
                    agnSettings.offsetMin = new Vector2(0, -50);
                }
                else
                {
                    agnDropdown.SetActive(true);
                    agnSettings.offsetMin = new Vector2(0, -235);
                }
                break;
            case 3:
                if (clrDropdown.activeSelf)
                {
                    clrDropdown.SetActive(false);
                    clrSettings.offsetMin = new Vector2(0, -50);
                }
                else
                {
                    clrDropdown.SetActive(true);
                    clrSettings.offsetMin = new Vector2(0, -175);
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (hidden)
            {
                genSettings.gameObject.SetActive(true);
                hidden = false;
            }
            else
            {
                genSettings.gameObject.SetActive(false);
                hidden = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Application.Quit();
        }
    }

    public void SetNumAgents(string input)
    {
        if (int.TryParse(input, out int intinput))
            ShaderScript.numberOfAgents = intinput;
    }

    public void SetBlurSize(string input)
    {
        if (int.TryParse(input, out int intinput))
            ShaderScript.blurSize = intinput;
    }

    public void SetBlurSpeed(string input)
    {
        if (int.TryParse(input, out int intinput))
            ShaderScript.blurSpeed = intinput;
    }

    public void SetDarkness(string input)
    {
        string processedInput = ProcessString(input);

        if (float.TryParse(processedInput, out float floatinput))
            ShaderScript.fadeAmount = floatinput;
    }

    public void SetAgentSpeed(string input)
    {
        string processedInput = ProcessString(input);

        if (float.TryParse(processedInput, out float floatinput))
            ShaderScript.agentSpeed = floatinput;
    }

    public void SetAgentTurnSpeed(string input)
    {
        string processedInput = ProcessString(input);

        if (float.TryParse(processedInput, out float floatinput))
            ShaderScript.turnSpeed = -floatinput;
    }

    public void SetAngleSpacing(string input)
    {
        string processedInput = ProcessString(input);

        if (float.TryParse(processedInput, out float floatinput))
            ShaderScript.sensorAngleSpacing = floatinput;
    }

    public void SetOffset(string input)
    {
        string processedInput = ProcessString(input);

        if (float.TryParse(processedInput, out float floatinput))
            ShaderScript.sensorOffsetDistance = floatinput;
    }

    public void EnableGradient(bool input)
    {
        ShaderScript.useGradient = input;
    }

    public void EnableAngleAnimation(bool input)
    {
        ShaderScript.changeSpacingOverTime = input;
    }

    public void ChangeAgentColor(int colour)
    {
        switch (colour)
        {
            case 0:
                agentColour.r = redSlider.value; break;
            case 1:
                agentColour.g = greenSlider.value; break;
            case 2:
                agentColour.b = blueSlider.value; break;
        }

        colourImage.color = agentColour;
        ShaderScript.agentColour = agentColour;
    }

    public void RestartSim()
    {
        ShaderScript.restart = true;
    }

    private string ProcessString(string input)
    {
        char[] chars = input.ToCharArray();
        for(int i = 0; i < chars.Length; i++)
        {
            if (chars[i].ToString() == ".")
            {
                chars[i] = ',';
            }
        }

        string output = new string(chars);

        Debug.Log(output);

        return output;
    }
}
