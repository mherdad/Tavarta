using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.Postes;

namespace Tavarta.DomainClasses.Configurations.Postes
{
    public class CategoryConfig:EntityTypeConfiguration<Category>
    {

        public CategoryConfig()
        {
            Property(x => x.Name).HasMaxLength(500);
        }
    }
}