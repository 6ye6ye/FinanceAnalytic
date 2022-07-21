using System;
using FinanceAnalytic.Models.ViewModels;

namespace FinanceAnalytic.Services
{
    public class CurrenciesRatesConverter
    {

        public CurrenciesRatesConverter(CurrenciesRate _KGS)
        {
            KGS = _KGS;
        }
        private CurrenciesRate kgs;
        public CurrenciesRate USD = new CurrenciesRate();
        public CurrenciesRate EUR = new CurrenciesRate();
        public CurrenciesRate KZT = new CurrenciesRate();
        public CurrenciesRate RUB = new CurrenciesRate();
        public CurrenciesRate KGS
        {
            get { return kgs; }
            set
            {
                kgs = value;
                USD.USD = 1;
                USD.KGS = Math.Round(1 / KGS.USD,3);
                USD.EUR = Math.Round(KGS.EUR / KGS.USD, 3);
                USD.RUB = Math.Round(KGS.RUB / KGS.USD, 3);
                USD.KZT = Math.Round(KGS.KZT / KGS.USD, 3);


                EUR.EUR = 1;
                EUR.KGS = Math.Round(1 / KGS.EUR,3);
                EUR.USD = Math.Round(KGS.USD / KGS.EUR, 3);
                EUR.RUB = Math.Round(KGS.RUB / KGS.EUR, 3);
                EUR.KZT = Math.Round(KGS.KZT / KGS.EUR, 3);

                RUB.RUB = 1;
                RUB.KGS = Math.Round(1 / KGS.RUB, 2);
                RUB.USD = Math.Round(KGS.USD / KGS.RUB, 3);
                RUB.EUR = Math.Round(KGS.EUR / KGS.RUB, 3);
                RUB.KZT = Math.Round(KGS.KZT / KGS.RUB, 3); 

                KZT.KZT = 1;
                KZT.KGS = Math.Round(1 / KGS.KZT, 2);
                KZT.USD = Math.Round(KGS.USD / KGS.KZT, 3);
                KZT.EUR = Math.Round(KGS.EUR / KGS.KZT, 3);
                KZT.RUB = Math.Round(KGS.RUB / KGS.KZT, 3);
            }
        }
       

        

        //public decimal KGS_USD {
        //    get 
        //    { 
        //        return KGS_USD; 
        //    }
        //    set 
        //    {
        //        //Currency rate for USD
        //        USD_KGS = 1 / KGS_USD;
        //        USD_EUR=KGS_USD/
        //    }
        //}
        //public decimal KGS_RUB  {
        //    get 
        //    { 
        //        return KGS_RUB; 
        //    }
        //    set 
        //    {
        //        RUB_KGS = 1 / KGS_RUB;
        //    }
        //}
        //public decimal KGS_EUR
        //{
        //    get
        //    {
        //        return KGS_EUR;
        //    }
        //    set
        //    {
        //        EUR_KGS = 1 / KGS_EUR;
        //    }
        //}
        //public decimal KGS_KZT
        //{
        //    get
        //    {
        //        return KGS_KZT;
        //    }
        //    set
        //    {
        //        USD_KZT = 1 / KGS_KZT;
        //    }
        //}

        ////Currency rate for USD
        //public decimal USD_KGS { get; set; }
        //public decimal USD_RUB { get; set; }
        //public decimal USD_EUR { get; set; }
        //public decimal USD_KZT { get; set; }
        ////Currency rate for EUR
        //public decimal EUR_KGS { get; set; }
        //public decimal EUR_RUB { get; set; }
        //public decimal EUR_USD { get; set; }
        //public decimal EUR_KZT { get; set; }
        ////Currency rate for RUB
        //public decimal RUB_KGS { get; set; }
        //public decimal RUB_EUR { get; set; }
        //public decimal RUB_USD { get; set; }
        //public decimal RUB_KZT { get; set; }
        ////Currency rate for KZT
        //public decimal KZT_KGS { get; set; }
        //public decimal KZT_EUR { get; set; }
        //public decimal KZT_USD { get; set; }
        //public decimal KZT_RUB { get; set; }



        //public decimal ConvertToEUR(decimal priceKGS) => priceKGS / KGS_EUR;
        //public decimal ConvertToRUB(decimal priceKGS) => priceKGS / KGS_RUB;
        //public decimal ConvertToUSD(decimal priceKGS) => priceKGS / KGS_USD;
        //public decimal ConvertToKZT(decimal priceKGS) => priceKGS / KGS_EUR;

        //public decimal ConvertToKGS(decimal priceKGS) => priceKGS / KGS;
    }
}
