using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BuyButProps : MonoBehaviour
{
    public int id = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (id)
        {
            case 1:
                if (PlayerController.speedBonus >= 3)
                {
                    GetComponentInChildren<TextMeshProUGUI>().text = "MAXED";
                    GetComponent<Button>().interactable = false;
                    

                }
                else
                {
                    if((PlayerController.speedBonus + 1) * 500 > PlayerController.currencyMeter)
                    {
                        GetComponent<Button>().interactable = false;
                        
                        GetComponent<Image>().color = Color.red;
                    }
                    GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL " + (PlayerController.speedBonus + 1) + "    X" + ((PlayerController.speedBonus + 1) * 500);
                }
                    break;
            case 2:
                if (PlayerController.forceBonus >= 3)
                {
                    GetComponentInChildren<TextMeshProUGUI>().text = "MAXED";
                    GetComponent<Button>().interactable = false;


                }
                else
                {
                    if ((PlayerController.forceBonus + 1) * 500 > PlayerController.currencyMeter)
                    {
                        GetComponent<Button>().interactable = false;
                        GetComponent<Image>().color = Color.red;
                    }
                    GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL " + (PlayerController.forceBonus + 1) + "    X" + ((PlayerController.forceBonus + 1) * 500);

                }
                break;
            case 3:
                if (PlayerController.magnetBonus >= 3)
                {
                    GetComponentInChildren<TextMeshProUGUI>().text = "MAXED";
                    GetComponent<Button>().interactable = false;


                }
                else
                {
                    if ((PlayerController.magnetBonus + 1) * 500 > PlayerController.currencyMeter)
                    {
                        GetComponent<Button>().interactable = false;
                        GetComponent<Image>().color = Color.red;
                    }
                    GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL " + (PlayerController.magnetBonus + 1) + "    X" + ((PlayerController.magnetBonus + 1) * 500);
                }
                    break;
            case 4:
                if (PlayerController.boostBonus >= 3)
                {
                    GetComponentInChildren<TextMeshProUGUI>().text = "MAXED";
                    GetComponent<Button>().interactable = false;


                }
                else
                {
                    if ((PlayerController.boostBonus + 1) * 500 > PlayerController.currencyMeter)
                    {
                        GetComponent<Button>().interactable = false;
                        GetComponent<Image>().color = Color.red;
                    }
                    GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL " + (PlayerController.boostBonus + 1) + "    X" + ((PlayerController.boostBonus + 1) * 500);
                }
                    break;
            case 5:
                if (PlayerController.fuelBonus >= 3)
                {
                    GetComponentInChildren<TextMeshProUGUI>().text = "MAXED";
                    GetComponent<Button>().interactable = false;


                }
                else
                {
                    if ((PlayerController.fuelBonus + 1) * 500 > PlayerController.currencyMeter)
                    {
                       GetComponent<Button>().interactable = false;
                        GetComponent<Image>().color = Color.red;
                    }
                    GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL " + (PlayerController.fuelBonus + 1) + "    X" + ((PlayerController.fuelBonus + 1) * 500);
                }
                    break;
        }        
    }
}
