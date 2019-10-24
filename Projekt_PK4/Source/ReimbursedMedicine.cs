using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_PK4
{
    public enum LevelOfFunding
    {
        [Description("Bezplatny - do wysokosci limitu")] darmowy,//free
        [Description("Ryczalt(3,20) - do wysokosci limitu")] rzyczalt,//lumpSum
        [Description("50% limitu funansowania")] procent30,//percent30
        [Description("30% limitu funansowania")] procent50,//percent50
        [Description("Nierefundowany")] nierefundowany//noRefund
    };

    public class ReimbursedMedicine : ICloneable
    {
        public double FundingLimit { get; set; }

        public LevelOfFunding Level { get; set; }


        public ReimbursedMedicine(double fundingLimit_ = 0, LevelOfFunding level_ = LevelOfFunding.nierefundowany)
        {
            FundingLimit = fundingLimit_;
            Level = level_;
        }


        public double Discount()
        {
            switch (Level)
            {
                case LevelOfFunding.darmowy:
                    return FundingLimit * RefundationUnits.free;
                case LevelOfFunding.procent30:
                    return FundingLimit * RefundationUnits.percent30;
                case LevelOfFunding.procent50:
                    return FundingLimit * RefundationUnits.percent50;
                case LevelOfFunding.rzyczalt:
                    return FundingLimit - RefundationUnits.lumpSum;
                default:
                    return 0;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
    
    public struct RefundationUnits
    {
        public const double free = 1;
        public const double percent30 = 0.7;
        public const double percent50 = 0.5;

        public const double lumpSum = 3.2;
    }

}
