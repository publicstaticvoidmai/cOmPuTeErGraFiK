using System;
using UnityEngine;

namespace Models
{
    
    public class Board : MonoBehaviour
    {

        public Piece[,] pieces = new Piece[8, 8];
        public GameObject whitePref;
        public GameObject blackPref;

        private void Start()
        {
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            GeneratePiece(0,0);
            GeneratePiece(1,0);
        }

        private void GeneratePiece(int x, int z)
        {
            GameObject go = Instantiate(whitePref) as GameObject;
            go.transform.SetParent(transform);
            Piece p = go.GetComponent<Piece>();
            pieces[x, z] = p;
            MovePiece(p, x, z);

        }

        private void MovePiece(Piece p, int x, int z)
        {
            p.transform.position = new Vector3(x, 0, z);

        }

        private void OnMouseOver()
        {
            //If your mouse hovers over the GameObject with the script attached, output this message
            Debug.Log("Mouse is over GameObject.");
        }

        private void OnMouseExit()
        {
            //The mouse is no longer hovering over the GameObject so output this message each frame
            Debug.Log("Mouse is no longer on GameObject.");
        }
    }
}