using Albelli.Assessment.Core.Ordering.Implementations;
using Albelli.Assessment.Core.Ordering.Interfaces;
using Albelli.Assessment.Infrastructure.Ordering.Implementations;
using Albelli.Assessment.Infrastructure.Ordering.Interfaces;
using Autofac;

namespace Albelli.Assessment.WebApi
{
    public class AutofacConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            var dbName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Albelli.Assessment.WebApi.SQLite.db3");

            builder.Register(c => new OrderDal(dbName)).As<IOrderDal>().SingleInstance();
            builder.RegisterType<OrderBl>().As<IOrderBl>().SingleInstance();

        }
    }
}
