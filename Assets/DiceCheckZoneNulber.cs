using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneNulber : MonoBehaviour
{
    Vector3 diceVelocity;
    public static int diceCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpade()
    {
        diceVelocity = LudoGame.diceVelocity;
    }
    private void OnTriggerStay(Collider other)
    {
        if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
        {
            switch (other.gameObject.name)
            {
                case "Side1":
                    LudoGame.diceNumber = 6;
                    diceCheck = 6;
                    break;
                case "Side2":
                    LudoGame.diceNumber = 5;
                    break;
                case "Side3":
                    LudoGame.diceNumber = 4;
                    break;
                case "Side4":
                    LudoGame.diceNumber = 3;
                    break;
                case "Side5":
                    LudoGame.diceNumber = 2;
                    break;
                case "Side6":
                    LudoGame.diceNumber = 1;
                    break;

            }
        }
    }

}
