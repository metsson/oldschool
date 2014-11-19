using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NumberGuessingGame.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NumberGuessingGame.ViewModels
{
    public class HomeIndexViewModel
    {
        #region Fields and properties

        private string sessionKey = "sessionStoredSecretNumber";

        /// <summary>
        /// Sets an instance of SecretNumber into a session variable
        /// </summary>
        public SecretNumber SecretNumber
        {
            get
            {
                var secretNumber = HttpContext.Current.Session[sessionKey] as SecretNumber;

                if (secretNumber == null)
                {
                    secretNumber = new SecretNumber();
                    HttpContext.Current.Session[sessionKey] = secretNumber;
                }
                return secretNumber;
            }
        }

        [Required(ErrorMessage = "Du måste gissa på något.")]
        [DisplayName("Gissning")]
        [Range(1, 100, ErrorMessage = "Gissa ett heltal mellan 1 och 100.")]
        public int Guess { get; set; }
        private List<string> guessHistory;
        
        /// <summary>
        /// Renders the guessed numbers by the player
        /// </summary>
        public IList<string> GuessHistory
        {
            get { return guessHistory; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear the session and start a new game
        /// </summary>
        public void Restart()
        {
            HttpContext.Current.Session.Clear();
        }

        /// <summary>
        /// Displays the current guess' value and outcome for the user in Swedish
        /// </summary>
        public string CurrentGuess(GuessedNumber number)
        {
            string swedishOutcome = "Vad blir din gissning?";
            string history = String.Empty;

            if (number.Number != null)
            {
                switch (number.Outcome)
                {
                    case Outcome.Low:
                        swedishOutcome = String.Format("Din gissning på {0} är för låg.", number.Number);
                        history = String.Format("{0} Låg :: ", number.Number);
                        break;
                    case Outcome.High:
                        swedishOutcome = String.Format("Din gissning på {0} är för hög.", number.Number);
                        history = String.Format("{0} Hög :: ", number.Number);
                        break;
                    case Outcome.Right:
                        swedishOutcome = String.Format("Din gissning på {0} är rätt!", number.Number);
                        break;
                    case Outcome.OldGuess:
                        swedishOutcome = String.Format("Du har redan gissat på {0}.", number.Number);
                        break;
                    default:
                        break;
                }

                if (number.Outcome != Outcome.OldGuess)
                {
                    SecretNumber.GuessHistory.Add(history);
                }
            }
            return swedishOutcome;
        }

        /// <summary>
        /// Returns the SecretNumber.Count in percent format
        /// </summary>
        public string GameProgress(SecretNumber game)
        {
            int percentValue = game.Count * 10;
            return percentValue.ToString();
        }


        /// <summary>
        /// Semi-observer, clear game area and shows "Congrats"-message if true
        /// </summary>
        public bool RightGuess(GuessedNumber guessedNumber)
        {
            if (guessedNumber.Outcome == Outcome.Right)
            {
                return true;
            }
            return false;
        }

        #endregion    
    }
}