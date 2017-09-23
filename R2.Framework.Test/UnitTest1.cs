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

        [Fact]
        public void CreationTime_Test()
        {
            var productUnitRepository = unitOfWork.GetRepository<ProductUnit>();
            var productUnit = new ProductUnit
            {
                Name = "Test Unit"
            };
            productUnitRepository.InsertAsync(productUnit);
            unitOfWork.SaveChangesAsync();

            #region Assert
            var x = productUnitRepository.FindAsync(productUnit.Id).Result;
            Assert.True(x.Name.Equals(productUnit.Name));
            Assert.NotNull(x.CreationTime);
            #endregion
        }

        [Fact]
        public void ModificationTime_Test()
        {
            var productUnitRepository = unitOfWork.GetRepository<ProductUnit>();
            var productUnit = new ProductUnit
            {
                Name = "Test Unit"
            };
            productUnitRepository.InsertAsync(productUnit);
            productUnit.Name = "Updated Test Unit";
            productUnitRepository.Update(productUnit);
            unitOfWork.SaveChangesAsync();

            #region Assert
            var x = productUnitRepository.FindAsync(productUnit.Id).Result;
            Assert.NotNull(x.LastModificationTime);
            #endregion
        }

        [Fact]
        public void Delete_Test()
        {
            var productCategoryRepository = unitOfWork.GetRepository<ProductCategory>();
            
            var productCategory = new ProductCategory
            {
                Name = "Test Category",
            };
            productCategoryRepository.InsertAsync(productCategory);
            productCategoryRepository.Delete(productCategory.Id);
            unitOfWork.SaveChangesAsync();

            #region Assert
            var x = productCategoryRepository.FindAsync(productCategory.Id).Result;
            Assert.Null(x);
            #endregion
        }

        [Fact]
        public void SoftDelete_Test()
        {
            var productUnitRepository = unitOfWork.GetRepository<ProductUnit>();
            var productUnit = new ProductUnit
            {
                Name = "Test Unit"
            };
            productUnitRepository.InsertAsync(productUnit);
            productUnitRepository.Delete(productUnit.Id);
            unitOfWork.SaveChangesAsync();

            #region Assert
            var x = productUnitRepository.FindAsync(productUnit.Id).Result;
            Assert.True(x.IsDeleted);
            Assert.NotNull(x.DeletionTime);
            #endregion
        }
    }
}
