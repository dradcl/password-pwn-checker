//
// Check if your password was found in a breached database.
// Idea from https://www.youtube.com/watch?v=hhUb5iknVJs
//

using System;
using System.Security.Cryptography;
using System.Text;
using RestSharp;

namespace PassCheck
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string password, passHash, rangePass;
            string[] possibleHashes;
            bool matchWasFound = false;

            Console.WriteLine("Enter Password: ");
            password = Console.ReadLine();

            // API takes a SHA1 hashed password to search.
            SHA1 hash = SHA1.Create();
            byte[] hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            passHash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

            // Save the first 5 characters of the hash to send to the API
            rangePass = passHash.Remove(5);

            RestClient restClient = new RestClient("https://api.pwnedpasswords.com");
            IRestRequest restRequest = new RestRequest($"range/{rangePass}");
            IRestResponse restResponse = restClient.Get(restRequest);

            // API response is split by line breaks, so let's turn it into an array that way.
            possibleHashes = restResponse.Content.Split(Environment.NewLine);

            foreach (string hashEntry in possibleHashes)
            {
                // The API removes the first 5 characters, so lets reconstruct it for comparison.
                string completeHash = rangePass + hashEntry;

                // The first 40 characters contains the password hash, the rest is an appearances count.
                if (completeHash.Remove(40) == passHash)
                {
                    string numAppearances = completeHash.Substring(41);
                    matchWasFound = true;
                    Console.WriteLine($"\nMatched {completeHash.Remove(40)}\nAppearances: {numAppearances}");
                }
            }

            if (!matchWasFound)
            {
                Console.WriteLine("No matches found! (PS: This is a good thing)");
            }


            Console.ReadKey();
        }
    }
}