using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorPicker : MonoBehaviour
{
    public Material tubeMaterial;
    TMP_Dropdown colorDropdown;
    public bool primary = true;
    private void Start()
    {
        colorDropdown = GetComponent<TMP_Dropdown>();
        if (primary)
        {
            colorDropdown.value = PlayerPrefs.GetInt("BASECOLOR");

        }
        else
        {
            colorDropdown.value = PlayerPrefs.GetInt("STRIPECOLOR");
        }
    }

    private void Update()
    {
        
        
    }

    public void OnColorChoose()
    {
        
            Color color;
    
        switch (colorDropdown.options[colorDropdown.value].text)
        {
            case "RED":
                color = Color.red;
                break;
            case "BLUE":
                color = Color.blue;
                break;
            case "WHITE":
                color = Color.white;
                break;
            case "BLACK":
                color = Color.black;
                break;
            case "MAGNETA":
                color = Color.magenta;
                break;
            case "GREEN":
                color = Color.green;
                break;
            case "CYAN":
                color = Color.cyan;
                break;
            case "YELLOW":
                color = Color.yellow;
                break;
            default:
                color = Color.white;
                break;
        }

            if (tubeMaterial != null)
            {
            if (primary)
            {
                PlayerPrefs.SetInt("BASECOLOR", colorDropdown.value);
                tubeMaterial.SetColor("Color_7C020304", color);
            }
            else
            {
                tubeMaterial.SetColor("Color_6E041D6A", color);
                PlayerPrefs.SetInt("STRIPECOLOR", colorDropdown.value);
            }
            }
        }
    }

