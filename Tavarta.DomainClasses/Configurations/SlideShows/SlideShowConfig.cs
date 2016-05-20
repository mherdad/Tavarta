using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.SlideShows;

namespace Tavarta.DomainClasses.Configurations.SlideShows
{
    public class SlideShowConfig : EntityTypeConfiguration<SlideShowImage>
    {
        public SlideShowConfig()
        {
            Property(x => x.Id).IsOptional();
            Property(x => x.Order).IsOptional();
            Property(x => x.Text).IsOptional();
            Property(x => x.CreatedDate).IsOptional();
            Property(x => x.Description).IsOptional();
            Property(x => x.Image).IsOptional();
            Property(x => x.Link).IsOptional();
            Property(x => x.Title).IsOptional();
        }
    }
}