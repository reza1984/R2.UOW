using Microsoft.EntityFrameworkCore;
using System;
using R2.Domain.Helpers;
using Autofac;
using R2.Domain.Context;
using R2.Domain.Repository;

public class TestDataFixture
{
    public IContainer Container { get; private set; }
    public TestDataFixture()
    {
        var dataContextBuilder = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("TestDatabase");

        // var dataContextBuilder = new DbContextOptionsBuilder<DataContext>()
        //     .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Warehouse;Trusted_Connection=True;MultipleActiveResultSets=true");

        var context = new DataContext(dataContextBuilder.Options);

        var builder = new ContainerBuilder();
        builder.RegisterInstance(context).As<DataContext>();

        builder.RegisterType<UnitOfWork<DataContext>>().AsSelf()
        .As<IUnitOfWork>();

        builder.RegisterAssemblyTypes(typeof(TestDataFixture).Assembly);

        Container = builder.Build();

    }

}