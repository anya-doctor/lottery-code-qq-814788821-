﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ytg.Scheduler.Comm.Bets.Calculate
{
    /// <summary>
    /// 前三/前三组选/组六复式 验证通过 2015/05/23 2016 01 18
    /// </summary>
    public class QsZ6fsCalculate : ZhiXuanFushiDetailsCalculate
    {
        /// <summary>
        /// 计算中奖情况 todo://还需要继续写的
        /// </summary>
        /// <param name="issueCode"></param>
        /// <param name="openResult"></param>
        /// <param name="item"></param>
        protected override void IssueCalculate(string issueCode, string openResult, BasicModel.LotteryBasic.BetDetail item)
        {
            Combinations<string> c = new Combinations<string>(item.BetContent.Select(x => x.ToString()).ToList(), 3);
            var list = c._combinations;
            //开奖结果
            var housan = openResult.Replace(",", "").Substring(0,3);
            var temp = string.Join("", housan.OrderBy(m => m).Select(m => m.ToString()));
            var count = 0;
            if (list != null && list.Count > 0)
            {
                count = list.Count(m => temp == string.Join("", m.OrderBy(n => n).Select(n => n.ToString())));
            }
            if (count > 0)
            {
                item.IsMatch = true;
                decimal stepAmt = 0;
                item.WinMoney = TotalWinMoney(item, GetBaseAmt(item, ref stepAmt), stepAmt, 1);
            }
        }

        /// <summary>
        /// 计算投注数
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override int TotalBetCount(BasicModel.LotteryBasic.BetDetail item)
        {
            return CombinationHelper.Cmn(item.BetContent.Length, 3);
        }

        protected override int GetLen
        {
            get
            {
                return 1;
            }
        }

        public override string HtmlContentFormart(string betContent)
        {
            betContent= base.HtmlContentFormart(betContent);
            if (string.IsNullOrEmpty(betContent))
                return string.Empty;
            if (betContent.Length < 3)
                return string.Empty;
            return betContent;
        }
    }
}
