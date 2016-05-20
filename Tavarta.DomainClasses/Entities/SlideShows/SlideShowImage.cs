using System;
using Tavarta.Utility;

namespace Tavarta.DomainClasses.Entities.SlideShows
{
    public class SlideShowImage
    {
        public SlideShowImage()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid();
            CreatedDate = DateTime.Now; ;
        }

        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}