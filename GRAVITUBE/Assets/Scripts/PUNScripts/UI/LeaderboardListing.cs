using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LeaderboardListing : MonoBehaviour
{
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI nameText;
   
    public void setTextColor(Color color)
    {
        nameText.color = color;
    }   

}
