using Infrastructure.Persistence.Interceptors;
using Duende.IdentityServer.EntityFramework.Options;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using MediatR;
using Infrastructure.Identity;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : KeyApiAuthorizationDbContext<User, Role, int>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext
        (
            DbContextOptions<ApplicationDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IMediator mediator,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
        )
        : base(options, operationalStoreOptions)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    #region Tables
    public DbSet<ImageEntity> Images => Set<ImageEntity>();
    public DbSet<NewsEntity> News => Set<NewsEntity>();
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<ContentEntity> Contents => Set<ContentEntity>();
    public DbSet<ContactEntity> Contacts => Set<ContactEntity>();
    public DbSet<SliderImageEntity> SliderImages => Set<SliderImageEntity>();
    public DbSet<RegisterMailEntity> RegisterMails => Set<RegisterMailEntity>();
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);

        builder.Entity<UserRole>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasOne(me => me.User)
                .WithMany(parent => parent.UserRoles)
                .HasForeignKey(me => me.UserId);

            b.HasOne(me => me.Role)
                .WithMany(parent => parent.UserRoles)
                .HasForeignKey(me => me.RoleId);
        });

        builder.Entity<User>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        var decimalProps = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

        foreach (var property in decimalProps)
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
