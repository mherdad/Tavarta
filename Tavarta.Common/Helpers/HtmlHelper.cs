﻿using System;
using System.Web.Mvc;
using Persia;

namespace Tavarta.Common.Helpers
{
    public static class ConverterToPersian
    {
        public static MvcHtmlString ConvertToPersianString(this HtmlHelper htmlHelper, int digit)
        {
            return MvcHtmlString.Create(PersianWord.ToPersianString(digit));
        }

        public static MvcHtmlString ConvertToPersianString(this HtmlHelper htmlHelper, string str)
        {
            return MvcHtmlString.Create(PersianWord.ToPersianString(str));
        }

        public static MvcHtmlString ConvertToPersianDateTime(this HtmlHelper htmlHelper, DateTime dateTime,
            string mode = "")
        {
            return dateTime.Year == 1 ? null : MvcHtmlString.Create(Tavarta.Common.DateAndTime.DateAndTime.ConvertToPersian(dateTime, mode));
        }

        public static string ConvertBooleanToPersian(this HtmlHelper htmlHelper, bool? value)
        {
            return !Convert.ToBoolean(value) ? "آزاد" : "مسدود";
        }

        public static string ConvertBooleanToPersian(this HtmlHelper htmlHelper, bool value)
        {
            return !Convert.ToBoolean(value) ? "آزاد" : "مسدود";
        }
    }
}