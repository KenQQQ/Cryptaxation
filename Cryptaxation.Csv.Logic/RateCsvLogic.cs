﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Cryptaxation.Csv.Contract;
using Cryptaxation.Csv.Fields;
using Cryptaxation.Entities;
using Cryptaxation.Entities.Types.Enums;
using Microsoft.VisualBasic.FileIO;

namespace Cryptaxation.Csv.Logic
{
    public class RateCsvLogic<TRate> : IRateCsvLogic<TRate> where TRate : Rate, new()
    {
        private string _path;

        public List<TRate> CreateRateList(string[] paths)
        {
            var rateList = new List<TRate>();
            foreach (var path in paths)
            {
                _path = path;
                rateList.AddRange(CreateRateList());
            }
            return rateList;
        }

        public List<TRate> CreateRateList()
        {
            var rateList = new List<TRate>();
            using (TextFieldParser parser = new TextFieldParser(_path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                for (int rowIndex = 0; !parser.EndOfData; rowIndex++)
                {
                    string[] row = parser.ReadFields();
                    if (rowIndex == 0)
                    {
                        continue;
                    }
                    if (row != null)
                    {
                        rateList.AddRange(CreateRates(row));
                    }
                    else
                    {
                        throw new Exception("No rows found.");
                    }
                }
            }
            return rateList;
        }

        private List<TRate> CreateRates(string[] row)
        {
            var rates = new List<TRate>();
            DateTime? date = null;
            for (int i = 0; i < row.Length; i++)
            {
                RateFields rateFields = (RateFields)i;
                if (row.Length == 2 && i == 1)
                {
                    rateFields = RateFields.BTCUSD;
                }
                switch (rateFields)
                {
                    case RateFields.Datum:
                        string tmp = row[i].Substring(0, 10);
                        date = DateTime.ParseExact(tmp, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        break;
                    case RateFields.SEKUSD:
                        rates.Add(new TRate
                        {
                            Date = date.GetValueOrDefault(),
                            OriginCurrency = CurrencyCode.SEK,
                            DestinationCurrency = CurrencyCode.USD,
                            Value = decimal.Parse(row[i], NumberStyles.Any, CultureInfo.InvariantCulture)
                        });
                        break;
                    case RateFields.USDSEK:
                        rates.Add(new TRate
                        {
                            Date = date.GetValueOrDefault(),
                            OriginCurrency = CurrencyCode.USD,
                            DestinationCurrency = CurrencyCode.SEK,
                            Value = decimal.Parse(row[i], NumberStyles.Any, CultureInfo.InvariantCulture)
                        });
                        break;
                    case RateFields.SEKEUR:
                        rates.Add(new TRate
                        {
                            Date = date.GetValueOrDefault(),
                            OriginCurrency = CurrencyCode.SEK,
                            DestinationCurrency = CurrencyCode.EUR,
                            Value = decimal.Parse(row[i], NumberStyles.Any, CultureInfo.InvariantCulture)
                        });
                        break;
                    case RateFields.EURSEK:
                        rates.Add(new TRate
                        {
                            Date = date.GetValueOrDefault(),
                            OriginCurrency = CurrencyCode.EUR,
                            DestinationCurrency = CurrencyCode.SEK,
                            Value = decimal.Parse(row[i], NumberStyles.Any, CultureInfo.InvariantCulture)
                        });
                        break;
                    case RateFields.EURUSD:
                        rates.Add(new TRate
                        {
                            Date = date.GetValueOrDefault(),
                            OriginCurrency = CurrencyCode.EUR,
                            DestinationCurrency = CurrencyCode.USD,
                            Value = decimal.Parse(row[i], NumberStyles.Any, CultureInfo.InvariantCulture)
                        });
                        break;
                    case RateFields.USDEUR:
                        rates.Add(new TRate
                        {
                            Date = date.GetValueOrDefault(),
                            OriginCurrency = CurrencyCode.USD,
                            DestinationCurrency = CurrencyCode.EUR,
                            Value = decimal.Parse(row[i], NumberStyles.Any, CultureInfo.InvariantCulture)
                        });
                        break;
                    case RateFields.BTCUSD:
                        rates.Add(new TRate
                        {
                            Date = date.GetValueOrDefault(),
                            OriginCurrency = CurrencyCode.BTC,
                            DestinationCurrency = CurrencyCode.USD,
                            Value = decimal.Parse(row[i], NumberStyles.Any, CultureInfo.InvariantCulture)
                        });
                        break;
                    default:
                        throw new Exception("Invalid field");
                }
            }
            return rates;
        }
    }
}
