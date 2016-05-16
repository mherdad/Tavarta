using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.Postes;

namespace Tavarta.DomainClasses.Configurations.Postes
{
    public class PostConfig:EntityTypeConfiguration<Post>
    {
        public PostConfig()
        {
            Property(p => p.RowVersion).IsRowVersion();
            Property(p => p.ModifiedOn).IsOptional();
            Property(p => p.LinkBackStatus).IsOptional();
            Property(p => p.UseCanonicalUrl).IsOptional();
            Property(p => p.UseNoFollow).IsOptional();
            Property(p => p.UseNoIndex).IsOptional();
            Property(p => p.IsInSitemap).IsOptional();
            Property(p => p.AllowCommentForAnonymous).IsOptional();
            Property(p => p.AllowComments).IsOptional();
            Property(p => p.ViewCount).IsOptional();
            Property(p => p.ViewCountByRss).IsOptional();
            Property(p => p.ApprovedCommentsCount).IsOptional();
            Property(p => p.UnApprovedCommentsCount).IsOptional();
            Property(p => p.IsDeleted).IsOptional();
            Property(p => p.ShowWithRss).IsOptional();
            Property(p => p.DaysCountForSupportComment).IsOptional();
            Property(p => p.AuthorId).IsOptional();
            Property(p => p.HeadTitle).HasMaxLength(200);
            Property(p => p.Title).HasMaxLength(200);


        }
    }
}