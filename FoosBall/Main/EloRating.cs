using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoosBall.Main
{
    public class EloRating
    {
        public EloRating(double winnerRating, double loserRating)
        {
            this._winnerExpectedScore = 1 / (1 + Math.Pow(10, (loserRating - winnerRating) / 400));

            this._ratingModifier = (KModifier * (1 - this._winnerExpectedScore));
        }

        private const int KModifier = 35;
        private readonly double _winnerExpectedScore;
        private readonly double _ratingModifier;

        public double GetRatingModifier
        {
            get { return this._ratingModifier; }
        }
    }
    public static class Rating
    {
        private const int KModifier = 35;

        public static double GetRatingModifier(double winnerRating, double loserRating)
        {
            var winnerExpectedScore = 1 / (1 + Math.Pow(10, (loserRating - winnerRating) / 400));
            return (KModifier * (1 - winnerExpectedScore));
        }
    }
}