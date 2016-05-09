using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.Postes;

namespace Tavarta.DomainClasses.Configurations.Postes
{
    public class PostConfig:EntityTypeConfiguration<Post>
    {
        public PostConfig()
        {
            Property(@t => @t.RowVersion).IsRowVersion();

        }
    }
}