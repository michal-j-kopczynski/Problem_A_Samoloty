namespace airplanes_logic
{
    public class AirplaneScheduler
    {
        private List<Tuple<double, double>> airplanes;

        public void LoadData(System.IO.StreamReader tr)
        {
            airplanes = new List<Tuple<double, double>>();

            try
            {
                string line = tr.ReadLine();
                int pairsToRead = int.Parse(line);

                if (pairsToRead == 0)
                {
                    Console.WriteLine("Natrafiono 0. Czytanie przedziałów zakończone.");
                    Environment.Exit(0);
                }

                for (int i = 0; i < pairsToRead; i++)
                {
                    line = tr.ReadLine();
                    string[] parts = line.Split(' ');
                    double firstValue = double.Parse(parts[0]);
                    double secondValue = double.Parse(parts[1]);

                    airplanes.Add(Tuple.Create(firstValue, secondValue));
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Czytanie pliku nieudane: " + ex.Message);
            }
        }

        public double Solve()
        {
            double bestWindowSize = -1;
            int n = airplanes.Count;


            var orderPermutations = GetPermutations(Enumerable.Range(0, n), n);


            foreach (var selectedOrder in orderPermutations)
            {
                if (IsValidPermutation(selectedOrder))
                {
                    double optimalWindowSize = FindOptimalWindowSize(selectedOrder);
                    bestWindowSize = Math.Max(bestWindowSize, optimalWindowSize);
                }
            }

            return bestWindowSize;
        }

        private IEnumerable<IEnumerable<int>> GetPermutations(IEnumerable<int> items, int count)
        {
            if (count == 0)
            {
                yield return Enumerable.Empty<int>();
            }
            else
            {
                int position = 0;
                foreach (var item in items)
                {
                    var unused = items.Where((_, index) => index != position);
                    foreach (var permutation in GetPermutations(unused, count - 1))
                    {
                        yield return permutation.Prepend(item);
                    }
                    position++;
                }
            }
        }

        private double FindOptimalWindowSize(IEnumerable<int> order)
        {
            int n = airplanes.Count;


            var adjustedOrder = order.ToArray();


            bool IsFeasible(double windowSize)
            {
                double total = airplanes[adjustedOrder[0]].Item1;
                for (int i = 1; i < n; i++)
                {
                    if (total + windowSize < airplanes[adjustedOrder[i]].Item1)
                    {
                        total = airplanes[adjustedOrder[i]].Item1;
                    }
                    else if (total + windowSize > airplanes[adjustedOrder[i]].Item2)
                    {
                        return false;
                    }
                    else
                    {
                        total += windowSize;
                    }
                }
                return true;
            }


            double left = 0.0;
            double right = 1000.0;
            double tolerance = 1e-6;
            double optimalWindowSize = -1.0;

            while (left + tolerance <= right)
            {
                double mid = (left + right) / 2.0;
                if (IsFeasible(mid))
                {
                    optimalWindowSize = mid;
                    left = mid;
                }
                else
                {
                    right = mid;
                }
            }

            return optimalWindowSize;
        }

        private bool IsValidPermutation(IEnumerable<int> order)
        {
            int n = order.Count();
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {

                    if (airplanes[order.ElementAt(i)].Item1 >= airplanes[order.ElementAt(j)].Item2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }


}

