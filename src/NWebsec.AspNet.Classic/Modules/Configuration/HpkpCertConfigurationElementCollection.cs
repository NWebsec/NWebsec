// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class HpkpCertConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HpkpCertConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HpkpCertConfigurationElement)element).ThumbPrint;
        }

        public HpkpCertConfigurationElement this[int index]
        {
            get
            {
                return (HpkpCertConfigurationElement)BaseGet(index);
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

        public void Add(HpkpCertConfigurationElement element)
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

        public void Remove(HpkpCertConfigurationElement element)
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

        public int IndexOf(HpkpCertConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        public bool IsRemoved(HpkpCertConfigurationElement element)
        {
            return BaseIsRemoved(element);
        }
    }
}