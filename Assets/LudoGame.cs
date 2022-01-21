using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
//using Random = System.Random;

public class LudoGame : MonoBehaviour
{
    private int[,] grid;
    private List<GameObject> ToDelete;
    public GameObject plateau, tempObjects, tile, pion3D, Dice, textScreen;
    public Material test;
    public List<GameObject> yellowHome, redHome, arena, finishYellow, finishRed, gamelist, newPath, yellowPath, redPath;
    public bool jeton = true;
    public int deepness = 2;
    private string yellow;
    public static GameObject selectedPlayer;
    private static Rigidbody rb;
    public static Vector3 diceVelocity;
    public static int diceNumber;
    public int dice;
    private Text screenText;
    private String[] minimaxi;
    public List<String> feuillesEnArene;
    public AudioSource audioData;
    public TypeOutScript TOS;
    String gameScore;

    // Start is called before the first frame update
    void Start()
    {
        screenText = textScreen.GetComponent<Text>(); // Display a Text zone on the camera HUD
        audioData = GetComponent<AudioSource>();
        audioData.Play(0); // Akatsuki theme for the song while playing
        gameScore = "0#0";
    }

    private void DrawTable(int x, int y)
    {
        var g = GameObject.Instantiate(tile, new Vector3((plateau.transform.position.x-0.18f)+(x*0.03f), plateau.transform.position.y, (plateau.transform.position.z-0.18f+(y*0.03f))), Quaternion.identity);
        g.name = x.ToString() + "," + y.ToString() + "#" + string.Empty;
        g.transform.parent = plateau.transform;
        g.tag = "white";
        gamelist.Add(g);
    }

/*
 * =================================================== Scores Panel ==============================================
 */
    IEnumerator ShowText(String textToShow)
    {
        screenText.text = textToShow;
        yield return new WaitForSeconds(5);
        String[] gameScoreShow = gameScore.Split('#');
        screenText.text = " Score : Y = " + gameScoreShow[0] + " R = " + gameScoreShow[1] ;
       
    }
/*
 * =================================================== Dice ===============================================================
 */
    public void DiceRecognition()
    {
        StartCoroutine(ShowText("Dés reconnu"));
        print("Dés reconnu");
    }

    public void StartGame()
    {
        StartCoroutine(ShowText("Game Started"));
        Debug.Log("Game started");
        for (int x = 0; x < 13; x++)
        {
            for (int y = 0; y < 13; y++)
            {
                DrawTable(x, y);
            }
        }
        DrawCross();
        SortPlayers();
        Path(gamelist);
        diceNumber = 6;
        yellowPath =new List<GameObject>(newPath);
        redPath = new List<GameObject>(newPath);
        YellowPathMaker(yellowPath);
        RedPathMaker(redPath);
    }

    /*
     * We created our board by delete 4 squares in a square of squares. Then We have to align the rest. Then, players could walk the right way
     * ====================================================== Paths Creation ===================================================================
     */
    private void Path (List<GameObject> path)
    {
        int a = 5, b = 7;
        
        for (int x = 0; x < 1; x++)
        {
            for (int y = 5; y <8; y++)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int y = 7; y < 8; y++)
        {
            for (int x = 1; x < 6; x++)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int x = 5; x < 6; x++)
        {
            for (int y = 8; y < 13; y++)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int y = 12; y < 13; y++)
        {
            for (int x = 6; x < 8; x++)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int x = 7; x < 8; x++)
        {
            for (int y = 11; y > 6; y--)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int y = 7; y < 8; y++)
        {
            for (int x = 8; x < 13; x++)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int x = 12; x < 13; x++)
        {
            for (int y = 6; y > 4; y--)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int y = 5; y < 6; y++)
        {
            for (int x = 11; x > 6; x--)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int x = 7; x < 8; x++)
        {
            for (int y = 4; y >= 0; y--)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int y = 0; y < 1; y++)
        {
            for (int x = 6; x > 4; x--)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int x = 5; x < 6; x++)
        {
            for (int y = 1; y < 6; y++)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
        for (int y = 5; y < 6; y++)
        {
            for (int x = 4; x > 0; x--)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                newPath.Add(g);
            }
        }
    }

    //Let's make a Yellow Path
    private void YellowPathMaker(List<GameObject> gameObjects)
    {
        gameObjects.AddRange( gameObjects.GetRange(0,39));
        gameObjects.RemoveRange(0, 39);
        yellowPath.RemoveAt(yellowPath.Count-1);
        yellowPath.AddRange(finishYellow.GetRange(0,5));
        //Colorions la première case
        yellowPath[0].GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    //Lets's make Red Path
    private void RedPathMaker(List<GameObject> gameObjects)
    {
        gameObjects.AddRange(gameObjects.GetRange(0, 3));
        gameObjects.RemoveRange(0, 3);
        redPath.RemoveAt(redPath.Count - 1);
        redPath.AddRange(finishRed.GetRange(0, 5));
        //Color the first case. It's the game
        redPath[0].GetComponent<MeshRenderer>().material.color = Color.red;
    }

    //We create teams. It's just a tag sort
    private void SortPlayers()
    {
        yellowHome = GameObject.FindGameObjectsWithTag("yellow").ToList();
        GameObject yellowStart = GameObject.Find(5 + "," + 1 + "#" + string.Empty);
        yellowStart.tag = "startyellow";
        gamelist.Add(yellowStart);
        //For the Red Team
        redHome = GameObject.FindGameObjectsWithTag("red").ToList();

    }

    //Cross are some color lines on the board to indicate the way of End Home
    public void DrawCross()
    {

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y <5; y++)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                g.tag = "Player";
                gamelist.Remove(g);
                Destroy(g);    
            }
        }
        for (int x = 8; x < 13; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                g.tag = "Player";
                gamelist.Remove(g);
                Destroy(g);

            }
        }
        for (int x = 0; x < 5; x++)
        {
            for (int y = 8; y < 13; y++)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                g.tag = "Player";
                gamelist.Remove(g);
                Destroy(g);

            }
        }
        for (int x = 8; x < 13; x++)
        {
            for (int y = 8; y < 13; y++)
            {
                GameObject g = GameObject.Find(x.ToString() + "," + y.ToString() + "#" + string.Empty);
                g.tag = "Player";
                gamelist.Remove(g);
                Destroy(g);

            }
        }
        GameObject center = GameObject.Find(6.ToString() + "," + 6.ToString() + "#" + string.Empty);
        gamelist.Remove(center);
        Destroy(center);
        //Just to color some lines on the board in yellow and red
        int redLineY = 6;
        int[] redlineX = {1,2,3,4,5,7,8,9,10,11};
        int[] yellowLineY = {1,2,3,4,5,7,8,9,10,11};
        int yellowLineX = 6;
        foreach (int x in redlineX)
        {
            GameObject g = GameObject.Find(x.ToString() + "," + redLineY.ToString() + "#" + string.Empty);
            var mesh = g.GetComponent<MeshRenderer>().material.color = Color.red;
            gamelist.Remove(g);
            finishRed.Add(g);
            g.tag = "finishRed";
        }
        foreach (int y in yellowLineY)
        {
            GameObject g = GameObject.Find(yellowLineX.ToString() + "," + y.ToString() + "#" + string.Empty);
            var mesh = g.GetComponent<MeshRenderer>().material.color = Color.yellow;
            gamelist.Remove(g);
            finishYellow.Add(g);
            g.tag = "finishYellow";
        }
    }

    // Let's simulate the game
    // Game starts if a player can have a 6 from dice

    /*
     * Ok Let's play. Playgame function can start a couroutine depending on the side
     */
    private void playGame()
    {
        if (jeton == false)
        {
            StartCoroutine(ShowText("Au tour de l'ordinateur"));
            StartCoroutine(CPUGame());
        }
        else
        if(jeton == true)
        {
            StartCoroutine(ShowText("A votre tour de jouer"));
            StartCoroutine(PlayerGame());
        }

    }

    /*
     * Did you realize how our Dice is so real ? Take a look at this code to know how
     */
    IEnumerator DiceTrick()
    {
        selectedPlayer = null;
        GameObject newDice = GameObject.Instantiate(Dice);
        newDice.transform.position = new Vector3(plateau.transform.position.x, plateau.transform.position.y+0.2f , plateau.transform.position.z );
        rb = newDice.GetComponent<Rigidbody>();
        newDice.tag = "dice";
        diceVelocity = rb.velocity;
        
        float dirX = UnityEngine.Random.Range(0, 500);
        float dirY = UnityEngine.Random.Range(0, 500);
        float dirZ = Random.Range(0, 500);
        transform.position = new Vector3(0, 2, 0);
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 500);
        rb.AddTorque(dirX, dirY, dirZ);
        
        yield return null;
    }

    //================================================== Player ==========================================================================
    //========================================================================================================================
    /*
     * Player game is the Player side algorithm
     */
    IEnumerator PlayerGame()
    {
        StartCoroutine(DiceTrick());

        StartCoroutine(ShowText(" Select a player "));
        yield return StartCoroutine(WaitForPlayerSelection());
        Debug.Log("Ok. You selected a Player : " + selectedPlayer.name);
        Destroy(GameObject.FindGameObjectWithTag("dice"));
        if (yellowHome.Count == 4)
        {
            if (diceNumber == 6)
            {
                MoveFromHome("yellow", selectedPlayer);
                jeton ^= jeton;
                playGame();
            }
            if(diceNumber != 6)
            {
                StartCoroutine(ShowText(diceNumber + " Cannot move "));
                jeton ^= jeton;
                playGame();
            }
              
        }else
        {
            if (arena.Contains(selectedPlayer))
            {
                string[] split1 = selectedPlayer.name.Split('#');
                int posInPath = int.Parse(split1[1]) + diceNumber;
                print(posInPath);
                
                if(posInPath > 46)
                {
                    if (posInPath < 51)
                    {
                        //On entre dans la dernière ligne
                        finishYellow.Add(selectedPlayer);
                        arena.Remove(selectedPlayer);
                        //On déplace le joueur dans la ligne d'arrivée
                        MoveAPlayer(selectedPlayer, posInPath, "yellow");
                        
                    }else

                    if(posInPath == 51)
                    {
                        arena.Remove(selectedPlayer);
                        StartCoroutine(ShowText("Player Arrived"));
                    }
                    else if(posInPath > 51)
                    {
                        StartCoroutine(ShowText("Unautorize Movement"));
                    }
                }
                else //Nous n'arrivons pas dans la ligne d'arrivée, donc on avance tout simplement.
                {
                    MoveAPlayer(selectedPlayer, posInPath, "yellow") ;
                }
                jeton ^= jeton;
                playGame();
            }else if (finishYellow.Contains(selectedPlayer))
            {
                string[] split1 = selectedPlayer.name.Split('#');
                int posInPath = int.Parse(split1[1]) + diceNumber;
                print(posInPath);
                if (posInPath < 51)
                {
                    //On déplace le joueur dans la ligne d'arrivée
                    Vector3 newPos = new Vector3(yellowPath[posInPath].transform.position.x, yellowPath[posInPath].transform.position.y + 0.028f, yellowPath[posInPath].transform.position.z);
                    selectedPlayer.transform.position = newPos;
                    selectedPlayer.name = split1[0].ToString() + "#" + posInPath.ToString();

                }
                else if(posInPath == 51)
                {
                    //On déplace le joueur jusqu'à l'arrivée
                    Vector3 newPos = new Vector3(yellowPath[posInPath].transform.position.x, yellowPath[posInPath].transform.position.y + 0.028f, yellowPath[posInPath].transform.position.z);
                    selectedPlayer.transform.position = newPos;
                    selectedPlayer.name = split1[0].ToString() + "#" + posInPath.ToString();
                    yield return new WaitForSeconds(1);
                    finishYellow.Remove(selectedPlayer);
                    Destruction(selectedPlayer);
                }
                else
                {
                    StartCoroutine(ShowText(" Impossible To Play"));
                }
            }else if (yellowHome.Contains(selectedPlayer))
            {
                MoveFromHome("yellow", selectedPlayer);
                jeton ^= jeton;
                playGame();
            }
        }
        selectedPlayer = null;
    }

    private void MoveAPlayer(GameObject selectedPlayer, int posInPath, string v)
    {
        if (v == "yellow")
        {
            string[] split1 = selectedPlayer.name.Split('#');
            Vector3 newPos = new Vector3(yellowPath[posInPath].transform.position.x, yellowPath[posInPath].transform.position.y + 0.028f, yellowPath[posInPath].transform.position.z);
            selectedPlayer.transform.position = newPos;
            selectedPlayer.name = split1[0].ToString() + "#" + posInPath.ToString();

            //On vérifie si la case destination est occupée
            GameObject playerInCase = arena.Find(x => x.transform.position == newPos && x.tag == "red");
            //Si elle est occupée on démolit le pion sur place
            if(playerInCase != null)
            {
                StartCoroutine(Destruction(playerInCase));
                //Change gameScore
                String[] scoresplit = gameScore.Split('#');
                gameScore = scoresplit[0] + "#" + (int.Parse(scoresplit[1] + 1)).ToString();
                //On verifie le gagnant
                if(int.Parse(scoresplit[1] + 1) == 4)
                {
                    StartCoroutine(ShowText(" Game Over. Yellow wins"));
                }
            }else if(playerInCase == null){
                StartCoroutine(ShowText(" No more Players at Home"));
            }

        }
        if (v == "red")
        {
            string[] splitNewPos = selectedPlayer.name.Split('#');
            print("Nouvelle position à : " + posInPath);
            Vector3 positionToMove = new Vector3(redPath[posInPath].transform.position.x, redPath[posInPath].transform.position.y + 0.028f, redPath[posInPath].transform.position.z);
            selectedPlayer.transform.position = Vector3.MoveTowards(selectedPlayer.transform.position, positionToMove, 200 * Time.deltaTime);
            selectedPlayer.name = splitNewPos[0] + "#" + posInPath.ToString();

            //On vérifie si la case destination est occupée
            GameObject playerInCase = arena.Find(x => x.transform.position == positionToMove && x.tag == "yellow");
            //Si ell est occupée on démolit le pion sur place
            if (playerInCase != null)
            {
                StartCoroutine(Destruction(playerInCase));
                print("Joueur Detruit");
                //Change gameScore
                String[] scoresplit = gameScore.Split('#');
                gameScore = (int.Parse(scoresplit[0] + 1)).ToString()+ "#"+ scoresplit[1];
                //On verifie le gagnant
                if (int.Parse(scoresplit[0] + 1) == 4)
                {
                    StartCoroutine(ShowText("Partie Terminée. Rouges gagnent"));
                }
            }
        }
    }

    IEnumerator Destruction(GameObject playerInCase)
    {
        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            playerInCase.transform.Translate(Vector3.forward * elapsedTime/1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        } 
        yield return new WaitForSeconds(1);

        arena.Remove(playerInCase);
        Destroy(playerInCase);
    }

    IEnumerator WaitForPlayerSelection()
    {
        while (selectedPlayer == null)
        {
            yield return null;
        }
    }

    IEnumerator JetonSwap()
    {
        jeton ^= jeton;
        yield return jeton;
    }

    IEnumerator CPUGame()
    {
        yield return StartCoroutine(DiceTrick());
        yield return new WaitForSeconds(3);
        StartCoroutine(ShowText("Le CPU doit jouer " + diceNumber));

        LaunchMinMax(ArenaToList(arena), 2, true, diceNumber);
        jeton = true;

        //Pseudo Code
        // MiniMax(arena, depth);
        yield return new WaitForSeconds(3);
        Destroy(GameObject.FindGameObjectWithTag("dice"));
    }

    private List<string> ArenaToList(List<GameObject> arena)
    {
        List<String> result = new List<string>();
        foreach(GameObject gameObject in arena)
        {
            result.Add(gameObject.name);
        }
        return result;
    }


    /*
     This is the evalution function
     */
    private int Evaluate(List<String> gameObjects)
    {
        print("======================================== Evaluation ==============================================================");
        int score = 2000;
        List<String> redTeam = gameObjects.FindAll(r => r.Contains("red"));
        List<String> yellowTeam = gameObjects.FindAll(s => s.Contains("yellow"));
        //Maintenant on peut comparer les pions de léquipe rouge avec ceux de l'équipe jaune
        int a =0;
        
        foreach(String pionRed in redTeam)
        {
            print("********************Evaluation*********************    " + a);
            string[] redSplit = pionRed.Split('#');
            foreach(String pionYellow in yellowTeam)
            {
                string[] yellowSplit = pionYellow.Split('#');
                //On peut retrouver les indexes des pions sur le chemin et les comparer pour savoir s'ils sont sur la même position
                int redPos = newPath.FindIndex(x=> x == redPath[int.Parse(redSplit[1])]);
                int yellowPos = newPath.FindIndex(y => y == yellowPath[int.Parse(yellowSplit[1])]);
                //Maintenant comparons
                if(yellowPos == redPos)
                {
                    //Alors nous venons de nous prendre un pion jaune dans la gueule. Mauvais plan... -100
                    score = score + 1000;
                    print("Possibilité de perdre un pion -100. Score : " + score);

                }
                if((yellowPos-redPos)<7)
                {
                    //dans ce cas, nous avons un point jaune à un tour de dé. Bon plan +200
                    score = score - 200;
                    print("Point jaune à proximité +200. Score : " + score);
                }
                if((redPos - yellowPos) < 7)
                {
                    //Pion rouge à proximité. Il faut le manger
                    score = score + 800;
                    print("Pion derrière nous +200. Score : " + score);
                }
                if(int.Parse(redSplit[1]) > 46)
                {
                    //le rouge sort de l'arène
                    score = score - 500;
                    print("Fin de l'arène +500 Score : " + score);
                }
                if (int.Parse(yellowSplit[1]) > 45)
                {
                    //La maison est là
                    score = score + 600;
                }
                if(int.Parse(redSplit[1]) < 10)
                {
                    //Trop proche de l'entrée, il faut bouger
                    score = score - 500;
                }
                // Et on peu ajouter une pincée de chance
                score = score + Random.Range(0,2000);
            }
            print("=============================== Fin Evaluation ==========================================================");
        }
        // Checking for victory
        return score;
    }

    private void LaunchMinMax(List<String> gameObjects, int depth, Boolean isMax, int diceValue)
    {
        int a = 0;
        print("================================== Lancement de MinMax ==================================================");
        foreach(String game in gameObjects)
        {
            print("pion " + a.ToString() + " :" + game.ToString());
        }
        //Creation du Root
        if(diceValue == 6)
        {
            if(redHome.Count !=0 ) // La maison n'est pas vide
            {
                bool pionOnEntry = false;
               foreach(GameObject gameObject in arena)
                {
                    string[] splitName = gameObject.name.Split('#');
                    if (splitName[1] == "0" && gameObject.tag == "red")
                    {
                        pionOnEntry = true;
                    }
                }

               //Si un pion occupe l'entrée nous ne pouvons pas sortir un nouveau pion
               if (pionOnEntry == true)
                {
                    Feuille arbre = DrawTree(gameObjects, diceValue);
                    MoveAfterMinMax(arbre,diceValue);
                }
                else
                {
                    //Pas le choix, on sort un joueur
                    MoveFromHome("red");
                }


            }
            else 
                if (redHome.Count == 0)
                {
                    //On peut jouer sur les joueurs existants. On construit l'arbre 
                    Feuille arbre = DrawTree(gameObjects, diceValue);
                    MoveAfterMinMax(arbre,diceValue);
                    
                }
        }
        else
        {
            if(redHome.Count == 4)
            {
                StartCoroutine(ShowText("Pas de déplacement possible"));
            }else
            {
                Feuille arbre = DrawTree(gameObjects, diceValue);
                MoveAfterMinMax(arbre,diceValue);
            }

        }

    }
    /*
     =================================================================================================================================
    ===================================================================================================================================
    ===================================================================================================================================
    ========================================= Deplacement des pions après construction de l'arbre =====================================
     */
    private void MoveAfterMinMax(Feuille arbre, int dice)
    {
        int i = 0;
        print(" ========================== Affichons les branches de la racine ================================================");
        print("la racine a : " + arbre.branche.Count + " branches");
        foreach(Feuille objet in arbre.branche)
        {
            print("==================================== Les branches sont : =======================================");
            foreach(String game in objet.arene)
            {
                print(game);
            }
            print("pour branche :"+i.ToString()+ "  le score est : " + objet.score);
            i++;
        }
        //On récupère la feuille avec plus le haut score
        Feuille feuille = arbre.GetMax();
        feuillesEnArene = feuille.arene;

        foreach (GameObject gameObject in arena)
        {
            if(gameObject.tag == "red")
            {
                String pionToMove = feuille.arene.Find(x => x == gameObject.name);
                if (pionToMove == null)
                {
                    print("Un pion peu être touvé dans l'arène");
                    string[] pionToMoveSplit = gameObject.name.Split('#');
                    print("La pièce recherchée s'appelle : " + gameObject.name);
                    String item = feuille.arene.Find(y => y.Contains(pionToMoveSplit[0]) == true);
                    print(item);
                    if (item == null)
                    {
                        print("Object non trouvé");
                    }
                    else
                    {
                        print("Deplacement possible");
                        print("Destination : " + item);
                        string[] splitNewPos = item.Split('#');
                        //Maintenant il suffit de déplacer le pion
                        int newPos = int.Parse(splitNewPos[1]);
                        if(newPos == 51)
                        {
                            //Le joueur est arrivé
                            MoveAPlayer(gameObject, newPos, "red");
                            StartCoroutine(ShowText("Joueur arrivé"));
                            arena.Remove(gameObject);
                            Destruction(gameObject);
                            break;
                        }
                        else
                        {
                            //Deplacement et nouveau nom
                            MoveAPlayer(gameObject, newPos, "red");
                            break;
                        }
                        
                    }
                }
            }

        }
    }

    private void MoveFromHome(string tag)
    {
        if(tag == "red")
        {
            GameObject selectedPlayer = redHome[0];

            Vector3 positionToMove = new Vector3(redPath[0].transform.position.x, redPath[0].transform.position.y + 0.028f, redPath[0].transform.position.z);
            selectedPlayer.transform.position = Vector3.MoveTowards(selectedPlayer.transform.position, positionToMove, 200 * Time.deltaTime);
            selectedPlayer.name = selectedPlayer.name + "#" + 0.ToString();
            arena.Add(selectedPlayer);
            redHome.Remove(selectedPlayer);
            GameObject playerInCase = arena.Find(x => x.transform.position == positionToMove && x.tag == "yellow");
            //Si ell est occupée on démolit le pion sur place
            if (playerInCase != null)
            {
                StartCoroutine(Destruction(playerInCase));
                StartCoroutine(ShowText("Joueur Détruit"));
            }
        }
        if (tag == "yellow")
        {
            GameObject selectedPlayer = yellowHome[0];

            Vector3 positionToMove = new Vector3(yellowPath[0].transform.position.x, yellowPath[0].transform.position.y + 0.028f, yellowPath[0].transform.position.z);
            selectedPlayer.transform.position = Vector3.MoveTowards(selectedPlayer.transform.position, positionToMove, 200 * Time.deltaTime);
            selectedPlayer.name = selectedPlayer.name + "#" + 0.ToString();
            arena.Add(selectedPlayer);
            yellowHome.Remove(selectedPlayer);
        }
    }
    private void MoveFromHome(string tag, GameObject player)
    {
        if (tag == "red")
        {
            GameObject selectedPlayer = redHome[0];

            Vector3 positionToMove = new Vector3(redPath[0].transform.position.x, redPath[0].transform.position.y + 0.028f, redPath[0].transform.position.z);
            selectedPlayer.transform.position = Vector3.MoveTowards(selectedPlayer.transform.position, positionToMove, 200 * Time.deltaTime);
            selectedPlayer.name = selectedPlayer.name + "#" + 0.ToString();
            arena.Add(selectedPlayer);
            redHome.Remove(selectedPlayer);
            //On vérifie si la case destination est occupée
            GameObject playerInCase = arena.Find(x => x.transform.position == positionToMove && x.tag == "yellow");
            //Si ell est occupée on démolit le pion sur place
            if (playerInCase != null)
            {
                StartCoroutine(Destruction(playerInCase));
                StartCoroutine(ShowText("Joueur Détruit"));
            }
        }
        if (tag == "yellow")
        {
            GameObject selectedPlayer = player;

            Vector3 positionToMove = new Vector3(yellowPath[0].transform.position.x, yellowPath[0].transform.position.y + 0.028f, yellowPath[0].transform.position.z);
            selectedPlayer.transform.position = Vector3.MoveTowards(selectedPlayer.transform.position, positionToMove, 200 * Time.deltaTime);
            selectedPlayer.name = selectedPlayer.name + "#" + 0.ToString();
            arena.Add(selectedPlayer);
            yellowHome.Remove(selectedPlayer);
            //On vérifie si la case destination est occupée
            GameObject playerInCase = arena.Find(x => x.transform.position == positionToMove && x.tag == "red");
            //Si ell est occupée on démolit le pion sur place
            if (playerInCase != null)
            {
                StartCoroutine(Destruction(playerInCase));
                StartCoroutine(ShowText("Joueur Détruit"));
            }
        }
    }

    /*
* ******************************************************************************************************************************
* *****************************************************************************************************************************
* *******************************************************************************************************************************
* ******************************************************************************************************************************
* ********************************************* Dessin de l'arbre ***************************************************************
*/
    private Feuille DrawTree(List<String> state, int dice)
    {
        Feuille root = new Feuille();
        root.SetValues(state); // La liste de gameobjects dans le root est egale à l'état du jeu actuel
        root.SetRoot();

        print("============================== Creation de l'arbre ========================================");
        /*=====================================   MAX   ===========================================================================*/
        //root créee
        //Pour la valeur du dé, nous déplaçons chaque joueur rouge

        List<String> redInList = new List<String>(root.arene.FindAll(x => x.Contains("red")));
        
        //Pour chacune des pièces rouges de la racine, je l'incrémente de la valeur du dé
        foreach(String red in redInList)
        {
            //On crée une liste temporaire égale à l'état
            List<String> cache = new List<String>(state);
            //On crée un pion temporaire (Instanciate)
            String tempRed = red;
            //tempRed.name = red.name;
            //On split son nom
            string[] redName = red.Split('#');
            int oldPosition = int.Parse(redName[1]);
            int newPosition = int.Parse(redName[1]) + dice;
            //On bloque les mouvements qui ne peuvent pas se faire
            if(newPosition < 51)
            {
                
                tempRed= redName[0] + "#" + newPosition.ToString();
                //On recherche dans cette liste temporaire, un pion rouge 
                String redInCache = cache.Find(y => y == red);
                //On le supprime
                cache.Remove(redInCache);
                //Et on Ajoute le nouveau
                cache.Add(tempRed);
                //Et nous l'ajoutons à une nouvelle feuille
                Feuille feuille = new Feuille();
                feuille.SetValues(cache);
                root.AddChild(feuille);
            }

            
        }
        print("===================================== Les noeuds sont ==========================================");
        int b = 0;
        foreach(Feuille noeud in root.branche)
        {
            print("Noeud : " + b);
            foreach(String gameObject in noeud.arene)
            {
                print("pion :" + gameObject);
            }
        }
       
        /*========================================       MIN   ========================================================================*/

        //Pour la valeur Max, nous devons prendre les feuilles de chaque branche du root, et ajouter la valeur du dé aux pion rouges.

        foreach(Feuille feuille1 in root.branche)
        {
            for(int i = 0; i <7;i++)
            {
                //Pour ne pas modifier l'état général, on crée une nouvelle liste
                List<String> temp = new List<String>();
                foreach (String pion in state)
                {
                    if (pion.Contains("yellow"))
                    {
                        string pionTemp = pion;
                        //pionTemp.name = pion.name;
                        //Si le pion est jaune alors on le déplace de i cases et on ajoute l'état
                        string[] split = pionTemp.Split('#');
                        int newPos = int.Parse(split[1]) + i;
                        pionTemp = split[0] + "#" + newPos.ToString();
                        //On ajoute le nouveau nom à la liste
                        temp.Add(pionTemp);
                    }
                    if (pion.Contains("red"))
                    {
                        String pionTemp = pion;
                        pionTemp = pion;
                        temp.Add(pionTemp);
                    }
                }
                Feuille feuille = new Feuille();
                feuille.SetValues(temp, Evaluate(temp));
                feuille1.AddChild(feuille);
                AddScoreToParent(feuille, Evaluate(temp));
            }
        }

        /*
         * Affichage des scores des différents scores de branches
         */
        print("============================================== Affichage des scores des branches ============================");
        foreach (Feuille noeud in root.branche)
        {
            print("Le score de la branche " + b + " est : " + noeud.score.ToString());
        }


        //On retourne l'arbre dessiné
        return root;
    }

    private void AddScoreToParent(Feuille feuille, int v)
    {
        if (feuille.parent.score == 0)
        {
            feuille.parent.score = v;
            //feuille.parent.SetValues(feuille.arene);
        }else
            if(v < feuille.parent.score)
            {
                feuille.parent.score = v;
                //feuille.parent.SetValues(feuille.arene);
            }
        
    }


    //================================================= UPDATE  ========================================================
    private void Update()
    {
        if(Input.GetKeyDown("x") == true)
        {
            playGame();
        }
    }
}
