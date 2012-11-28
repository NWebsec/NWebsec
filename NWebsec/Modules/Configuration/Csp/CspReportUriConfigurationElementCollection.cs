// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspReportUriConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CspReportUriConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CspReportUriConfigurationElement)element).ReportUri;
        }

        public CspReportUriConfigurationElement this[int index]
        {
            get
            {
                return (CspReportUriConfigurationElement)BaseGet(index);
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

        public void Add(CspReportUriConfigurationElement element)
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

        public void Remove(CspReportUriConfigurationElement element)
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

        public int IndexOf(CspReportUriConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        public bool IsRemoved(CspReportUriConfigurationElement element)
        {
            return BaseIsRemoved(element);
        }
    }
}