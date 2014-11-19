using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NumberGuessingGame.Models
{
    [Serializable]
    public class SecretNumber
    {
        #region Fields

        private List<GuessedNumber> _guessedNumbers;
        private GuessedNumber _lastGuessedNumber;
        private int? _number;
        public const int MaxNumberOfGuesses = 7;
        public const int RandomMinValue = 1;
        public const int RandomMaxValue = 100;
        private List<string> guessHistory;

        #endregion

        #region Properties

        public bool CanMakeGuess
        {
            get
            {
                if (GuessedNumbers.Any(n => n.Outcome == Outcome.Right) ||
                    Count == MaxNumberOfGuesses)
                {
                    return false;
                }
                return true;
            }
        }

        public int Count
        {
            get { return GuessedNumbers.Count; }
        }

        public IList<GuessedNumber> GuessedNumbers
        {
            get
            {
                return _guessedNumbers.AsReadOnly();
            }
        }

        /// <summary>
        /// GuessedNumber with set Outcome
        /// </summary>
        public GuessedNumber LastGuessedNumber
        {
            get
            {
                if (GuessedNumbers.Any())
                {
                    return GuessedNumbers.Last();
                }
                else
                {
                    return new GuessedNumber();
                }
            }
        }

        public List<string> GuessHistory
        {
            get 
            {
                if (guessHistory != null)
                {
                    return guessHistory;
                }
                guessHistory = new List<string>(7);
                return guessHistory;
            }
        }

        public int? Number
        {
            get
            {
                return CanMakeGuess ? null : _number;
            }
            private set
            {
                _number = value;
            }
        }

        #endregion

        public SecretNumber()
        {
            Initialize();
            _lastGuessedNumber = new GuessedNumber();
        }

        #region Methods

        /// <summary>
        /// Clears list of guessed numbers and generates a new
        /// randomized number
        /// </summary>
        public void Initialize()
        {
            _guessedNumbers = new List<GuessedNumber>(MaxNumberOfGuesses);
            Number = GetRandomNumber();
        }

        /// <summary>
        /// Checks if the guessed number matches the secret one
        /// </summary>
        /// <param name="guess">Int guess</param>
        /// <returns>Outcome for the guessed number</returns>
        public Outcome MakeGuess(int guess)
        {
            if (guess < RandomMinValue || guess > RandomMaxValue)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (CanMakeGuess)
            {
                if (guess == _number)
                {
                    _lastGuessedNumber.Outcome = Outcome.Right;
                }

                if (guess < _number)
                {
                    _lastGuessedNumber.Outcome = Outcome.Low;
                }

                if (guess > _number)
                {
                    _lastGuessedNumber.Outcome = Outcome.High;
                }

                // The guessed number already exists...
                if (GuessedNumbers.Any(n => n.Number == guess))
                {
                    // ...therefore only the outcome is returned
                    _lastGuessedNumber.Outcome = Outcome.OldGuess;
                }
                else
                {
                    _lastGuessedNumber.Number = guess;
                    _guessedNumbers.Add(_lastGuessedNumber);
                }
            }
            else
            {
                _lastGuessedNumber.Outcome = Outcome.NoMoreGuesses;
            }

            return LastGuessedNumber.Outcome;
        }

        /// <summary>
        /// Generates a new randomized number
        /// </summary>
        /// <returns>int Randomized number</returns>
        private int GetRandomNumber()
        {
            var randomNumber = new Random();
            return randomNumber.Next(RandomMinValue, RandomMaxValue + 1);
        }

        #endregion
    }
}