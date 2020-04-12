using System.Collections.Generic;

namespace PolygonWeb
{
    class PgonData { }

	public class PolygonBase
	{
		public string ev { get; set; }
	}

	public class PolygonBaseX
	{
		public string ev { get; set; }
		public Trade TradeRef { get; set; }
		public Quote QuoteRef { get; set; }
		public ASecond ASecondRef { get; set; }
		public AMinute AMinuteRef { get; set; }
		public Status StatusRef { get; set; }
	}

	// https://polygon.io/sockets

	// [{"ev":"T","sym":"AMZN","i":7657,"x":4,"p":1863.745,"s":2,"c":[37],"t":1580319160113,"z":3}]
	public class Trade : PolygonBase
	{
		public string sym { get; set; }		// Symbol Ticker
		public string i { get; set; }			// Trade ID
		public int x { get; set; }			// Exchange ID
		public double p { get; set; }			// Price
		public int s { get; set; }			// Trade Size
		public object c { get; set; }			// Trade Conditions
		public long t { get; set; }			// Trade Timestamp ( Unix MS )
		public int z { get; set; }			// Tape ( 1=A 2=B 3=C)
	}

	// [{"ev":"Q","sym":"AMZN","c":1,"bx":12,"ax":12,"bp":1864.6,"ap":1865.14,"bs":1,"as":1,"t":1580319307234,"z":3}]
	public class Quote : PolygonBase
	{
		public string sym { get; set; }		// Symbol Ticker
		public object c { get; set; }			// Quote Condition
		public string bx { get; set; }		// Bix Exchange ID
		public string ax { get; set; }		// Ask Exchange ID
		public double bp { get; set; }		// Bid Price
		public double ap { get; set; }		// Ask Price
		public int bs { get; set; }			// Bid Size
		public int ask { get; set; }			// Ask Size
		public long t { get; set; }			// Quote Timestamp ( Unix MS )
		public int z { get; set; }			// ?
	}

	public class ASecond : AMinute
	{
	}

	public class AMinute : PolygonBase
	{
		public string sym { get; set; }	// Symbol Ticker
		public int v { get; set; }		// Tick Volume
		public int av { get; set; }		// Accumulated Volume ( Today )
		public double op { get; set; }	// Todays official opening price
		public double vw { get; set; }	// VWAP (Volume Weighted Average Price)
		public double o { get; set; }		// Open
		public double c { get; set; }		// Close
		public double h { get; set; }		// High
		public double l { get; set; }		// Low
		public double a { get; set; }		// Tick Average / VWAP Price
		public long s { get; set; }		// Tick Start Timestamp ( Unix MS )
		public long e { get; set; }		// Tick End Timestamp ( Unix MS )
	}

	// [{"ev":"status","status":"connected","message":"Connected Successfully"}]
	public class Status : PolygonBase
{
		public string status { get; set; }
		public string message { get; set; }
	}


	#region Historic

	public class HistoricTrade
	{
		public int results_count { get; set; }		// Total number of results in this response
		public int db_latency { get; set; }		// Milliseconds of latency for the query results from DB
		public bool success { get; set; }			// If this query was executed successfully
		public string ticker { get; set; }			// Ticker symbol that was evaluated from the request
		public List<HTResult> results { get; set; } // Results
	}

	// JSON map
	//"success": true,
	//"map": {
	//	"t": {
	//		"name": "sip_timestamp",
	//		"type": "int64"
	//	},
	//	"q": {
	//		"name": "sequence_number",
	//		"type": "int"
	//	},
	//	"I": {
	//		"name": "orig_id",
	//		"type": "string"
	//	},
	//	"s": {
	//		"name": "size",
	//		"type": "int"
	//	},
	//	"p": {
	//		"name": "price",
	//		"type": "float64"
	//	},
	//	"y": {
	//		"name": "participant_timestamp",
	//		"type": "int64"
	//	},
	//	"f": {
	//		"name": "trf_timestamp",
	//		"type": "int64"
	//	},
	//	"c": {
	//		"name": "conditions",
	//		"type": "[]int"
	//	},
	//	"i": {
	//		"name": "id",
	//		"type": "string"
	//	},
	//	"e": {
	//		"name": "correction",
	//		"type": "int"
	//	},
	//	"x": {
	//		"name": "exchange",
	//		"type": "int"
	//	},
	//	"r": {
	//		"name": "trf_id",
	//		"type": "int"
	//	},
	//	"z": {
	//		"name": "tape",
	//		"type": "int"
	//	}
	//},
	//"ticker": "AMZN",
	//"results_count": 0,
	//"db_latency": 2984

	//"c": [ 0, 12, 37 ],
	//"i": "1",
	//"p": 1906.33,
	//"s": 25,
	//"x": 11,
	//"r": 4,
	//"z": 3,
	//"t": 1583485200032242379,
	//"y": 1583485200031851264,
	//"q": 1184

	public class HTResult
	{
		public string T { get; set; }			// Ticker of the object
		public long t { get; set; }			// Nanosecond accuracy SIP Unix Timestamp
		public long y { get; set; }			// Nanosecond accuracy Participant/Exchange Unix Timestamp
		public long f { get; set; }			// Nanosecond accuracy TRF(Trade Reporting Facility) Unix Timestamp
		public int q { get; set; }			// Sequence Number
		public int r { get; set; }			// Sequence Number
		public string i { get; set; }			// Trade ID
		public int x { get; set; }			// Exchange ID
		public int s { get; set; }			// Size/Volume of the trade
		public List<int> c { get; set; }		// Conditions
		public double p { get; set; }			// Price of the trade
		public int z { get; set; }			// Tape where trade occurred. ( 1,2 = CTA, 3 = UTP )
	}

	public class HistoricQuote
	{
		public int results_count { get; set; }			// Total number of results in this response
		public int db_latency { get; set; }			// Milliseconds of latency for the query results from DB
		public bool success { get; set; }				// If this query was executed successfully
		public string ticker { get; set; }				// Ticker symbol that was evaluated from the request
		public List<HQResult> results { get; set; }		// Results
	}

	public class HQResult
	{
		public string T { get; set; }			// Ticker of the object
		public long t { get; set; }			// Nanosecond accuracy SIP Unix Timestamp
		public long y { get; set; }			// Nanosecond accuracy Participant/Exchange Unix Timestamp
		public long f { get; set; }			// Nanosecond accuracy TRF(Trade Reporting Facility) Unix Timestamp
		public int q { get; set; }			// Sequence Number
		public List<HQCondition> c { get; set; }		// Conditions
		public List<HQIndicator> i { get; set; }		// Indicators
		public double p { get; set; }			// BID Price
		public int x { get; set; }			// BID Exchange ID
		public int s { get; set; }			// BID Size ( In round lots )
		public double P { get; set; }			// ASK Price
		public int X { get; set; }			// ASK Exchange ID
		public int S { get; set; }			// ASK Size ( In round lots )
		public int z { get; set; }			// Tape where trade occurred. ( 1,2 = CTA, 3 = UTP )
	}

	public class HQCondition
	{
	}
	public class HQIndicator
	{
	}

	#endregion

	#region Aggregate

	// https://polygon.io/docs/#!/Stocks--Equities/get_v2_aggs_ticker_ticker_range_multiplier_timespan_from_to
	public class Aggregate
	{
		public string ticker { get; set; }			// Ticker symbol requested
		public string status { get; set; }			// Status of the response
		public bool adjusted { get; set; }			// If this response was adjusted for splits
		public int queryCount { get; set; }		// Number of aggregate ( min or day ) used to generate the response
		public int resultsCount { get; set; }		// Total number of results generated
		public List<AggResult> results { get; set; }
	}

	public class AggResult
	{
		//public string T { get; set; }
		public int v { get; set; }		// Volume
		public double o { get; set; }		// Open
		public double c { get; set; }		// Close
		public double h { get; set; }		// High
		public double l { get; set; }		// Low
		public long t { get; set; }		// Unix Msec Timestamp ( Start of Aggregate window )
		public int n { get; set; }		// Number of items in aggregate window
	}

	#endregion

	#region Last

	// https://polygon.io/docs/#!/Stocks--Equities/get_v1_last_stocks_symbol
	public class LastTrade
	{
		public string status { get; set; }      // Status of this requests response
		public string symbol { get; set; }      // Symbol that was evaluated from the request
		public TradeLast last { get; set; }
	}
	public class TradeLast
	{
		public double price { get; set; }       // Price of the trade
		public int size { get; set; }           // Size of this trade
		public int exchange { get; set; }       // Exchange this trade happened on
		public int cond1 { get; set; }
		public int cond2 { get; set; }
		public int cond3 { get; set; }
		public int cond4 { get; set; }
		public long timestamp { get; set; }
	}

	public class LastQuote
	{
		public string status { get; set; }      // Status of this requests response
		public string symbol { get; set; }      // Symbol that was evaluated from the request
		public QuoteLast last { get; set; }
	}
	public class QuoteLast
	{
		public double askprice { get; set; }
		public int asksize { get; set; }
		public int askexchange { get; set; }
		public double bidprice { get; set; }
		public int bidsize { get; set; }
		public int bidexchange { get; set; }    // Exchange this trade happened on
		public long timestamp { get; set; }
	}

	#endregion

	#region Condition Mappings

	//https://polygon.io/docs/#get_v1_meta_conditions__ticktype__anchor
	//https://api.polygon.io/v1/meta/conditions/trades

	//{
	//  "0": "Regular",
	//  "1": "Acquisition",
	//  "2": "AveragePrice",
	//  "3": "AutomaticExecution",
	//  "4": "Bunched",
	//  "5": "BunchSold",
	//  "6": "CAPElection",
	//  "7": "CashTrade",
	//  "8": "Closing",
	//  "9": "Cross",
	//  "10": "DerivativelyPriced",
	//  "11": "Distribution",
	//  "12": "FormT(ExtendedHours)",
	//  "13": "FormTOutOfSequence",
	//  "14": "InterMarketSweep",
	//  "15": "MarketCenterOfficialClose",
	//  "16": "MarketCenterOfficialOpen",
	//  "17": "MarketCenterOpening",
	//  "18": "MarketCenterReOpenning",
	//  "19": "MarketCenterClosing",
	//  "20": "NextDay",
	//  "21": "PriceVariation",
	//  "22": "PriorReferencePrice",
	//  "23": "Rule155Amex",
	//  "24": "Rule127Nyse",
	//  "25": "Opening",
	//  "26": "Opened",
	//  "27": "RegularStoppedStock",
	//  "28": "ReOpening",
	//  "29": "Seller",
	//  "30": "SoldLast",
	//  "31": "SoldLastStoppedStock",
	//  "32": "SoldOutOfSequence",
	//  "33": "SoldOutOfSequenceStoppedStock",
	//  "34": "Split",
	//  "35": "StockOption",
	//  "36": "YellowFlag",
	//  "37": "OddLot",
	//  "38": "CorrectedConsolidatedClosePrice",
	//  "39": "Unknown",
	//  "40": "Held",
	//  "41": "TradeThruExempt",
	//  "42": "NonEligible",
	//  "43": "NonEligible-extended",
	//  "44": "Cancelled",
	//  "45": "Recovery",
	//  "46": "Correction",
	//  "47": "AsOf",
	//  "48": "AsOfCorrection",
	//  "49": "AsOfCancel",
	//  "50": "OOB",
	//  "51": "Summary",
	//  "52": "Contingent",
	//  "53": "Contingent(Qualified)",
	//  "54": "Errored"
	//}

	#endregion

}
