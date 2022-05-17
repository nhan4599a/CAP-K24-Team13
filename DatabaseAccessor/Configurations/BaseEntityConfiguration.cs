using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        public abstract bool IsForTestingPurpose { get; }

        public abstract void Configure(EntityTypeBuilder<TEntity> modelBuilder);
    }
}
