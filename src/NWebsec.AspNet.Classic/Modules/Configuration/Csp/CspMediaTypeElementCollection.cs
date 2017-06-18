// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspMediaTypeElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CspMediaTypeConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CspMediaTypeConfigurationElement)element).MediaType;
        }

        public CspMediaTypeConfigurationElement this[int index]
        {
            get
            {
                return (CspMediaTypeConfigurationElement)BaseGet(index);
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

        public void Add(CspMediaTypeConfigurationElement element)
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

        public void Remove(CspMediaTypeConfigurationElement element)
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

        public int IndexOf(CspMediaTypeConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        public bool IsRemoved(CspMediaTypeConfigurationElement element)
        {
            return BaseIsRemoved(element);
        }
    }
}