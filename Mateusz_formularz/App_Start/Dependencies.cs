
using Autofac;
using Autofac.Integration.Mvc;
using Mateusz_formularz.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mateusz_formularz.App_Start
{
    public static class Dependencies
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<LectionsSqlRepository>().As<LectionsRepository>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}