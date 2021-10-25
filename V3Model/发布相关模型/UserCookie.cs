using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MyProto;

namespace Model.发布相关模型
{
    [ProtoContract]
    [Serializable]
    public class Account
    {
         [ProtoMember(1)]
        public string username = "";
         [ProtoMember(2)]
        public string password = "";
         [ProtoMember(3)]
        public string loginvalue1 = "";
         [ProtoMember(4)]
        public string loginvalue2 = "";
        
        private xEngine.Model.Execute.Http.XCookieManager mycookie;
         [ProtoMember(5)]
        public xEngine.Model.Execute.Http.XCookieManager MyCookie
        {
            get { return mycookie; }
            set { mycookie = value; }
        }
         [ProtoMember(6)]
        public DateTime LastActiveTime = new DateTime();
         [ProtoMember(7)]
        public int CountFalse = 0;
         [ProtoMember(8)]
        public string Status = "未知";
         [ProtoMember(9)]
        public int CountTrue = 0;
        private string majiaurl = "";
         [ProtoMember(10)]
        public string Majiaurl
        {
            get { return majiaurl; }
            set { majiaurl = value; }
        }
        private string majiastr = "";
         [ProtoMember(11)]
        public string Majiastr
        {
            get { return majiastr; }
            set { majiastr = value; }
        }
    }
    [ProtoContract]
    [Serializable]
    public class UserAccount
    {
        /// <summary>
        /// 正常的账号
        /// </summary>
        [ProtoMember(12)]
        public Dictionary<string, Account> AccountTrue = new Dictionary<string, Account>();
        /// <summary>
        /// 异常的账号
        /// </summary>
         [ProtoMember(13)]
        public Dictionary<string, Account> AccountFalse = new Dictionary<string, Account>();
        /// <summary>
        /// 随机调用账号
        /// </summary>
         [ProtoMember(14)]
        public bool randomget = false;
        /// <summary>
        /// 失败多少次后尝试登录
        /// </summary>
         [ProtoMember(15)]
        public int tryLoginCount = 5;
        /// <summary>
        /// 尝试多少次后认为登录失败
        /// </summary>
        [ProtoMember(16)]
        public int tryLoginFalse = 7;
        /// <summary>
        /// 失败统计
        /// </summary>
         [ProtoMember(17)]
        public int FalseCount = 0;
         [ProtoMember(18)]
        public int No = 0;
        private string NowAccount = "";
        /// <summary>
        /// 获取新账号
        /// </summary>
        /// <returns></returns>
        public Account GetNewAccount()
        {
            CheckAccount();
            if (AccountTrue.Count > 0)
            {
                if (randomget)
                {

                    try
                    {
                        Random r = new Random();
                        string[] dataArray = new string[AccountTrue.Values.Count];
                        AccountTrue.Keys.CopyTo(dataArray, 0);
                        NowAccount =dataArray[r.Next(0, dataArray.Length)];
                        return AccountTrue[NowAccount];
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    if (No < AccountTrue.Count)
                    {
                        try
                        {
                            string[] dataArray = new string[AccountTrue.Values.Count];
                            AccountTrue.Keys.CopyTo(dataArray, 0);
                            if (No + 1 >= AccountTrue.Count)
                                No = 0;
                            else
                                No++;
                            NowAccount = dataArray[No];
                            return AccountTrue[dataArray[No]];
                        }
                        catch
                        {
                            return null;
                        }
                    }
                    else
                    {
                        try
                        {
                            No = 0;
                            string[] dataArray = new string[AccountTrue.Values.Count];
                            AccountTrue.Keys.CopyTo(dataArray, 0);
                            if (No + 1 >= AccountTrue.Count)
                                No = 0;
                            else
                                No++;
                            NowAccount = dataArray[No];
                            return AccountTrue[dataArray[No]];
                        }
                        catch
                        {
                            return null;
                        }
                    }

                }
            }
            else
                return null;
        }
        /// <summary>
        /// 获取当前账号
        /// </summary>
        /// <returns></returns>
        public Account GetNowAccount(int falseCount)
        {
            if (NowAccount == "")
                GetNewAccount();
            if (NowAccount == "")
                return null;
            return  AccountTrue[NowAccount];
        }
        public void AddorUpdateAccount(Account account,bool Type)
        {
            if (Type)
            {
                if (account.Status != "正常")
                {
                    account.Status = "未知";
                    account.CountFalse = 0;
                    account.CountTrue = 0;
                
                }
                if (!AccountTrue.ContainsKey(account.username))
                {
                    AccountTrue.Add(account.username, account);
                    if (AccountFalse.ContainsKey(account.username))
                    {
                        AccountFalse.Remove(account.username);
                    }

                }
                else
                {
                    AccountTrue[account.username] = account;
                    if (AccountFalse.ContainsKey(account.username))
                    {
                        AccountFalse.Remove(account.username);
                    }
                }
            }
            else
            {
                if (account.Status == "正常" || account.Status == "未知")
                    account.Status = "账号异常";
                if (!AccountFalse.ContainsKey(account.username))
                {
                    AccountFalse.Add(account.username, account);
                    if (AccountTrue.ContainsKey(account.username))
                    {
                        AccountTrue.Remove(account.username);
                    }
                }
                else
                {
                    AccountFalse[account.username] = account;
                    if (AccountTrue.ContainsKey(account.username))
                    {
                        AccountTrue.Remove(account.username);
                    }
                }
            }

        }
        void CheckAccount()
        {
            string[] dataArray = new string[AccountTrue.Values.Count];
            AccountTrue.Keys.CopyTo(dataArray, 0);
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (AccountTrue[dataArray[i]].CountFalse >=7)
                {
                    AddorUpdateAccount(AccountTrue[dataArray[i]], false);
                }
            }
        }
    }


}
