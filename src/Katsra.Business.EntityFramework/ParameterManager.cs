using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Kastra.Core;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business
{
	public class ParameterManager : IParameterManager
	{
		private readonly KastraContext _dbContext = null;
		private readonly CacheEngine _cacheEngine = null;

        public ParameterManager(KastraContext dbContext, CacheEngine cacheEngine)
		{
            _cacheEngine = cacheEngine;
			_dbContext = dbContext;
		}

		public SiteConfigurationInfo GetSiteConfiguration()
		{
			SiteConfigurationInfo siteConfig = null;

			if (!_cacheEngine.GetCacheObject(Constants.SiteConfig.SiteConfigCacheKey, out siteConfig))
				siteConfig = _cacheEngine.SetCacheObject(Constants.SiteConfig.SiteConfigCacheKey, LoadSiteConfiguration());

			return siteConfig;
		}

		private SiteConfigurationInfo LoadSiteConfiguration()
		{
			String obj = null;
			TypeConverter typeConverter = null;
			SiteConfigurationInfo siteConfiguration = new SiteConfigurationInfo();
			IDictionary<String, String> parameters = null;

			if (_dbContext == null)
				return siteConfiguration;

			parameters = _dbContext.KastraParameters.ToDictionary(p => p.Key, p => p.Value);

			if (parameters == null || parameters.Count == 0)
				return siteConfiguration;

			foreach (PropertyInfo property in typeof(SiteConfigurationInfo).GetProperties())
			{
				obj = parameters[property.Name];

				if (String.IsNullOrEmpty(obj))
					continue;

				typeConverter = TypeDescriptor.GetConverter(property.PropertyType);

				if (typeConverter == null)
					continue;

				property.SetValue(siteConfiguration, typeConverter.ConvertFromString(obj), (object[])null);
			}

			// Enable cache
			if(siteConfiguration.CacheActivated)
				_cacheEngine.EnableCache();

			return siteConfiguration;
		}

		public void SaveSiteConfiguration(SiteConfigurationInfo siteConfiguration)
		{
			KastraParameters parameter = null;
			List<KastraParameters> parameters = _dbContext.KastraParameters.ToList();

			foreach (PropertyInfo property in typeof(SiteConfigurationInfo).GetProperties())
			{
				if (parameters == null || (parameter = parameters.SingleOrDefault(p => p.Key == property.Name)) == null)
				{
					parameter = new KastraParameters();
					parameter.Key = property.Name;
					parameter.Name = ((property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault()) as DisplayNameAttribute)?.DisplayName;

					_dbContext.KastraParameters.Add(parameter);
				}

				parameter.Value = property.GetValue(siteConfiguration)?.ToString();
			}

			_dbContext.SaveChanges();
		}
	}
}
