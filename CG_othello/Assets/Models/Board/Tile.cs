using System.Collections.Generic;
using UnityEngine;

namespace Models.Board
{
    public class Tile : MonoBehaviour
    {
        //When the mouse hovers over the GameObject, it turns to this color (red)
        Color _MouseOverColor = Color.green;

        private int _x;
        private int _z;

        Color _OriginalColor;
        MeshRenderer _Renderer;
        
        void Start()
        {
            _Renderer = GetComponent<MeshRenderer>();
            _OriginalColor = _Renderer.material.color;
            var position = transform.position;
            _x = (int) position.x;
            _z = (int) position.z;
        }
        
        private void OnMouseDown()
        {
            if (IsPlayable()) Game.Instance.white.SetNextMove(_x, _z);
        }

        void OnMouseOver()
        { 
            if (IsPlayable()) _Renderer.material.color = _MouseOverColor;
        }

        void OnMouseExit()
        {
            if (IsPlayable()) _Renderer.material.color = _OriginalColor;
        }

        private bool IsPlayable()
        {
            if (Game.Instance.State[_x, _z] == null) return false;
            
            List<Move> valid = FilteredForCurrentPlayer(Game.Instance.ValidMoves);
            
            Piece potential = new Piece(_x, _z, Game.Instance.GetCurrentColor());
            return valid.Find(move => move.Origin.Equals(potential)) != null;
        }

        private static List<Move> FilteredForCurrentPlayer(List<Move> moves)
        {
            PlayerColor currentColor = Game.Instance.GetCurrentColor();
            return moves
                .FindAll(move => move.Origin.Color.Equals(currentColor));
        }

        private bool IsPlacedOnThis(Piece piece)
        {
            return piece.x == _x && piece.z == _z;
        }
    }
}