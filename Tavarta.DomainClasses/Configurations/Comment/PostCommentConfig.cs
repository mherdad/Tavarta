using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.Comment;

namespace Tavarta.DomainClasses.Configurations.Comment
{
    public class PostCommentConfig:EntityTypeConfiguration<PostComment>
    {

        public PostCommentConfig()
        {
            Property(x => x.PostId).IsOptional();

        }
    }
}