﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ytg.Scheduler.Comm.Bets.Calculate.HouEr
{
    /// <summary>
    /// 后二/后二组选/组选和值 验证通过 2015/05/24 2016 01 18
    /// </summary>
    public class HeZuxhzCalculate : ZhiXuanDanShiDetailsCalculate
    {
        /// <summary>
        /// 计算中奖情况
        /// </summary>
        /// <param name="issueCode"></param>
        /// <param name="openResult"></param>
        /// <param name="item"></param>
        protected override void IssueCalculate(string issueCode, string openResult, BasicModel.LotteryBasic.BetDetail item)
        {
            var count = TotalBet(item, openResult);
            if (count > 0)
            {
                item.IsMatch = true;
                decimal stepAmt = 0;
                item.WinMoney = TotalWinMoney(item, GetBaseAmt(item, ref stepAmt), stepAmt, 1);
            }
        }

        /// <summary>
        /// 计算头注数
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override int TotalBetCount(BasicModel.LotteryBasic.BetDetail item)
        {
            return TotalBet(item, "");
        }

        #region 计算具体中奖算法，和投注数
        private int TotalBet(BasicModel.LotteryBasic.BetDetail item, string openResult)
        {
            var sum = 0;
            var res = new List<string>();
            if (!string.IsNullOrEmpty(openResult))
            {
                res = openResult.Split(',').ToList();
                sum = res.Skip(3).Take(2).Sum(m => Convert.ToInt32(m));
            }
            List<int> hezhi = item.BetContent.Split(',').Select(m => Convert.ToInt32(m.ToString())).ToList();
            List<string> list = new List<string>();
            if (hezhi.Count >= 1)
            {
                for (int i = 0; i < 100; i++)
                {
                    var v = i.ToString("d2");
                    var vsum = v.Sum(p => Convert.ToInt16(p.ToString()));
                    if (v.Distinct().Count() > 1)
                    {
                        if (hezhi.Any(m => v.Sum(n => Convert.ToInt16(n.ToString())) == m) && !list.Any(n => string.Join("", n.OrderBy(s => s)) == string.Join("", v.OrderBy(s => s))))
                        {
                            if (string.IsNullOrEmpty(openResult))
                                list.Add(v);
                            else if (sum == vsum)
                            {
                                if (v.Distinct().Count() == 2)
                                    list.Add(v);
                            }
                        }
                    }
                }
            }
            return list.Count;
        }
        #endregion

        public override string HtmlContentFormart(string betContent)
        {

            var array = betContent.Replace("&", ",").Split(',');
            var list = new List<string>();
            foreach (var item in array)
            {
                if (list.Contains(item))
                    continue;
                int num = Convert.ToInt32(item);
                if (num >= 1 && num <= 17)
                {
                    list.Add(item);
                }
            }
            return string.Join(",", list);
        }
    }
}
