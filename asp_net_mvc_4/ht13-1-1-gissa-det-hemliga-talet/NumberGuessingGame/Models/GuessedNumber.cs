using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NumberGuessingGame.Models
{
    public enum Outcome { Indefinite, Low, High, Right, NoMoreGuesses, OldGuess }
    
    public class GuessedNumber
    {
        #region Fields

        // Value of the guessed number
        public int? Number;

        // The guessing's outcome
        public Outcome Outcome;

        #endregion 
    }
}