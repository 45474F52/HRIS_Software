using HRIS_Software.Core;
using HRIS_Software.Models.Converters;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HRIS_Software.Models
{
    internal class DynamicItem : ObservableObject
    {
        [JsonIgnore]
        private bool _isDirty;
        public bool IsDirty
        {
            get => _isDirty;
            private protected set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title { get; }
        public string Description { get; }
        [JsonConverter(typeof(TypeJsonConverter))]
        public Type Type { get; }

        public DynamicItem(string title, string description, Type type, dynamic value = null)
        {
            Title = title;
            Description = description;
            Type = type;

            try
            {
                Value = ((JsonElement)value).Deserialize(type);
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                Value = value;
            }

            ResetDirty();
        }

        private dynamic _oldValue;
        private dynamic _value;
        public dynamic Value
        {
            get => _value;
            set
            {
                if (IsEquals(in _oldValue, in value))
                {
                    _value = _oldValue;
                    IsDirty = false;
                }
                else
                {
                    _oldValue = _value ?? value;
                    _value = value;
                    IsDirty = true;
                }
            }
        }

        public void ResetDirty()
        {
            _oldValue = _value;
            IsDirty = false;
        }

        private bool IsEquals(in dynamic value, in dynamic target) =>
            Type.IsEnum && value != null ? (int)target == (int)value : target.Equals(value);
    }
}
