using System;
using Autofac;
using R2.Domain.Context;
using R2.Domain.Repository;

namespace R2.Framework.Domain.Helpers
{
    public class AutofacModuleConfig : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            //builder.RegisterType<DataContext>().As<IDataContext>().SingleInstance();
            base.Load(builder);
        }
    }
}