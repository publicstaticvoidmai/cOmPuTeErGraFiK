using System.Linq;
using Models.Player;
using UnityEngine;

namespace Models.Board
{
    public class Tile : MonoBehaviour
    {
        //When the mouse hovers over the GameObject, it turns to this color (red)
        Color _MouseOverColor = Color.red;

        private int _x;
        private int _z;

        Color _originalColor;
        MeshRenderer _renderer;
        
        public void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            _originalColor = _renderer.material.color;
            var position = transform.position;
            _x = (int) position.x;
            _z = (int) position.z;
        }
        
        public void OnMouseDown()
        {
            if (IsPlayable()) ((HumanPlayer) Game.Instance.CurrentPlayer).SetNextMove(_x, _z);
        }

        public void OnMouseOver()
        {
            if (IsPlayable()) _renderer.material.color = _MouseOverColor;
        }

        public void OnMouseExit()
        {
            if (IsPlayable()) _renderer.material.color = _originalColor;
        }

        private bool IsPlayable()
        {
            IPlayer player = Game.Instance.CurrentPlayer;
            if (player is HumanPlayer)
            {
                return player
                    .GetPotentialMoves()
                    .Any(move => move.Origin.X == _x && move.Origin.Z == _z);
            }

            return false;
        }
    }
}