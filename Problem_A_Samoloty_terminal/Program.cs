class Program
{
    public static void Main(string[] args)
    {


        System.IO.StreamReader tr = new System.IO.StreamReader("dane.TXT");

        int i = 1;
        while (true)
        {

            airplanes_logic.AirplaneScheduler scheduler = new airplanes_logic.AirplaneScheduler();
            scheduler.LoadData(tr);
            double bestWindowSize = Math.Round(scheduler.Solve(), 2);
            int minutes = (int)bestWindowSize;
            int seconds = (int)((bestWindowSize - minutes) * 60);

            string formattedTime = $"Case {i}: {minutes}:{seconds:D2}";

            Console.WriteLine(formattedTime);

            i++;
        }

    }
}