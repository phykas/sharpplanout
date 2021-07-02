using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SharpPlanOut.Core.Random
{
    public abstract class PlanOutOpRandom : PlanOutOpSimple
    {
        public static double _longScale = 0XFFFFFFFFFFFFFFFL;

        protected PlanOutOpRandom(Dictionary<string, object> param, Dictionary<string, object> units)
            : base(param, units)
        {
        }

        public long HashCalculation(string hash)
        {
            var hashCalculation = Convert.ToInt64(hash.Substring(0, 15), 16);
            return hashCalculation;
        }

        public double ZeroToOneCalculation(string appendedUnit)
        {
            return GetHash(appendedUnit) / _longScale;
        }

        public long GetHash(string appendedUnit = null)
        {
            string fullSalt;
            if (_param.ContainsKey("full_salt"))
            {
                fullSalt = _param["full_salt"] as string;
            }
            else
            {
                var salt = _param["salt"];
                fullSalt = Mapper.Get("experimentSalt") + "." + salt + Mapper.Get("saltSeparator");
            }
            var unitStr = GetUnitSalt(appendedUnit);
            var hashStr = fullSalt + unitStr;
            _param["full_salt"] = hashStr;
            var hash = Hash(hashStr);
            return HashCalculation(hash);
        }

        private static string Hash(string input)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public List<string> GetUnit(string appendedUnit)
        {
            var unit = _units.Select(e => e.Value.ToString()).ToList();

            if (appendedUnit != null)
            {
                unit.Add(appendedUnit);
            }
            return unit;
        }

        public string GetUnitSalt(string appendedUnit)
        {
            var unit = GetUnit(appendedUnit);

            return string.Join(".", unit.Select(e => e));
        }

        public double GetUniform(double minVal = 0.0, double maxVal = 1.0,
            string appendedUnit = null)
        {
            var zeroToOne = ZeroToOneCalculation(appendedUnit);
            return zeroToOne * (maxVal - minVal) + (minVal);
        }

        public override object Execute(IMapper mapper)
        {
            Mapper = mapper;
            return null;
        }
    }
}