using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetInstaller
{
    public class ProductSetup : AssetSetup
    {
        bool initialCanInstall;
        public bool InitialCanInstall
        {
            get => initialCanInstall;
            set => SetValue(ref initialCanInstall, value);
        }

        public ProductSetup(string resourceName, string productName, string productVersion) : base (resourceName, productName, productVersion)
        {
            InitialCanInstall = true;
        }
    }
}
