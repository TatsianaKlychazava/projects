﻿using ApotekaShop.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApotekaShop.Services.Interfaces
{
    public interface IConfigurationSettingsProvider
    {
        void SetConfiguration(ConfigurationSettingsModel config);
        ConfigurationSettingsModel GetConfiguration();
    }
}
