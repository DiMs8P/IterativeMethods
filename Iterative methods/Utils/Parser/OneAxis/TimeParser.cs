using Application;
using Application.Core.DataTypes;
using Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iterative_methods.Utils.Parser.OneAxis
{
    public class TimeParser : IParser<double>
    {
        double[] IParser<double>.Parse()
        {
            double[] timepoints = new double[Config.SplitsTimeNumber+1];
            double step = (Config.Totime - Config.Fromtime) / Config.SplitsTimeNumber;
            for (int i = 0; i < timepoints.Length; i++)
            {
                timepoints[i] = (Config.Fromtime + i * step);
            }

            return timepoints;
        }
    }
}
