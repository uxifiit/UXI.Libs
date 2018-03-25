using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common.UI
{
    public static class DesignTimeHelper
    {
        private static bool? isDesignTime = null;
        public static bool IsDesignTime
        {
            get
            {
                if (isDesignTime.HasValue == false)
                {
                    isDesignTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());
                }
                return isDesignTime.Value;
            }
        }
    }
}
