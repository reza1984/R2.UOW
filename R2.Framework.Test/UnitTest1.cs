using System;
using Autofac;
using R2.Domain.Context;
using R2.Framework.Sample.Domain.Entity;
using Xunit;

namespace R2.Framework.Test
{
    public class UnitTest1 : IClassFixture<TestDataFixture>
    {
        private readonly IUnitOfWork unitOfWork;
        public UnitTest1(TestDataFixture fixture)
        {
            
            unitOfWork = fixture.Container.Resolve<IUnitOfWork>();
        }

        [Fact]
        public void Test1()
        {
            var productUnitRepository = unitOfWork.GetRepository<ProductUnit>();
            var productUnit = new ProductUnit
            {
                Name = "Test Unit"
            };
            productUnitRepository.InsertAsync(productUnit);
            unitOfWork.SaveChangesAsync();
            var x = productUnitRepository.FindAsync(productUnit.Id).Result;
            Assert.True(x.Name.Equals(productUnit.Name));
        }
    }
}
