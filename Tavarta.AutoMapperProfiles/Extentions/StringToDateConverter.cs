using System;
using AutoMapper;

namespace Tavarta.AutoMapperProfiles.Extentions
{
    public class StringToDateConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(ResolutionContext context)
        {
            var stringDate = context.SourceValue;
            return PersianDateTime.Parse(stringDate.ToString()).ToDateTime();
        }

    }
}
