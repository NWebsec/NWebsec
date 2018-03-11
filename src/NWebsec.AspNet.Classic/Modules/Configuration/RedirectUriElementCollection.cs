// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class RedirectUriElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RedirectUriConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RedirectUriConfigurationElement)element).RedirectUri;
        }

        public RedirectUriConfigurationElement this[int index]
        {
            get => (RedirectUriConfigurationElement)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(RedirectUriConfigurationElement element)
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

        public void Remove(RedirectUriConfigurationElement element)
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

        public int IndexOf(RedirectUriConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        public bool IsRemoved(RedirectUriConfigurationElement element)
        {
            return BaseIsRemoved(element);
        }
    }
}