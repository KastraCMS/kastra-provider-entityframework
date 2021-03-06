﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Kastra.Core.Services.Contracts;
using Kastra.Core.Constants;
using Kastra.Core.DTO;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business
{
	public class ParameterManager : IParameterManager
	{
		private readonly KastraDbContext _dbContext;
		private readonly CacheEngine _cacheEngine;

        public ParameterManager(KastraDbContext dbContext, CacheEngine cacheEngine)
		{
            _cacheEngine = cacheEngine;
			_dbContext = dbContext;
		}

		public SiteConfigurationInfo GetSiteConfiguration()
		{
			SiteConfigurationInfo siteConfig = null;

			if (!_cacheEngine.GetCacheObject(SiteConfiguration.SiteConfigCacheKey, out siteConfig))
            {
				siteConfig = _cacheEngine.SetCacheObject(SiteConfiguration.SiteConfigCacheKey, LoadSiteConfiguration());

				if (siteConfig is not null && siteConfig.CacheActivated)
                {
					_cacheEngine.EnableCache();
                }
				else
                {
					_cacheEngine.DisableCache();
                }
            }

			return siteConfig;
		}

		private SiteConfigurationInfo LoadSiteConfiguration()
		{
			string obj = null;
			TypeConverter typeConverter = null;
			SiteConfigurationInfo siteConfiguration = new SiteConfigurationInfo();
			IDictionary<string, string> parameters = null;

			if (_dbContext == null)
				return siteConfiguration;

			parameters = _dbContext.KastraParameters.ToDictionary(p => p.Key, p => p.Value);

			foreach (PropertyInfo property in typeof(SiteConfigurationInfo).GetProperties())
			{
                // Set default value if the value does not exist in database
                if(parameters == null || !parameters.ContainsKey(property.Name))
                {
                    //Save parameter
                    _dbContext.KastraParameters.Add(new Parameter() 
                    { 
                        Key = property.Name, 
                        Value = property.GetValue(siteConfiguration)?.ToString() 
                    });

                    _dbContext.SaveChanges();

                    continue;
                }

                obj = parameters[property.Name];

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
			Parameter parameter = null;
			List<Parameter> parameters = _dbContext.KastraParameters.ToList();

			foreach (PropertyInfo property in typeof(SiteConfigurationInfo).GetProperties())
			{
				if (parameters != null)
				{
					parameter = parameters.SingleOrDefault(p => p.Key == property.Name);
				}
				
				if (parameters == null || parameter == null)
				{
					parameter = new Parameter();
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
