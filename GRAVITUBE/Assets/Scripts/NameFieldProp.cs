using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class NameFieldProp : MonoBehaviour
{
    

    public Button applyBut;

    public static string name;

    public bool fresh=false;

    public static TMP_InputField my_InputField;

    public bool roomCase = false;

    private void Start()
    {
        my_InputField = GetComponent<TMP_InputField>();
    }
    public bool isValidName(string name)
    {
        if (name.Trim().Length <= 1 || name.Length>8)
        {
            return false;
        }

        int errorCounter;
        errorCounter = System.Text.RegularExpressions.Regex.Matches(name, @"[a-zA-Z]").Count;
        if (errorCounter == 0)
        {
            return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        my_InputField = GetComponent<TMP_InputField>();
        if(!roomCase)
        GetComponentInChildren<RectMask2D>().GetComponentInChildren<TextMeshProUGUI>().text = UIScript.playerName;
        
        name = my_InputField.text;
        if(applyBut!=null)
        applyBut.interactable = (isValidName(my_InputField.text)||(!roomCase&&(string.IsNullOrEmpty(my_InputField.text)&&!fresh)));
    }
}
