using Elekta.Code.Challenge.Api.Models;
using Elekta.Code.Challenge.Api.Models.External;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Elekta.Code.Challenge.Api.Tests.Helper
{
    public static class FileHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string fileName)
        {

            var parentPath = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\";
            return new ConfigurationBuilder()
                .SetBasePath(parentPath)
                .AddJsonFile(fileName, optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static EquipmentSetting GetEquipmentSettingConfiguration(string fileName)
        {
            var equipmentSetting = new EquipmentSetting();

            var iConfig = GetIConfigurationRoot(fileName);

            iConfig
                .GetSection("EquipmentSetting")
                .Bind(equipmentSetting);

            return equipmentSetting;
        }

        public static EmailSetting GetEmailSettingConfiguration(string fileName)
        {
            var emailSetting = new EmailSetting();

            var iConfig = GetIConfigurationRoot(fileName);

            iConfig
                .GetSection("EmailSetting")
                .Bind(emailSetting);

            return emailSetting;
        }
    }
}
