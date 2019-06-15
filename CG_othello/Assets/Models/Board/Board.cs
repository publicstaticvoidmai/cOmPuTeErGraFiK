using UnityEngine;

namespace Models.Board
{
    public class Board : MonoBehaviour
    {
        public GameObject PlacePiece(int x, int z, GameObject pref)
        {
            return Instantiate(pref, new Vector3(x, 0, z), Quaternion.identity);
        }
        
        private void FlipPieces(int originX, int originZ, int destX, int destZ, PlayerColor color)
        {
            
        }
    }
}