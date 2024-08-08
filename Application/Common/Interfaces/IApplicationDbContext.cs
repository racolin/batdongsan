using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ImageEntity> Images { get; }
    DbSet<ImagePageEntity> ImagePages { get; }
    DbSet<NewsEntity> News { get; }
    DbSet<ProjectEntity> Projects { get; }
    DbSet<SectionEntity> Sections { get; } 
    DbSet<SliderEntity> Sliders { get; }
    DbSet<ContactEntity> Contacts { get; }
    DbSet<RegisterMailEntity> RegisterMails { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
