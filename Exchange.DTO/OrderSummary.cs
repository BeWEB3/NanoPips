using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.DTO
{
    public class OrderResult
    {
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public string QuantityString => Quantity.ToString("N", new CultureInfo("en-US"));
        public string RateString=> Rate.ToString("N", new CultureInfo("en-US"));

    }
    //public class OrderSummary
    //{
    //    public bool success { get; set; }
    //    public string message { get; set; }
    //    public List<OrderResult> result { get; set; }
    //}
    //public class OrderSummaries {

    //    public OrderSummary Buy { get; set; }
    //    public OrderSummary Sell { get; set; }

    //}

    //public class Buy
    //{
    //    public double Quantity { get; set; }
    //    public double Rate { get; set; }
    //    public string QuantityString => Quantity.ToString("N", new CultureInfo("en-US"));
    //    public string RateString => Rate.ToString("N", new CultureInfo("en-US"));

    //}

    //public class Sell
    //{
    //    public double Quantity { get; set; }
    //    public double Rate { get; set; }
    //    public string QuantityString => Quantity.ToString("N", new CultureInfo("en-US"));
    //    public string RateString => Rate.ToString("N", new CultureInfo("en-US"));

    //}

    public class OrderSummaries
    {
        public List<OrderResult> buy { get; set; }
        public List<OrderResult> sell { get; set; }
    }

    public class OrderSummary
    {
        public bool success { get; set; }
        public string message { get; set; }
        public OrderSummaries result { get; set; }
    }

}
