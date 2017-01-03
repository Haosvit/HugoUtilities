using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WordScrambler.Models;

namespace WordScrambler.Controllers
{
    public class WordScramblerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Scramble(string wordsInput, bool toCapsOriginal = false, bool toCapsResult = false, bool scrambleSentence = false, int fromLeft = 0, int fromRight = 0)
		{
			var startTime = DateTime.Now;
			string[] tokens = wordsInput.Split(new[] { "\r\n", " ", ",", ".", "?", "!", ";", ":" }, StringSplitOptions.RemoveEmptyEntries);
			
			List<Word> words = new List<Word>();

			foreach (string line in tokens)
			{
				var scrambledWord = GetScrambledWord(line, fromLeft, fromRight);

				words.Add(new Word
				{
					Origin = toCapsOriginal ? line.ToUpper() : line,
					Scrambled = toCapsResult ? scrambledWord.ToUpper() : scrambledWord
				});
			}
			var operatedTime = TimeSpan.FromTicks(DateTime.Now.Ticks - startTime.Ticks);
			ViewBag.OperatedTime = string.Format("{0:c}", operatedTime);
			ViewBag.IsScrambleSentence = scrambleSentence;
		    return View("Result", words);
	    }

	    private string GetScrambledWord(string word, int fromLeft, int fromRight)
	    {
		    if ((word.Length < 2) || (fromLeft < 0) || (fromRight < 0) || (fromLeft + fromRight > word.Length))
		    {
			    return word;
		    }

		    HashSet<int> newPositionIndices;
		    Random rand = new Random();
		    StringBuilder sb;
		    var right = word.Length - fromRight - 1;
		    var scrambleRange = word.Length - fromLeft - fromRight;
		    if (scrambleRange < 2)
		    {
			    return word;
		    }

            do
            {
                sb = new StringBuilder();
                newPositionIndices = new HashSet<int>();

                while (newPositionIndices.Count != scrambleRange)
                {
                    newPositionIndices.Add(rand.Next(fromLeft, right + 1));
                }

            } while (!IsNewPositionDifferent(newPositionIndices));

            sb.Append(word.Substring(0, fromLeft));

            foreach (int i in newPositionIndices)
            {
                sb.Append(word[i]);
            }

            sb.Append(word.Substring(word.Length - fromRight, fromRight));

            return sb.ToString();
		}

        private bool IsNewPositionDifferent(HashSet<int> newPositionIndices)
        {
            var testPositionArray = new List<int>(newPositionIndices);

            testPositionArray.OrderBy(x => x);

            if (testPositionArray.SequenceEqual(newPositionIndices))
            {
                return true;
            }
            return false;
        }
    }
}