using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberGuessingGame.Models;

namespace NumberGuessingGame.Tests
{
    /// <summary>
    /// Summary description for SecretNumberUnitTest.
    /// </summary>
    [TestClass]
    public class SecretNumberUnitTest
    {
        [TestMethod]
        public void CheckConstructor()
        {
            var number = (int)GetFieldValue(new SecretNumber(), "_number");
            Assert.IsTrue(number >= 1 && number <= 100, "_number är inte ett tal i det slutna intervallet mellan 1 och 100.");

            var guessedNumbers = (List<GuessedNumber>)GetFieldValue(new SecretNumber(), "_guessedNumbers");
            Assert.IsNotNull(guessedNumbers, "_guessedNumbers har inte initierats.");
            Assert.IsTrue(guessedNumbers.Capacity == 7, "_guessedNumbers är inte förberedd för sju element (Capacity ger inte värdet 7).");
        }

        [TestMethod]
        public void CheckInitialize()
        {
            var sn = new SecretNumber();
            int number;
            do
            {
                sn.Initialize();
                number = (int)GetFieldValue(sn, "_number");
                Assert.IsTrue(number >= 1 && number <= 100, "_number är inte ett tal i det slutna intervallet mellan 1 och 100.");
            } while (number >= 94);

            for (int i = 94; i <= 100; i++)
            {
                sn.MakeGuess(i);
            }
            sn.Initialize();

            Assert.IsTrue(sn.GuessedNumbers.Count == 0, "Listan med gamla gissningar töms inte.");
            Assert.IsTrue(sn.Count == 0, "Count är inte till 0.");
            Assert.IsNull(sn.LastGuessedNumber.Number, "LastGuessedNumber.Number är inte satt till null.");
            Assert.IsTrue(sn.LastGuessedNumber.Outcome == Outcome.Indefinite, "LastGuessedNumber.Outcome är inte satt till Outcome.Indefinite.");
        }

        [TestMethod]
        public void CheckMakeGuessLow()
        {
            var sn = new SecretNumber();
            int number, prevNumber, loopCount = 5;
            do
            {
                prevNumber = (int)GetFieldValue(sn, "_number");
                Thread.Sleep(100);
                sn.Initialize();
                number = (int)GetFieldValue(sn, "_number");
                Assert.IsTrue(number >= 1 && number <= 100, "Initialize(): _number är inte ett tal i det slutna intervallet mellan 1 och 100.");
                if (--loopCount == 0)
                {
                    Assert.IsTrue(prevNumber != number, "Initialize(): _number verkar inte slumpas");
                    return;
                }
            } while (number < 2);
            var result = sn.MakeGuess(1);
            Assert.IsTrue(result == Outcome.Low, "MakeGuess returnerar inte Outcome.Low då det gissade talet är mindre än det hemliga talet.");
            Assert.IsTrue(sn.LastGuessedNumber.Outcome == Outcome.Low, "Egenskapen LastGuessedNumber egenskap Outcome är inte Outcome.Low efter en gissning på ett för litet tal gjorts.");
        }

        [TestMethod]
        public void CheckMakeGuessHigh()
        {
            var sn = new SecretNumber();
            int number, prevNumber, loopCount = 5;
            do
            {
                prevNumber = (int)GetFieldValue(sn, "_number");
                Thread.Sleep(100);
                sn.Initialize();
                number = (int)GetFieldValue(sn, "_number");
                Assert.IsTrue(number >= 1 && number <= 100, "Initialize(): _number är inte ett tal i det slutna intervallet mellan 1 och 100.");
                if (--loopCount == 0)
                {
                    Assert.IsTrue(prevNumber != number, "Initialize(): _number verkar inte slumpas");
                    return;
                }
            } while (number > 99);
            var result = sn.MakeGuess(100);
            Assert.IsTrue(result == Outcome.High, "MakeGuess returnerar inte Outcome.High då det gissade talet är högre än det hemliga talet.");
            Assert.IsTrue(sn.LastGuessedNumber.Outcome == Outcome.High, "Egenskapen LastGuessedNumber egenskap Outcome är inte Outcome.High efter en gissning på ett för stort tal gjorts.");
        }

        [TestMethod]
        public void CheckMakeGuessRight()
        {
            var sn = new SecretNumber();
            var guess = (int)GetFieldValue(sn, "_number");
            Assert.IsTrue(sn.MakeGuess(guess) == Outcome.Right, "MakeGuess returnerar inte Outcome.Right då det gissade talet är samma som det hemliga talet.");
            Assert.IsTrue(sn.LastGuessedNumber.Outcome == Outcome.Right, "Egenskapen LastGuessedNumber egenskap Outcome är inte Outcome.Right efter en gissning på rätt tal gjorts.");
        }

        [TestMethod]
        public void CheckMakeGuessOldGuess()
        {
            var sn = new SecretNumber();
            var guess = Math.Max((int)GetFieldValue(sn, "_number") ^ 3, 1);
            sn.MakeGuess(guess);
            guess = Math.Max((int)GetFieldValue(sn, "_number") ^ 3, 1);
            Assert.IsTrue(sn.MakeGuess(guess) == Outcome.OldGuess, "MakeGuess returnerar inte Outcome.OldGuess då det gissade talet är samma som en tidigare gissning.");
            //Assert.IsTrue(sn.LastGuessedNumber.Outcome == Outcome.OldGuess, "Egenskapen LastGuessedNumber egenskap Outcome är inte Outcome.OldGuess då det gissade talet är samma som en tidigare gissning.");
        }

        [TestMethod]
        public void CheckMakeGuesskNoMoreGuesses()
        {
            var sn = new SecretNumber();
            var number = (int)GetFieldValue(sn, "_number");
            while (number >= 94)
            {
                sn.Initialize();
                number = (int)GetFieldValue(sn, "_number");
            }
            for (int i = 94; i <= 100; i++)
            {
                sn.MakeGuess(i);
            }
            var result = sn.MakeGuess(number);
            Assert.IsTrue(result == Outcome.NoMoreGuesses, "MakeGuess returnerar inte Outcome.NoMoreGuesses då fler gissningar än sju görs.");
            //Assert.IsTrue(sn.LastGuessedNumber.Outcome == Outcome.NoMoreGuesses, "Egenskapen LastGuessedNumbers egenskap Outcome är inte Outcome.NoMoreGuesses då fler gissningar än sju görs.");
        }

        [TestMethod]
        public void CheckCanMakeGuessProperty()
        {
            var sn = new SecretNumber();
            var number = (int)GetFieldValue(sn, "_number");
            while (number >= 94)
            {
                sn.Initialize();
                number = (int)GetFieldValue(sn, "_number");
            }
            for (int i = 94; i <= 100; i++)
            {
                Assert.IsTrue(sn.CanMakeGuess, "Egenskapen CanMakeGuess är inte true trots att alla gissningar inte gjorts.");
                sn.MakeGuess(i);
            }
            Assert.IsFalse(sn.CanMakeGuess, "Egenskapen CanMakeGuess är inte false trots att sju gissningar gjorts.");

            sn.Initialize();
            number = (int)GetFieldValue(sn, "_number");
            sn.MakeGuess(number);
            Assert.IsFalse(sn.CanMakeGuess, "Egenskapen CanMakeGuess är inte false trots att en gissning på rätt tal gjorts.");
        }

        [TestMethod]
        public void CheckMakeGuessArgumentOfRangeExcceptionIfGuessLowerThan1()
        {
            var sn = new SecretNumber();
            try
            {
                sn.MakeGuess(0);
                throw new ApplicationException();
            }
            catch (ArgumentOutOfRangeException)
            {
                // Eat!
                return;
            }
            catch
            {
                Assert.Fail("ArgumentOutOfRangeException kastas inte vid gissning på ett tal mindre än 1.");
            }
        }

        [TestMethod]
        public void CheckMakeGuessArgumentOfRangeExcceptionIfGuessGreaterThan100()
        {
            var sn = new SecretNumber();
            try
            {
                sn.MakeGuess(101);
                throw new ApplicationException();
            }
            catch (ArgumentOutOfRangeException)
            {
                // Eat!
                return;
            }
            catch
            {
                Assert.Fail("ArgumentOutOfRangeException kastas inte vid gissning på ett tal större än 100.");
            }
        }

        [TestMethod]
        public void CheckCountProperty()
        {
            var sn = new SecretNumber();
            var number = (int)GetFieldValue(sn, "_number");
            while (number >= 94)
            {
                sn.Initialize();
                number = (int)GetFieldValue(sn, "_number");
            }
            Assert.IsTrue(sn.Count == 0, "Egenskapen Count är inte 0 trots att ingen gissning gjorts.");
            int count = 0;
            for (int i = 94; i <= 100; i++)
            {
                sn.MakeGuess(i);
                Assert.IsTrue(sn.Count == ++count, String.Format("Egenskapen Count är inte {0} trots att {0} gissning(ar) gjorts.", count));
            }
            sn.MakeGuess(number);
            Assert.IsTrue(sn.Count == 7, "Egenskapen Count slutar inte att räknas upp efter att sju gissningar gjorts och ytterligare gissningar görs.");
        }

        [TestMethod]
        public void CheckLastGuessedNumberProperty()
        {
            var sn = new SecretNumber();
            var number = (int)GetFieldValue(sn, "_number");
            while (number >= 94)
            {
                sn.Initialize();
                number = (int)GetFieldValue(sn, "_number");
            }
            Assert.IsNull(sn.LastGuessedNumber.Number, "Egenskapen LastGuessedNumbers egenskap Guess är inte null trots att ingen gissning gjorts.");
            sn.MakeGuess(number - 1);
            Assert.IsTrue(sn.LastGuessedNumber.Number == number - 1, "Egenskapen LastGuessedNumbers egenskap Guess har inte värdet av den senaste gissningen då en gissning på ett för lågt tal gjorts.");
            sn.MakeGuess(number + 1);
            Assert.IsTrue(sn.LastGuessedNumber.Number == number + 1, "Egenskapen LastGuessedNumbers egenskap Guess har inte värdet av den senaste gissningen då en gissning på ett för högt tal gjorts.");
            sn.MakeGuess(number);
            Assert.IsTrue(sn.LastGuessedNumber.Number == number, "Egenskapen LastGuessedNumbers egenskap Guess har inte värdet av den senaste gissningen då en gissning på rätt tal gjorts.");
        }

        [TestMethod]
        public void CheckGuessedNumberProperty()
        {
            var sn = new SecretNumber();
            var guessedNumbersField = (IList<GuessedNumber>)GetFieldValue(sn, "_guessedNumbers");
            Assert.AreNotSame(guessedNumbersField, sn.GuessedNumbers, "Privacy leak!!! En kopia av referensen till det privata List-objektet returneras av egenskapen GuessedNumbers.");
        }

        [TestMethod]
        public void CheckNumberProperty()
        {
            var sn = new SecretNumber();
            Assert.IsNull(sn.Number, "Egenskapen Number returnerar inte null trots att det finns gissningar kvar.");
            var number = (int)GetFieldValue(sn, "_number");
            while (number >= 94)
            {
                sn.Initialize();
                number = (int)GetFieldValue(sn, "_number");
            }
            for (int i = 94; i <= 100; i++)
            {
                sn.MakeGuess(i);
            }
            Assert.IsTrue(sn.Number == number, "Egenskapen Number har inte samma värde som fältet _number trots att det inte finns några gissningar kvar.");
        }

        [TestMethod]
        public void CheckConstant()
        {
            Assert.IsTrue(SecretNumber.MaxNumberOfGuesses == 7, "Konstanten MaxNUmberOfGuesses är inte tilldelad värdet 7.");
        }

        private static object GetFieldValue(object o, string name)
        {
            var field = o.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                throw new ApplicationException(String.Format("FEL! Det privata fältet {0} saknas.", name));
            }
            return field.GetValue(o);
        }
    }
}
