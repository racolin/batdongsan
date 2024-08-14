using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ImageEntity> Images { get; }
    DbSet<NewsEntity> News { get; }
    DbSet<ProjectEntity> Projects { get; }
    DbSet<ContentEntity> Contents { get; }
    DbSet<ContactEntity> Contacts { get; }
    DbSet<ConfigurationEntity> Configurations { get; }
    DbSet<SliderImageEntity> SliderImages { get; }
    DbSet<RegisterMailEntity> RegisterMails { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
