string dataPath = "data.txt";
const int numPlacesInColumnAfterDividerDays = 2;
const int numPlacesInColumnAfterDividerCaculated = 3;//Max is 3 with Average as "avg" in the code to match up with the example
char underLinePattern = '-';

string DELIMETER_1 = ",";
string DELIMETER_2 = "|";

// ask for input
Console.WriteLine("Enter 1 to create data file.");
Console.WriteLine("Enter 2 to parse data.");
Console.WriteLine("Enter anything else to quit.");
// input response
string? resp = Console.ReadLine();

if (resp == "1")
{
    // create data file

    // ask a question
    Console.WriteLine("How many weeks of data is needed?");
    // input the response (convert to int)
    int weeks = int.Parse(Console.ReadLine());
    // determine start and end date
    DateTime today = DateTime.Now;
    // we want full weeks sunday - saturday
    DateTime dataEndDate = today.AddDays(-(int)today.DayOfWeek);
    // subtract # of weeks from endDate to get startDate
    DateTime dataDate = dataEndDate.AddDays(-(weeks * 7));
    // random number generator
    Random rnd = new Random();
    // create file
    StreamWriter sw = new StreamWriter(dataPath);

    // loop for the desired # of weeks
    while (dataDate < dataEndDate)
    {
        // 7 days in a week
        int[] hours = new int[7];
        for (int i = 0; i < hours.Length; i++)
        {
            // generate random number of hours slept between 4-12 (inclusive)
            hours[i] = rnd.Next(4, 13);
        }
        // M/d/yyyy,#|#|#|#|#|#|#
        // Console.WriteLine($"{dataDate:M/d/yy},{string.Join("|", hours)}");
        sw.WriteLine($"{dataDate:M/d/yyyy},{string.Join("|", hours)}");
        // add 1 week to date
        dataDate = dataDate.AddDays(7);
    }
    sw.Close();
}
else if (resp == "2")
{
    if (System.IO.File.Exists(dataPath))
    {
        StreamReader sr = new StreamReader(dataPath);

        // Caculate just once
        string weekHeader = $" {DayOfWeek.Sunday.ToString().Substring(0,numPlacesInColumnAfterDividerDays),numPlacesInColumnAfterDividerDays} {DayOfWeek.Monday.ToString().Substring(0,numPlacesInColumnAfterDividerDays),numPlacesInColumnAfterDividerDays} {DayOfWeek.Tuesday.ToString().Substring(0,numPlacesInColumnAfterDividerDays),numPlacesInColumnAfterDividerDays} {DayOfWeek.Wednesday.ToString().Substring(0,numPlacesInColumnAfterDividerDays),numPlacesInColumnAfterDividerDays} {DayOfWeek.Thursday.ToString().Substring(0,numPlacesInColumnAfterDividerDays),numPlacesInColumnAfterDividerDays} {DayOfWeek.Friday.ToString().Substring(0,numPlacesInColumnAfterDividerDays),numPlacesInColumnAfterDividerDays} {DayOfWeek.Saturday.ToString().Substring(0,numPlacesInColumnAfterDividerDays),numPlacesInColumnAfterDividerDays}";
        string calcHeader = $" {"Total".Substring(0,numPlacesInColumnAfterDividerCaculated),numPlacesInColumnAfterDividerCaculated} {"Avg".Substring(0,numPlacesInColumnAfterDividerCaculated),numPlacesInColumnAfterDividerCaculated}";
        string underLineHeader = "";
        for (int i = 0; i < 7; i++)
        {
            underLineHeader += " ";
            for (int j = 0; j < numPlacesInColumnAfterDividerDays; j++)
            {
                underLineHeader += underLinePattern;
            }
        }
        for (int i = 0; i < 2; i++)
        {
            underLineHeader += " ";
            for (int j = 0; j < numPlacesInColumnAfterDividerCaculated; j++)
            {
                underLineHeader += underLinePattern;
            }
        }
        string fullHeader = $"{weekHeader}{calcHeader}\n{underLineHeader}";

        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            DateTime date = DateTime.Parse(line.Substring(0, line.IndexOf(DELIMETER_1)));
            Console.WriteLine($"Week of {date:MMM}, {date:dd}, {date:yyyy}");


            string[] nightHours = line.Substring(line.IndexOf(DELIMETER_1) + 1).Split(DELIMETER_2);
            Console.WriteLine(fullHeader);

            //Days
            foreach (string nightHour in nightHours)
            {
                Console.Write($" {nightHour,numPlacesInColumnAfterDividerDays}");
            }
            //Caculated
            // float[] nightHoursNums = new float[nightHours.Length];
            float totalWeekNightHours = 0f;
            for (int i = 0; i < nightHours.Length; i++)
            {
                totalWeekNightHours += float.Parse(nightHours[i]);
            }

            string average = $"{(totalWeekNightHours / nightHours.Length).ToString():F10}";//TODO:Make to numPlacesInColumnAfterDividerCaculated
            Console.Write($" {totalWeekNightHours.ToString(),numPlacesInColumnAfterDividerCaculated}");

            if(average.Length > numPlacesInColumnAfterDividerCaculated){
                average = average.Substring(0,numPlacesInColumnAfterDividerCaculated);
            }

            Console.Write($" {average,numPlacesInColumnAfterDividerCaculated}");
            Console.WriteLine("\n");
        }
        sr.Close();
    }
    else
    {
        Console.WriteLine($"The file, '{dataPath}' was not found.");
    }

}
