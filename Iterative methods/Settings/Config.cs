namespace Application;

public static class Config
{
    public static double Sigma = 1.0;
    public static double Eps = 1e-10;


    public static double From = 0.0;
    public static double To = 10.0;

    public static double Fromtime = 0.0;
    public static double Totime = 10.0;
    public static int SplitsTimeNumber = 4;

    public static int SplitsNumber = 4;

    public static double fun(double x)
    {
        return 2*x+1;
    }

    public static double lambda(double x)
    {
        return 2 * x + 1;
    }

}