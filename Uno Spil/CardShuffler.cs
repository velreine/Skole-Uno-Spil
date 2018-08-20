using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno_Spil
{
    public static class CardShuffler
    {




        //Durstenfield Algorithm:
        public static List<Card> Shuffle(List<Card> shuflee)
        {
            var results = new List<Card>(shuflee.Capacity);
            var rng = new Random(DateTime.Now.Millisecond);

            var numLoops = shuflee.Count;

            for (int index = 0; index < numLoops; index++)
            {
                // Generate random number from 1 to N (N = Number of elemenets remaining in shuflee)
                int random = rng.Next(1, shuflee.Count);

                // Allocate a buffer for our randomly chosen index.
                var buffer = shuflee[random - 1];

                // Place last element at randomly chosen index.
                shuflee[random - 1] = shuflee[shuflee.Count - 1];
                // Remove the last element at list now.
                shuflee.RemoveAt((shuflee.Count - 1));
                // Now add buffered (selected) item to results.
                results.Add(buffer);
            }

            // Return the shuffled list:
            return results;
        }










    }
}
