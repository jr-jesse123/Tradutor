﻿using FastLegenda.Core;
using FastLegenda.Modules.ModuleName.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace FastLegenda.Modules.ModuleName
{
    public class ModuleNameModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleNameModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "ViewA");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>();
        }
    }
}