// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class HpkpPinConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HpkpPinConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HpkpPinConfigurationElement)element).Pin;
        }

        public HpkpPinConfigurationElement this[int index]
        {
            get
            {
                return (HpkpPinConfigurationElement)BaseGet(index);
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

        public void Add(HpkpPinConfigurationElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public object[] GetAllKeys()
        {
            return BaseGetAllKeys();
        }

        public void Remove(HpkpPinConfigurationElement element)
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

        public int IndexOf(HpkpPinConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        public bool IsRemoved(HpkpPinConfigurationElement element)
        {
            return BaseIsRemoved(element);
        }
    }
}