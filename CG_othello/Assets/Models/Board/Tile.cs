using System;
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
        
        public void Start()
        {
            _Renderer = GetComponent<MeshRenderer>();
            _OriginalColor = _Renderer.material.color;
            var position = transform.position;
            _x = (int) position.x;
            _z = (int) position.z;
        }
        
        public void OnMouseDown()
        {
            if (IsPlayable()) Game.Instance.GetCurrentPlayer().SetNextMove(_x, _z);
        }

        public void OnMouseOver()
        {
            if (IsPlayable()) _Renderer.material.color = _MouseOverColor;
        }

        public void OnMouseExit()
        {
            if (IsPlayable()) _Renderer.material.color = _OriginalColor;
        }

        private bool IsPlayable()
        {
            // TODO Check if field is occupado
            List<Move> valid = FilteredForCurrentPlayer(Game.Instance.ValidMoves);
            LogicalPiece potential = new LogicalPiece(_x, _z, Game.Instance.GetCurrentColor());
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
            return piece.X == _x && piece.Z == _z;
        }
    }
}