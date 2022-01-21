using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Feuille
    {
        public List<String> arene;
        public int score;
        public List<Feuille> branche;
        public int scoreBranche;
        public bool isRoot;
        public Feuille parent;
        public int niveau;
        List<Feuille> tree;
        public bool firstLaunch;

        public Feuille()
        {
            score = 0;
            branche = new List<Feuille>();
        }

        //Fontion qui renvoie le score
        public int GetScore(Feuille F)
        {
            return F.score;
        }
        //Fonction qui renvoie la valeur
        
        
        //Fonction qui peuple une feuille
        public void SetValues(List<String> A)
        {
            arene = A;
        }
        public void SetValues(List<String> A, int S)
        {
            arene = A;
            score = S;
        }
        //Ajouter une feuille. Gardons à l'esprit qu'une feuille peut contenir une liste de feuilles pour faire une branche
        //Et que la première feuille est une racine
        public void AddChild(Feuille F)
        {
            F.parent = this; //A la création d'un enfant, on assigne un parent. Nous retrouverons plus facilement le chemin de la feuille
            branche.Add(F);
        }
        public void SetRoot()
        {
            isRoot = true;
        }

        //Renvoyer tous les enfants d'une feuille
        public List<Feuille> GetAllChildren(Feuille F)
        {
            return F.branche;
        }

        public Feuille GetParent(Feuille F)
        {
            return F.parent;
        }

        //Affiche de l'arbre
        public List<String> ShowTree()
        {
            List<String> result = new List<string>();
            foreach (Feuille A in branche)
            {
                List<String> gameObjects = new List<String>();
                gameObjects = A.arene;
                foreach(String gameObject in gameObjects)
                {
                    result.Add(gameObject);
                }
                if(A.branche.Count > 0)
                {
                    foreach(Feuille B in A.branche)
                    {
                        List<String> gameObjects2 = B.arene;
                        foreach(String gameObject2 in gameObjects2)
                        {
                            result.Add(gameObject2);
                        }
                    }
                }
            }
            return result;
        }

        public Feuille GetMax()
        {
            // Renvoie la branche avec le plus grand score
            Feuille max = new Feuille();
            foreach(Feuille feuille in branche)
            {
                if(max.score == 0)
                {
                    max = feuille;
                }else
                    if(max.score < feuille.score)
                {
                    max = feuille;
                }
            }
            return max;
        }

    }
}
