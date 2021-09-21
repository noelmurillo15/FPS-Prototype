/*
 * Heart -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using System;
using UnityEngine.UI;

namespace ANM.FPS.Player
{
    public class Heart
    {
        private const int HeartPiecesPerHeart = 4;
        private const float FillPerHeartPiece = 0.25f;
        private readonly Image _image;

        public Heart(Image image)
        {
            _image = image;
        }
        
        public void Replenish(int numberOfHeartPieces)
        {
            if (numberOfHeartPieces < 0) throw new ArgumentOutOfRangeException(nameof(numberOfHeartPieces));
            _image.fillAmount += numberOfHeartPieces * FillPerHeartPiece;
        }

        public void Deplete(int numberOfHeartPieces)
        {
            if (numberOfHeartPieces < 0) throw new ArgumentOutOfRangeException(nameof(numberOfHeartPieces));
            _image.fillAmount -= numberOfHeartPieces * FillPerHeartPiece;
        }
        
        public int EmptyHeartPieces => HeartPiecesPerHeart - CalculateFilledHeartPieces();
        public int FilledHeartPieces => CalculateFilledHeartPieces();

        private int CalculateFilledHeartPieces()
        {
            return (int) (_image.fillAmount * HeartPiecesPerHeart);
        }
    }
}
