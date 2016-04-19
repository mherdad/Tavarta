using System;
using AutoMapper;

namespace Tavarta.AutoMapperProfiles.Extentions
{
    public class DateTimeToStringResolver : ValueResolver<DateTime,string>
    {
        private readonly PersianDateTimeFormat _format;

        public DateTimeToStringResolver(PersianDateTimeFormat format)
        {
            _format = format;
        }
        protected override string ResolveCore(DateTime source)
        {
            var persianDateTime=new PersianDateTime(source);
            return persianDateTime.ToString(_format);
        }
    }
}
