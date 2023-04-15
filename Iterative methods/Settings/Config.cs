namespace Application;

public static class Config
{
    public static double Sigma = 1.0;
    public static double Eps = 1e-10;

    public static double[,] bc1 = { { 0, 1 }, { 1, 3 } };

    public static double From = 0.0;
    public static double To = 1.0;

    public static double Fromtime = 0.0;
    public static double Totime = 3.0;
    public static int SplitsTimeNumber = 3;

    public static int SplitsNumber = 3;

    public static double fun(double x, double t)
    {
        //return x;
        return 0;
    }

    public static double U(double x)
    {
        return 2 * x + 1;
    }


    public static double lambda(double qLeft, double qRight, double h)
    {
        double derr = (qRight - qLeft) / h;
        //return derr * derr;
        return 1;
    }

}