using Advent2023;

string? line;
int sum = 0;
try
{
    StreamReader sr = new StreamReader("C:\\code\\advent2023\\day1input1.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        sum += CalibrationReader.ReadCalibrationValue(line);
        line = sr.ReadLine();
    }
    sr.Close();
    Console.WriteLine(sum);
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e.Message);
}
finally
{
    Console.WriteLine("Executing finally block.");
}
