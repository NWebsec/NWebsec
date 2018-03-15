// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspSourcesElementCollection<T> : ConfigurationElementCollection where T : CspSourceConfigurationElement, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CspSourceConfigurationElement)element).Source;
        }

        public CspSourceConfigurationElement this[int index]
        {
            get => (CspSourceConfigurationElement)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(CspSourceConfigurationElement element)
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

        public void Remove(CspSourceConfigurationElement element)
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

        public int IndexOf(CspSourceConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        public bool IsRemoved(CspSourceConfigurationElement element)
        {
            return BaseIsRemoved(element);
        }
    }
}