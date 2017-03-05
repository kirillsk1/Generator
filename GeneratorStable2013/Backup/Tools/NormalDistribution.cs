 
using System;

namespace Tools
{
  public class NormalDistribution
  {
    public double mMean;
    public double mSigma;
    private readonly Random mRnd = new Random();

    public NormalDistribution(double mean, double sigma)
    {
      mMean = mean;
      mSigma = sigma;
    }

    public double doubleND()
    {
      bool NRand_Available = false;
      double NRand;
      double Saved_NRand = 0.0;
      double v1;
      double v2;
      double r;
      double fac;

      if (NRand_Available)
      {
        NRand = Saved_NRand*mSigma + mMean;
        NRand_Available = false;
      }
      else
      {
        do
        {
          v1 = 2.0*mRnd.NextDouble() - 1.0;
          v2 = 2.0*mRnd.NextDouble() - 1.0;
          r = v1*v1 + v2*v2;
        } while (r >= 1.0);
        fac = Math.Sqrt(-2.0*Math.Log(r)/r);
        NRand = v1*fac*mSigma + mMean;
        Saved_NRand = v2*fac;
        NRand_Available = true;
      }

      return NRand;
    }

    public int intND()
    {
      return (int) doubleND();
    }
  }
}