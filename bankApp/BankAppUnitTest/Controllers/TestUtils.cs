using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Controllers.Tests
{
    public class TestUtils
    {
        public static void AssertRedirectToIndexHome(RedirectToRouteResult result)
        {
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["controller"], "Home");
        }
        public static T GetJsonValue<T>(JsonResult jsonResult, string propertyname)
        {
            var property = jsonResult.Data.GetType().GetProperties().FirstOrDefault(a => string.Compare(a.Name, propertyname) == 0);
            if (property == null)
                throw new Exception();
            return (T)property.GetValue(jsonResult.Data, null);
        }
    }
}