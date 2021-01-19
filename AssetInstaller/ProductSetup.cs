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

        bool initialCanRepair;
        public bool InitialCanRepair
        {
            get => initialCanRepair;
            set => SetValue(ref initialCanRepair, value);
        }

        bool initialCanUnInstall;
        public bool InitialCanUnInstall
        {
            get => initialCanUnInstall;
            set => SetValue(ref initialCanUnInstall, value);
        }

        public ProductSetup()
        {
            InitialCanInstall = true;
            InitialCanRepair = InitialCanUnInstall = false;
        }
    }
}
