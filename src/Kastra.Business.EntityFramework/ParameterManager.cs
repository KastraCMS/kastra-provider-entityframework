using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Kastra.Core.Services.Contracts;
using Kastra.Core.Constants;
using Kastra.Core.DTO;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

		/// <inheritdoc cref="IParameterManager.GetSiteConfigurationAsync"/>
		public async Task<SiteConfigurationInfo> GetSiteConfigurationAsync()
		{
            if (!_cacheEngine.GetCacheObject(SiteConfiguration.SiteConfigCacheKey, out SiteConfigurationInfo siteConfig))
            {
                siteConfig = _cacheEngine.SetCacheObject(SiteConfiguration.SiteConfigCacheKey, await LoadSiteConfigurationAsync());

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

		/// <inheritdoc cref="IParameterManager.SaveSiteConfigurationAsync(SiteConfigurationInfo)" />
		public async Task SaveSiteConfigurationAsync(SiteConfigurationInfo siteConfiguration)
		{
			Parameter parameter = null;
			List<Parameter> parameters = await _dbContext.KastraParameters.ToListAsync();

			foreach (PropertyInfo property in typeof(SiteConfigurationInfo).GetProperties())
			{
				if (parameters is not null)
				{
					parameter = parameters.SingleOrDefault(p => p.Key == property.Name);
				}
				
				if (parameters is null || parameter is null)
				{
					parameter = new ()
                    {
						Key = property.Name,
						Name = ((property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault()) as DisplayNameAttribute)?.DisplayName
                    };

					_dbContext.KastraParameters.Add(parameter);
				}

				parameter.Value = property.GetValue(siteConfiguration)?.ToString();
			}

			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Load the site configuration.
		/// </summary>
		/// <returns>Site configuration</returns>
		private async Task<SiteConfigurationInfo> LoadSiteConfigurationAsync()
		{
			string obj = null;
			TypeConverter typeConverter = null;
			SiteConfigurationInfo siteConfiguration = new ();
			IDictionary<string, string> parameters = null;

			if (_dbContext is null)
            {
				return siteConfiguration;
            }

			parameters = await _dbContext.KastraParameters.ToDictionaryAsync(p => p.Key, p => p.Value);

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

                    await _dbContext.SaveChangesAsync();

                    continue;
                }

                obj = parameters[property.Name];

                typeConverter = TypeDescriptor.GetConverter(property.PropertyType);

				if (typeConverter is null)
                {
					continue;
                }

				property.SetValue(siteConfiguration, typeConverter.ConvertFromString(obj), (object[])null);
			}

			// Enable cache
			if(siteConfiguration.CacheActivated)
				_cacheEngine.EnableCache();

			return siteConfiguration;
		}

	}
}
