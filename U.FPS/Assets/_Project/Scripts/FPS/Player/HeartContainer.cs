/*
 * HeartContainer -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using System.Collections.Generic;
using System.Linq;

namespace ANM.FPS.Player
{
    public class HeartContainer
    {
        private readonly List<Heart> _hearts;

        public HeartContainer(List<Heart> hearts)
        {
            _hearts = hearts;
        }

        public void SetHearts(int heartPieces)
        {
            Deplete(20);
            Replenish(heartPieces);
        }

        public void Replenish(int heartPieces)
        {
            foreach (var heart in _hearts)
            {
                var toReplenish = heartPieces < heart.EmptyHeartPieces
                    ? heartPieces
                    : heart.EmptyHeartPieces;

                heartPieces -= heart.EmptyHeartPieces;
                heart.Replenish(toReplenish);
                if (heartPieces <= 0) break;
            }
        }

        public void Deplete(int heartPieces)
        {
            foreach (var heart in _hearts.AsEnumerable().Reverse())
            {
                var toDeplete = heartPieces < heart.FilledHeartPieces
                    ? heartPieces
                    : heart.FilledHeartPieces;

                heartPieces -= heart.FilledHeartPieces;
                heart.Deplete(toDeplete);
                if (heartPieces <= 0) break;
            }
        }
    }
}
