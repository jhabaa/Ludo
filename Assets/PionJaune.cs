using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PionJaune : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClicOnPlayer()
    {
        LudoGame.selectedPlayer = this.gameObject;
        print("Player Sel");
    }
}
