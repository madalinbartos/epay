namespace epay_denomination_routine
{
    class Program
    {
        static void Main()
        {
            int[] denominations = { 10, 50, 100 };
            int[] amounts = { 30, 50, 60, 80, 140, 230, 370, 610, 980 };

            foreach (int amount in amounts)
            {
                Console.WriteLine($"Possible combinations for {amount} EUR:");

                List<List<int>> combinations = CalculateCombinations(amount, denominations);

                foreach (List<int> combination in combinations)
                {
                    Console.WriteLine(FormatCombination(combination));
                }

                Console.WriteLine();
            }
        }

        static List<List<int>> CalculateCombinations(int amount, int[] denominations)
        {
            List<List<int>> result = new List<List<int>>();
            CalculateCombinationsRecursive(amount, denominations, 0, new List<int>(), result);
            return result;
        }

        static void CalculateCombinationsRecursive(int amount, int[] denominations, int startIndex, List<int> currentCombination, List<List<int>> result)
        {
            if (amount == 0)
            {
                result.Add(new List<int>(currentCombination));
                return;
            }

            if (amount < 0)
                return;

            for (int i = startIndex; i < denominations.Length; i++)
            {
                currentCombination.Add(denominations[i]);
                CalculateCombinationsRecursive(amount - denominations[i], denominations, i, currentCombination, result);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        static string FormatCombination(List<int> combination)
        {
            Dictionary<int, int> count = new Dictionary<int, int>();
            foreach (int denomination in combination)
            {
                if (count.ContainsKey(denomination))
                    count[denomination]++;
                else
                    count[denomination] = 1;
            }

            List<string> formattedParts = new List<string>();
            foreach (var pair in count)
            {
                formattedParts.Add($"{pair.Value} x {pair.Key} EUR");
            }

            return string.Join(" + ", formattedParts);
        }
    }
}