// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspReportUriConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ReportUriConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ReportUriConfigurationElement)element).ReportUri;
        }

        public ReportUriConfigurationElement this[int index]
        {
            get
            {
                return (ReportUriConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(ReportUriConfigurationElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public Object[] GetAllKeys()
        {
            return BaseGetAllKeys();
        }

        public void Remove(ReportUriConfigurationElement element)
        {
            BaseRemove(element);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public int IndexOf(ReportUriConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        public bool IsRemoved(ReportUriConfigurationElement element)
        {
            return BaseIsRemoved(element);
        }
    }
}